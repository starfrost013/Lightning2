using static NuCore.SDL2.SDL_image;

namespace LightningGL
{
    /// <summary>
    /// Texture
    /// 
    /// February 11, 2022
    /// 
    /// A texture api allowing modification of individual pixels
    /// </summary>
    public unsafe class Texture : Renderable
    {
        internal const string CREATED_TEXTURE_PATH = "<<<CREATED TEXTURE>>>";

        /// <summary>
        /// Backing field for <see cref="Path"/>
        /// </summary>
        private string _path;

        /// <summary>
        /// Path to the texture 
        /// </summary>
        public string Path
        {
            get
            {
                if (_path == null)
                {
                    return CREATED_TEXTURE_PATH;
                }
                else
                {
                    return _path;
                }
            }
            set
            {
                _path = value;
            }
        }

        /// <summary>
        /// Pointer to unmanaged memory for the SDL_Texture of this Texture.
        /// </summary>
        public IntPtr Handle { get; internal set; }

        /// <summary>
        /// Pointer to unmanaged SDL_Texture pixels.
        /// 
        /// NULL if not locked.
        /// </summary>
        public int* Pixels { get; private set; }

        /// <summary>
        /// Determines if this texture repeats, and if so
        /// by how many tiles. 
        /// 
        /// If NULL or zero, the texture will be drawn only once.
        /// </summary>
        public Vector2 Repeat { get; set; }

        /// <summary>
        /// The start of the viewport of this texture.
        /// 
        /// If either ViewportStart or ViewportEnd are null, the entire texture will be drawn.
        /// Otherwise, will draw the texture from (ViewportStart) to (ViewportEnd)
        /// </summary>
        public Vector2 ViewportStart { get; set; }

        /// <summary>
        /// The viewport of this texture.
        /// 
        /// If either ViewportStart or ViewportEnd are null, the entire texture will be drawn.
        /// Otherwise, will draw the texture from (ViewportStart) to (ViewportEnd)
        /// </summary>
        public Vector2 ViewportEnd { get; set; }

        /// <summary>
        /// Updated when the texture is locked in C++. Used to determine if the pixels can be acquired.
        /// </summary>
        public bool Locked { get; private set; }

        /// <summary>
        /// The pitch of this texture
        /// </summary>
        public int Pitch { get; private set; }

        /// <summary>
        /// Private: Texture format allocated for internal use
        /// </summary>
        internal IntPtr FormatHandle { get; set; }

        /// <summary>
        /// Types of texture acess.
        /// </summary>
        public SDL_TextureAccess Access { get; internal set; }

        /// <summary>
        /// Initialises a new texture with the size specified in the <paramref name="nSize"/> parameter.
        /// </summary>
        /// <param name="sizeX">The width of the texture in pixels.</param>
        /// <param name="sizeY">The height of the texture in pixels.</param>
        public Texture(Renderer cRenderer, float sizeX, float sizeY, SDL_TextureAccess access = SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING)
        {
            Size = new Vector2(sizeX, sizeY);
            Access = access;

            if (Size == default) _ = new NCException($"Error creating texture: Must have a size!", 20, "Texture constructor called with invalid size", NCExceptionSeverity.FatalError);

            Handle = SDL_CreateTexture(cRenderer.Settings.RendererHandle, SDL_PIXELFORMAT_ARGB8888, Access, (int)Size.X, (int)Size.Y);

            // check if texture failed to load
            if (Handle == IntPtr.Zero) _ = new NCException($"Error creating texture: {SDL_GetError()}", 119, "An SDL error occurred in the Texture constructor", NCExceptionSeverity.FatalError);

            OnRender += Draw;

            Init_AllocFormat(cRenderer);
        }

        /// <summary>
        /// Loads the texture for the window <see cref="cRenderer"/>
        /// </summary>
        /// <param name="cRenderer">The window to load the texture on.</param>
        /// <exception cref="NCException">An error occurred loading the texture.</exception>
        internal override void Load(Renderer cRenderer)
        {
            if (Path == CREATED_TEXTURE_PATH)
            {
                // not great
                Loaded = true;
                return;
            }

            if (!File.Exists(Path)) _ = new NCException($"{Path} does not exist!", 9, "Texture::Path property does not exist", NCExceptionSeverity.FatalError);

            Handle = IMG_LoadTexture(cRenderer.Settings.RendererHandle, Path);

            if (Handle == IntPtr.Zero)
            {
                _ = new NCException($"Failed to load texture at {Path} - {SDL_GetError()}", 10, "An SDL error occurred in Texture::Load!", NCExceptionSeverity.Error);
            }
            else
            {
                Loaded = true;
            }
        }

