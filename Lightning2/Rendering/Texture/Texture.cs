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
        private string? _path;

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
        public nint Handle { get; internal set; }

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
        internal nint FormatHandle { get; set; }

        /// <summary>
        /// Determines if this texture can be used as a a render target.
        /// </summary>
        public bool IsTarget { get; set; }

        public byte _opacity;

        /// <summary>
        /// SDL modulus for alpha
        /// </summary>
        public byte Opacity
        {
            get
            {
                return _opacity;
            }
            internal set
            {
                if (!Loaded)
                {
                    Logger.LogError("Attempted to set the opacity of an unloaded Texture - please load it first!. \n" +
                        "The Opacity will not be changed until you load the texture.", 161, LoggerSeverity.Warning, null, false);
                    return;
                }

                _opacity = value;
                
                // slower than the old method but renderer independent
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int x = 0; x < Size.X; x++)
                    {
                        Color orig = GetPixel(x, y);

                        if (orig.A != _opacity) SetPixel(x, y, Color.FromArgb(_opacity, orig.R, orig.G, orig.B));
                    }
                }
            }
        }

        /// <summary>
        /// Initialises a new texture with the size specified in the <paramref name="nSize"/> parameter.
        /// </summary>
        /// <param name="sizeX">The width of the texture in pixels.</param>
        /// <param name="sizeY">The height of the texture in pixels.</param>
        public Texture(string name, float sizeX, float sizeY, bool isTarget = false, string path = CREATED_TEXTURE_PATH) : base(name)
        {
            Size = new Vector2(sizeX, sizeY);

            IsTarget = isTarget;

            if (Size == default) Logger.LogError($"Error creating texture: Must have a size!", 20, LoggerSeverity.FatalError);

            Handle = Lightning.Renderer.CreateTexture((int)sizeX, (int)sizeY, isTarget);
            FormatHandle = Lightning.Renderer.AllocTextureFormat();
            Loaded = Handle != nint.Zero
                && FormatHandle != nint.Zero;
            Path = path;
        }

        public override void Create()
        {
            // not great
            if (Path == CREATED_TEXTURE_PATH)
            {
                Loaded = true;
            }
            else
            {
                // missing texture will be loaded if it fails to load

                Handle = Lightning.Renderer.LoadTexture(Path);
                Loaded = (Handle != default);
            }
        }

        /// <summary>
        /// Gets the pixel at coordinates <see cref="X"/>,<see cref="Y"/>.
        /// </summary>
        /// <param name="x">The X coordinate of the pixel to acquire.</param>
        /// <param name="y">The Y coordinate of the pixel to acquire.</param>
        /// <param name="unlockNow">Unlocks the texture immediately - use this if you do not need to draw any more pixels</param>
        /// <returns>A <see cref="Color"/> instance containing the color data of the pixel acquired</returns>
        /// <exception cref="NCError">An invalid coordinate was supplied or the texture does not have a valid size.</exception>
        public virtual Color GetPixel(int x, int y, bool unlockNow = false)
        {
            // do not lock it if we are already locked
            if (!Locked) Lock();

            if (x < 0 || y < 0
                || x > Size.X || y > Size.Y) Logger.LogError($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({x},{y})," +
                    $" min (0,0). max ({Size.X},{Size.Y})!", 12, LoggerSeverity.FatalError);

            int pixelToGet = y * (int)Size.X + x;
            int maxPixelID = Pitch / 4 * Pitch;

            if (pixelToGet > maxPixelID) Logger.LogError($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({x},{y}), " +
                $"min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {pixelToGet} > {maxPixelID}!)", 14, LoggerSeverity.FatalError);

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
        /// <exception cref="NCError">An invalid coordinate was supplied or the texture does not have a valid size.</exception>
        public virtual void SetPixel(int x, int y, Color color, bool unlockNow = false)
        {
            // do not lock it if we are already locked
            if (!Locked) Lock();

            if (x < 0 || y < 0
                || x >= Size.X || y >= Size.Y) Logger.LogError($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({x},{y}), " +
                    $"min (0,0). max ({Size.X},{Size.Y}) ", 15, LoggerSeverity.FatalError);

            int pixelToGet = (y * (int)Size.X) + x;
            int maxPixelId = (int)((Size.X * 4) * Size.Y); 

            if (pixelToGet > maxPixelId) Logger.LogError($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({x},{y}), " +
                $"min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {pixelToGet} > {maxPixelId}!)", 16, LoggerSeverity.FatalError);

            // use pixeltoget to twiddle the pixel that we need using the number we calculated before
            Pixels[pixelToGet] = color.ToArgb();

            // unlock the texture if unlocknow specified
            if (unlockNow) Unlock();
        }

        /// <summary>
        /// Locks this texture so that its pixels can be modified.
        /// </summary>
        /// <exception cref="NCError">An error occurred locking this pixel's texture.</exception>
        public void Lock()
        {
            // do nothing if we are calling this on an already locked texture
            if (Locked) return;

            Lightning.Renderer.LockTexture(Handle, new(0, 0), new(Size.X, Size.Y), out var pixels, out var pitch);

            Pitch = pitch;
            // convert to C pointer
            Pixels = (int*)pixels.ToPointer();
            Locked = true;
        }

        /// <summary>
        /// Unlocks this texture so that it can be rendered.
        /// </summary>
        public void Unlock()
        {
            // don't unlock if already unlocked
            if (!Locked) return;

            Lightning.Renderer.UnlockTexture(Handle);

            // these values are now invalid 
            Pixels = null;

            Pitch = 0;

            Locked = false;
        }

        /// <summary>
        /// Draws this texture instance.
        /// </summary>
        /// <exception cref="NCError">An error occurred rendering the texture. Extended information is available in <see cref="NCError.Description"/></exception>
        public override void Draw()
        {
            if (!Loaded
                && _path != null) Logger.LogError($"Texture {Path} being drawn without being loaded, you will see a black box!", 94, 
                    LoggerSeverity.Warning, null, true); // don't show a message box

            Unlock();

            // failsafe just in case of any weird stuff happening
            if (RenderPosition == default) RenderPosition = Position;

            Lightning.Renderer.DrawTexture(ViewportStart, ViewportEnd, RenderPosition, Size, Handle, Repeat);
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
        /// Sets the opacity of this texture. This will apply an alpha colour modulation of the texture where the alpha value is set to the value of the <see cref="Opacity"/> parameter.
        /// </summary>
        /// <param name="opacity">A <see cref="byte"/> determinig the opacity to set</param>
        public void SetOpacity(byte opacity) => Opacity = opacity;

        /// <summary>
        /// Sets the blend mode of this texture. See the documentation for the <see cref="SDL_BlendMode"/> enum.
        /// </summary>
        /// <param name="blendMode">The <see cref="SDL_BlendMode"/> to set the texture to.</param>
        public void SetBlendMode(SDL_BlendMode blendMode)
        {
            if (Lightning.Renderer is SdlRenderer)
            {
                Lightning.Renderer.SetTextureBlendMode(Handle, blendMode);
            }
            
        }

        /// <summary>
        /// Unloads this texture.
        /// </summary>
        public override void Destroy()
        {
            Loaded = false;
            Handle = Lightning.Renderer.DestroyTexture(Handle);
            FormatHandle = nint.Zero;
            Pitch = 0;
            Pixels = null;
        }
    }
}