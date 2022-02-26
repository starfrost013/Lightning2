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
        /// The size of this texture. 
        /// Does not have to be equal to image size.
        /// </summary>
        public Vector2 Size { get; set; }
        /// <summary>
        /// Updated when the texture is locked in C++. Used to determine if the pixels can be acquired.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Alias for <see cref="SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING"/>. Determines if the texture is modifiable after it loads.
        /// </summary>
        public bool Modifiable { get; set; }

        /// <summary>
        /// The pitch of this texture
        /// </summary>
        public int Pitch { get; set; }

        /// <summary>
        /// Determines if this object has been destroyed.
        /// </summary>
        private bool Destroyed { get; set; }

        private IntPtr CFormat { get; set; }

        public Texture()
        {

        }

        public Texture(int X, int Y)
        {
            Size = new Vector2(X, Y);
        }

        public void Create(Window CWindow)
        {
            TextureHandle = SDL.SDL_CreateTexture(CWindow.Settings.RendererHandle, SDL.SDL_PIXELFORMAT_RGBA8888, SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, Size.X, Size.Y);

            // check if texture failed to load
            if (TextureHandle == IntPtr.Zero)
            {
                throw new NCException($"Error creating texture {SDL.SDL_GetError()}: {SDL.SDL_GetError()}", 11, "Texture.Create", NCExceptionSeverity.FatalError);
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

        public Pixel GetPixel(int X, int Y, bool UnlockNow = false)
        {
            if (Size == null) throw new NCException($"Invalid size - cannot get pixel!", 16, "Texture.GetPixel", NCExceptionSeverity.FatalError);

            if (!Locked) Lock();

            if (X < 0 || Y < 0
                || X > Size.X || Y > Size.Y)
            {
                throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) ", 12, "Texture.GetPixel", NCExceptionSeverity.FatalError);
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

            return new Pixel((Color4)NP);
        }

        public void SetPixel(int X, int Y, Color4 Colour, bool UnlockNow = false)
        {
            if (!Locked) Lock();

            if (X < 0 || Y < 0
                || X > Size.X || Y > Size.Y)
            {
                throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) ", 15, "Texture.GetPixel", NCExceptionSeverity.FatalError);
            }

            int PixelToGet = (X / Pitch) * Y;
            int MaxPixelID = (Pitch / 4) * Y;

            if (PixelToGet > MaxPixelID)
            {
                throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {PixelToGet} > {MaxPixelID}!", 16, "Texture.GetPixel", NCExceptionSeverity.FatalError);
            }

            // CS0211 
            // does not matter as still points to same place
            IntPtr npixels = Pixels;
            IntPtr* PixelPtr = &npixels;

            NCLogging.Log($"DEBUG: Setting pixel {X},{Y} for tex {Path}, size {Size.X},{Size.Y} to colour {Colour.R},{Colour.G},{Colour.B},{Colour.A} (RGBA)...");
            // oh god
            PixelPtr[PixelToGet] = (IntPtr)(uint)Colour;

            if (UnlockNow) Unlock(); // unlock the texture

            
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

            IntPtr npixels = Pixels;

            SDL.SDL_UnlockTexture(npixels);

        }

    }
}