        /// <summary>
        /// Private method that allocates a texture format based on the window pixel format for this texture during loading. 
        /// </summary>
        /// <param name="cRenderer">The window to allocate the texture format for this texture.</param>
        /// <exception cref="NCException">An error occurred while allocating a texture format.</exception>
        private void Init_AllocFormat(Renderer cRenderer)
        {
            uint currentFormat = SDL_GetWindowPixelFormat(cRenderer.Settings.WindowHandle);

            FormatHandle = SDL_AllocFormat(currentFormat);

            // probably not the best to actually like, allocate formats like this
            if (FormatHandle == IntPtr.Zero) _ = new NCException($"Error allocating texture format for texture at {Path}: {SDL_GetError()}", 13, "An SDL error occurred in Texture::Init_AllocFormat", NCExceptionSeverity.FatalError);
        }

        /// <summary>
        /// Gets the pixel at coordinates <see cref="X"/>,<see cref="Y"/>.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to acquire.</param>
        /// <param name="y">The Y coordinate of the pixel to acquire.</param>
        /// <param name="unlockNow">Unlocks the texture immediately - use this if you do not need to draw any more pixels</param>
        /// <returns>A <see cref="Color"/> instance containing the color data of the pixel acquired</returns>
        /// <exception cref="NCException">An invalid coordinate was supplied or the texture does not have a valid size.</exception>
        public virtual Color GetPixel(int x, int y, bool unlockNow = false)
        {
            // do not lock it if we are already locked
            if (!Locked) Lock();

            if (x < 0 || y < 0
                || x > Size.X || y > Size.Y) _ = new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({x},{y}), min (0,0). max ({Size.X},{Size.Y})!", 12, "An SDL error occurred in Texture::GetPixel", NCExceptionSeverity.FatalError);

            int pixelToGet = y * (int)Size.X + x;
            int maxPixelID = Pitch / 4 * Pitch;

            if (pixelToGet > maxPixelID) _ = new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({x},{y}), min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {pixelToGet} > {maxPixelID}!)", 14, "An SDL error occurred in Texture::GetPixel", NCExceptionSeverity.FatalError);

            int pixel = Pixels[pixelToGet];

            if (unlockNow) Unlock();

            return Color.FromArgb(pixel);
        }

        /// <summary>
        /// Sets the pixel at coordinates <see cref="X"/>,<see cref="Y"/> to the color specified by the <see cref="color"/> parameter.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to set.</param>
        /// <param name="y">The Y coordinate of the pixel to set.</param>
        /// <param name="unlockNow">Unlocks the texture immediately - use this if you do not need to draw any more pixels</param>
        /// <exception cref="NCException">An invalid coordinate was supplied or the texture does not have a valid size.</exception>
        public virtual void SetPixel(int x, int y, Color color, bool unlockNow = false)
        {
            // do not lock it if we are already locked
            if (!Locked) Lock();

            if (x < 0 || y < 0
                || x >= Size.X || y >= Size.Y) _ = new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({x},{y}), min (0,0). max ({Size.X},{Size.Y}) ", 15, "An SDL error occurred in Texture::SetPixel", NCExceptionSeverity.FatalError);

            int pixelToGet = (y * (int)Size.X) + x;
            int maxPixelId = Pitch / 4 * Pitch;

            if (pixelToGet > maxPixelId) _ = new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({x},{y}), min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {pixelToGet} > {maxPixelId}!)", 16, "An SDL error occurred in Texture::SetPixel", NCExceptionSeverity.FatalError);

            // use pixeltoget to twiddle the pixel that we need using the number we calculated before
            Pixels[pixelToGet] = color.ToArgb();

