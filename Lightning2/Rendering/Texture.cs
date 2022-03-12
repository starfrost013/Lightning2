using NuCore.SDL2;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightning2
{
    /// <summary>
    /// TextureAPI
    /// 
    /// February 11, 2022
    /// 
    /// A texture api allowing modification of individual pixels
    /// </summary>
    public unsafe class Texture
    {
        /// <summary>
        /// Backing field for <see cref="Path"/>
        /// </summary>
        private string _path { get; set; }


        /// <summary>
        /// Path to the texture 
        /// </summary>
        public string Path
        {
            get
            { 
                if (_path == null)
                {
                    return "<<<CREATED TEXTURE>>>";
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

        public IntPtr TextureHandle { get; set; }

        /// <summary>
        /// Pointer to unmanaged SDL_Texture pixels.
        /// 
        /// NULL if not locked.
        /// </summary>
        public IntPtr Pixels { get; set; }

        /// <summary>
        /// The position of this texture.
        /// Must be valid to draw.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The size of this texture. 
        /// Does not have to be equal to image size.
        /// </summary>
        public Vector2 Size { get; set; }

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
        /// If either ViewportStart or ViewportEnd are null or (0, 0), the entire texture will be drawn.
        /// Otherwise, will draw the texture from (ViewportStart) to (ViewportEnd)
        /// </summary>
        public Vector2 ViewportStart { get; set; }

        /// <summary>
        /// The viewport of this texture.
        /// 
        /// If either ViewportStart or ViewportEnd are null or (0, 0), the entire texture will be drawn.
        /// Otherwise, will draw the texture from (ViewportStart) to (ViewportEnd)
        /// </summary>
        public Vector2 ViewportEnd { get; set; }

        /// <summary>
        /// Updated when the texture is locked in C++. Used to determine if the pixels can be acquired.
        /// </summary>
        public bool Locked { get; set; }


        /// <summary>
        /// The pitch of this texture
        /// </summary>
        public int Pitch { get; set; }

        /// <summary>
        /// Determines if this object has been destroyed.
        /// </summary>
        private bool Destroyed { get; set; }

        /// <summary>
        /// Private: Texture format allocated for tAPI use
        /// </summary>
        private IntPtr CFormat { get; set; }

        
        /// <summary>
        /// Initialises a new texture with a size.
        /// </summary>
        /// <param name="X">The width of the texture in pixels.</param>
        /// <param name="Y">The height of the texture in pixels.</param>
        public Texture(int X, int Y)
        {
            Size = new Vector2(X, Y);
        }

        public void Create(Window CWindow)
        {
            if (Size == null) throw new NCException($"Error creating texture: Must have a size!", 20, "Texture.Create", NCExceptionSeverity.FatalError);

            TextureHandle = SDL.SDL_CreateTexture(CWindow.Settings.RendererHandle, SDL.SDL_PIXELFORMAT_RGBA8888, SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, Size.X, Size.Y);

            // check if texture failed to load
            if (TextureHandle == IntPtr.Zero)
            {
                throw new NCException($"Error creating texture: {SDL.SDL_GetError()}", 11, "Texture.Create", NCExceptionSeverity.FatalError);
            }

            Init_AllocFormat(CWindow);
        }
        public void Load(Window CWindow)
        {
            if (!File.Exists(Path)) throw new NCException($"{Path} does not exist!", 9, "!File.Exists(Texture.Path)!", NCExceptionSeverity.FatalError);

            TextureHandle = SDL_image.IMG_LoadTexture(CWindow.Settings.RendererHandle, Path);

            if (TextureHandle == IntPtr.Zero) throw new NCException($"Failed to load texture at {Path} - {SDL.SDL_GetError()}", 10, "Error in SDL_image.IMG_LoadTexture", NCExceptionSeverity.Error);
        }

        private void Init_AllocFormat(Window CWindow)
        {
            uint CurFormat = SDL.SDL_GetWindowPixelFormat(CWindow.Settings.WindowHandle);

            CFormat = SDL.SDL_AllocFormat(CurFormat);

            // probably not the best to actually like, allocate formats like this
            if (CFormat == IntPtr.Zero)
            {
                throw new NCException($"Error allocating texture format for texture at {Path}: {SDL.SDL_GetError()}", 13, "Texture.Init_AllocFormat", NCExceptionSeverity.FatalError);
            }
        }

        /// <summary>
        /// Gets the pixel at coordinates <see cref="X"/>,<see cref="Y"/>.
        /// </summary>
        /// <param name="X">The X coordinate of the pixel to acquire.</param>
        /// <param name="Y">The Y coordinate of the pixel to acquire.</param>
        /// <param name="UnlockNow">Unlocks the texture immediately - use this if you do not need to draw any more pixels</param>
        /// <returns>A <see cref="Color4"/> instance containing the colour data of the pixel acquired</returns>
        /// <exception cref="NCException">An invalid coordinate was supplied or the texture does not have a valid size.</exception>
        public Color4 GetPixel(int X, int Y, bool UnlockNow = false)
        {
            if (Size == null) throw new NCException($"Invalid size - cannot get pixel!", 16, "Texture.GetPixel", NCExceptionSeverity.FatalError);

            if (!Locked) Lock();

            if (X < 0 || Y < 0
                || X > Size.X || Y > Size.Y)
            {
                throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y})!", 12, "Texture.GetPixel", NCExceptionSeverity.FatalError);
            }

            int PixelToGet = (X / 4) * Y;
            int MaxPixelID = (Pitch / 4) * Y;

            if (PixelToGet > MaxPixelID)
            {
                throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {PixelToGet} > {MaxPixelID}!", 14, "Texture.GetPixel", NCExceptionSeverity.FatalError);
            }

            // CS0211 
            // does not matter as still points to same place
            IntPtr npixels = Pixels;
            IntPtr* PixelPtr = &npixels;

            uint NP = (uint)PixelPtr[PixelToGet];

            if (UnlockNow) Unlock();

            return (Color4)NP;
        }

        /// <summary>
        /// Sets the pixel at coordinates <see cref="X"/>,<see cref="Y"/> to the colour specified by the <see cref="Colour"/> parameter.
        /// </summary>
        /// <param name="X">The X coordinate of the pixel to set.</param>
        /// <param name="Y">The Y coordinate of the pixel to set.</param>
        /// <param name="UnlockNow">Unlocks the texture immediately - use this if you do not need to draw any more pixels</param>
        /// <exception cref="NCException">An invalid coordinate was supplied or the texture does not have a valid size.</exception>
        public void SetPixel(int X, int Y, Color4 Colour, bool UnlockNow = false)
        {
            if (!Locked) Lock();

            if (X < 0 || Y < 0
                || X > Size.X || Y > Size.Y)
            {
                throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) ", 15, "Texture.GetPixel", NCExceptionSeverity.FatalError);
            }
            
            int MaxPixelID = (Pitch / 4) * Size.Y;
            int PixelToGet = (Y * Size.X) + X; 

            if (PixelToGet > MaxPixelID)
            {
                throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {PixelToGet} > {MaxPixelID}!", 16, "Texture.GetPixel", NCExceptionSeverity.FatalError);
            }
            
            // convert to C pointer
            uint* npixels = (uint*)Pixels.ToPointer();

            // use pixeltoget to twiddle the pixel that we need using the number we calculated before
            npixels[PixelToGet] = (uint)Colour;

            // unlock the texture if unlocknow specified
            if (UnlockNow) Unlock();
        }

        public void Lock()
        {
            if (Locked) return;
            Locked = true;

            // hack to get around CS0206
            // https://docs.microsoft.com/en-us/dotnet/csharp/misc/cs0206
            // should work around this by not having out/ref,
            // but not sure if this is feasible wrt C++/PInvoke

            IntPtr npixels = Pixels;
            SDL.SDL_Rect rect = new SDL.SDL_Rect(0, 0, Size.X, Size.Y);
            int npitch = Pitch;
           
            if (SDL.SDL_LockTexture(TextureHandle, ref rect, out npixels, out npitch) < 0)
            {
                throw new NCException($"Error locking pixels for texture with path {Path}, error {SDL.SDL_GetError()}.", 11, "Texture.Lock", NCExceptionSeverity.FatalError);
            }

            Pitch = npitch;
            Pixels = npixels;

        }

        public void Unlock()
        {
            if (!Locked) return;
            Locked = false;

            SDL.SDL_UnlockTexture(TextureHandle);
            Pixels = IntPtr.Zero; // now invalid
            Pitch = 0;

        }

        /// <summary>
        /// Draws this texture instance.
        /// </summary>
        /// <param name="Win">The window to draw this texture to.</param>
        /// <exception cref="NCException">An error occurred rendering the texture. Extended information is available in <see cref="NCException.Description"/></exception>
        public void Draw(Window Win)
        {
            if (Position == null) throw new NCException("Invalid texture draw pos!", 27, $"Position null for texture {Path}!", NCExceptionSeverity.Error);
            
            SDL.SDL_Rect src_rect = new SDL.SDL_Rect();
            SDL.SDL_Rect dst_rect = new SDL.SDL_Rect();

            // Draw to the viewpoint
            if (ViewportStart == null
                || ViewportEnd == null)
            {
                src_rect.x = 0;
                src_rect.y = 0;
                src_rect.w = Size.X;
                src_rect.h = Size.Y;

                dst_rect.x = Position.X;
                dst_rect.y = Position.Y;
                dst_rect.w = Position.X + Size.X;
                dst_rect.h = Position.Y + Size.Y;
            }
            else
            {
                src_rect.x = ViewportStart.X;
                src_rect.y = ViewportStart.Y;
                src_rect.w = Size.X - ViewportEnd.X;
                src_rect.h = Size.Y - ViewportEnd.Y; 

                dst_rect.x = Position.X;
                dst_rect.y = Position.Y;
                dst_rect.w = Position.X + ViewportEnd.X;
                dst_rect.h = Position.Y + ViewportEnd.Y;
            }

            if (Repeat == null)
            {
                // call to SDL - we are simply drawing it once.
                SDL.SDL_RenderCopy(Win.Settings.RendererHandle, TextureHandle, ref src_rect, ref dst_rect);
            }
            else
            {
                SDL.SDL_Rect new_rect = new SDL.SDL_Rect(dst_rect.x, dst_rect.y, dst_rect.w, dst_rect.h);

                // Draws a tiled texture.
                for (int y = 0; y < Repeat.Y; y++)
                {

                    SDL.SDL_RenderCopy(Win.Settings.RendererHandle, TextureHandle, ref src_rect, ref new_rect);

                    for (int x = 0; x < Repeat.X; x++)
                    {
                        SDL.SDL_RenderCopy(Win.Settings.RendererHandle, TextureHandle, ref src_rect, ref new_rect);

                        new_rect.x += dst_rect.w;
                    }

                    new_rect.y += dst_rect.h; // we already set it up
                    new_rect.x = dst_rect.x;
                }
            }
            
        }
    }
}