            // unlock the texture if unlocknow specified
            if (unlockNow) Unlock();
        }

        /// <summary>
        /// Locks this texture so that its pixels can be modified.
        /// </summary>
        /// <exception cref="NCException">An error occurred locking this pixel's texture.</exception>
        public void Lock()
        {
            // do nothing if we are calling this on an already locked texture
            if (Locked) return;

            Locked = true;

            SDL_Rect rect = new SDL_Rect(0, 0, (int)Size.X, (int)Size.Y);

            if (SDL_LockTexture(Handle, ref rect, out var nPixels, out var nPitch) < 0) _ = new NCException($"Error locking pixels for texture with path {Path}: {SDL_GetError()}.", 11, "An SDL error occurred in Texture::Lock", NCExceptionSeverity.FatalError);

            Pitch = nPitch;
            // convert to C pointer
            Pixels = (int*)nPixels.ToPointer();
        }

        /// <summary>
        /// Unlocks this texture so that it can be rendered.
        /// </summary>
        public void Unlock()
        {
            // don't unlock if already unlocked
            if (!Locked) return;

            Locked = false;

            SDL_UnlockTexture(Handle);

            // these values are now invalid 
            Pixels = null;

            Pitch = 0;
        }

        /// <summary>
        /// Draws this texture instance.
        /// </summary>
        /// <param name="cRenderer">The window to draw this texture to.</param>
        /// <exception cref="NCException">An error occurred rendering the texture. Extended information is available in <see cref="NCException.Description"/></exception>
        internal override void Draw(Renderer cRenderer)
        {
            if (!Loaded
                && _path != null) _ = new NCException($"Texture {Path} being drawn without being loaded, you will see a black box!", 94, "Texture with image not loaded (Texture::Loaded = false)", NCExceptionSeverity.Warning, null, true); // don't show a message box

            Unlock();

            SDL_Rect sourceRect = new SDL_Rect();
            SDL_FRect destinationRect = new SDL_FRect();

            // failsafe just in case of any weird stuff happening
            if (RenderPosition == default(Vector2)) RenderPosition = Position;

            // Draw to the viewpoint
            if (ViewportStart == default
                && ViewportEnd == default)
            {
                sourceRect.x = 0;
                sourceRect.y = 0;
                sourceRect.w = (int)Size.X;
                sourceRect.h = (int)Size.Y;

                destinationRect.x = RenderPosition.X;
                destinationRect.y = RenderPosition.Y;
                destinationRect.w = Size.X;
                destinationRect.h = Size.Y;
            }
            else
            {
                sourceRect.x = (int)ViewportStart.X;
                sourceRect.y = (int)ViewportStart.Y;
                sourceRect.w = (int)(ViewportEnd.X - ViewportStart.X);
                sourceRect.h = (int)(ViewportEnd.Y - ViewportStart.Y);

                destinationRect.x = RenderPosition.X;
                destinationRect.y = RenderPosition.Y;
                destinationRect.w = ViewportEnd.X - ViewportStart.X;
                destinationRect.h = ViewportEnd.Y - ViewportStart.Y;
            }

            Camera curCamera = cRenderer.Settings.Camera;

            if (curCamera != null
                && !SnapToScreen)
            {
                destinationRect.x -= curCamera.Position.X;
                destinationRect.y -= curCamera.Position.Y;
            }

            if (Repeat == default)
            {
                // call to SDL - we are simply drawing it once.
                SDL_RenderCopyF(cRenderer.Settings.RendererHandle, Handle, ref sourceRect, ref destinationRect);
            }
            else
            {
                SDL_FRect newRect = new SDL_FRect(destinationRect.x, destinationRect.y, destinationRect.w, destinationRect.h);

                // Draws a tiled texture.
                for (int y = 0; y < Repeat.Y; y++)
                {
                    SDL_RenderCopyF(cRenderer.Settings.RendererHandle, Handle, ref sourceRect, ref newRect);

                    for (int x = 0; x < Repeat.X; x++)
                    {
                        SDL_RenderCopyF(cRenderer.Settings.RendererHandle, Handle, ref sourceRect, ref newRect);

                        newRect.x += destinationRect.w;
                    }

                    newRect.y += destinationRect.h; // we already set it up
                    newRect.x = destinationRect.x;
                }
            }
        }

        /// <summary>
        /// Clears the texture with the color specified in the <paramref name="color"/> parameter. If this is not set, it will default to ARGB <c>0,0,0,0</c>.
        /// </summary>
        /// <param name="color">The optional color to clear the texture with.</param>
        public void Clear(Color color = default)
        {
            Color clearColor = Color.FromArgb(0, 0, 0, 0);

            if (color != default) clearColor = color;

            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    SetPixel(x, y, clearColor);
                }
            }
        }

        /// <summary>
        /// Sets the blend mode of this texture. See the documentation for the <see cref="SDL_BlendMode"/> enum.
        /// </summary>
        /// <param name="blendMode">The <see cref="SDL_BlendMode"/> to set the texture to.</param>
        public void SetBlendMode(SDL_BlendMode blendMode) => SDL_SetTextureBlendMode(Handle, blendMode);

        /// <summary>
        /// Unloads this texture.
        /// </summary>
        public void Unload()
        {
            Loaded = false;
            SDL_DestroyTexture(Handle);
            FormatHandle = IntPtr.Zero;
            Handle = IntPtr.Zero;
            Pitch = 0;
            Pixels = null;
        }
    }
}