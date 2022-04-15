﻿using static NuCore.SDL2.SDL;
using static NuCore.SDL2.SDL_image;
using NuCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
        /// Private: Texture format allocated for internal use
        /// </summary>
        private IntPtr CFormat { get; set; }

        /// <summary>
        /// Private: Pointer to unmanaged texture pixels (when the texture is locked)
        /// </summary>
        private uint* PixelPtr { get; set; }

        /// <summary>
        /// Initialises a new texture with a size.
        /// </summary>
        /// <param name="X">The width of the texture in pixels.</param>
        /// <param name="Y">The height of the texture in pixels.</param>
        public Texture(Window Win, float X, float Y)
        {
            Size = new Vector2(X, Y);

            if (Size == default(Vector2)) throw new NCException($"Error creating texture: Must have a size!", 20, "Texture.Create", NCExceptionSeverity.FatalError);

            TextureHandle = SDL_CreateTexture(Win.Settings.RendererHandle, SDL_PIXELFORMAT_RGBA8888, SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, (int)Size.X, (int)Size.Y);

            // check if texture failed to load
            if (TextureHandle == IntPtr.Zero) throw new NCException($"Error creating texture: {SDL_GetError()}", 11, "Texture.Create", NCExceptionSeverity.FatalError);

            Init_AllocFormat(Win);
        }

        public void Load(Window CWindow)
        {
            if (!File.Exists(Path)) throw new NCException($"{Path} does not exist!", 9, "!File.Exists(Texture.Path)!", NCExceptionSeverity.FatalError);

            TextureHandle = IMG_LoadTexture(CWindow.Settings.RendererHandle, Path);

            if (TextureHandle == IntPtr.Zero) throw new NCException($"Failed to load texture at {Path} - {SDL_GetError()}", 10, "Error in SDL_image.IMG_LoadTexture", NCExceptionSeverity.Error);
        }

        private void Init_AllocFormat(Window CWindow)
        {
            uint current_format = SDL_GetWindowPixelFormat(CWindow.Settings.WindowHandle);

            CFormat = SDL_AllocFormat(current_format);

            // probably not the best to actually like, allocate formats like this
            if (CFormat == IntPtr.Zero) throw new NCException($"Error allocating texture format for texture at {Path}: {SDL_GetError()}", 13, "Texture.Init_AllocFormat", NCExceptionSeverity.FatalError);
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
            if (Size == default(Vector2)) throw new NCException($"Invalid size - cannot get pixel!", 16, "Texture.GetPixel", NCExceptionSeverity.FatalError);

            if (!Locked) Lock();

            if (X < 0 || Y < 0
                || X > Size.X || Y > Size.Y) throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y})!", 12, "Texture.GetPixel", NCExceptionSeverity.FatalError);

            int PixelToGet = (Y * (int)Size.X) + X;
            int MaxPixelID = (Pitch / 4) * Pitch;

            if (PixelToGet > MaxPixelID) throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {PixelToGet} > {MaxPixelID}!", 14, "Texture.GetPixel", NCExceptionSeverity.FatalError);

            uint NP = PixelPtr[PixelToGet];

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
                || X > Size.X || Y > Size.Y) throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) ", 15, "Texture.SetPixel", NCExceptionSeverity.FatalError);
            int PixelToGet = (Y * (int)Size.X) + X;
            int MaxPixelID = (Pitch / 4) * Pitch;

            if (PixelToGet > MaxPixelID) throw new NCException($"Attempted to acquire invalid pixel coordinate for texture with path {Path} @ ({X},{Y}), min (0,0). max ({Size.X},{Size.Y}) (Pixel ID {PixelToGet} > {MaxPixelID}!", 16, "Texture.SetPixel", NCExceptionSeverity.FatalError);

            // use pixeltoget to twiddle the pixel that we need using the number we calculated before
            PixelPtr[PixelToGet] = (uint)Colour;

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
            SDL_Rect rect = new SDL_Rect(0, 0, (int)Size.X, (int)Size.Y);
            int npitch = Pitch;

            if (SDL_LockTexture(TextureHandle, ref rect, out npixels, out npitch) < 0) throw new NCException($"Error locking pixels for texture with path {Path}, error {SDL_GetError()}.", 11, "Texture.Lock", NCExceptionSeverity.FatalError);

            Pitch = npitch;
            Pixels = npixels;

            // convert to C pointer
            PixelPtr = (uint*)Pixels.ToPointer();
        }

        public void Unlock()
        {
            if (!Locked) return;
            Locked = false;

            SDL_UnlockTexture(TextureHandle);

            // these values are now invalid 
            Pixels = IntPtr.Zero;
            PixelPtr = null;
            Pitch = 0;
        }

        /// <summary>
        /// Draws this texture instance.
        /// </summary>
        /// <param name="Win">The window to draw this texture to.</param>
        /// <exception cref="NCException">An error occurred rendering the texture. Extended information is available in <see cref="NCException.Description"/></exception>
        public void Draw(Window Win, bool SnapToScreen = false)
        {
            SDL_Rect src_rect = new SDL_Rect();
            SDL_FRect dst_rect = new SDL_FRect();

            Camera cur_camera = Win.Settings.Camera;

            // Draw to the viewpoint
            if (ViewportStart == default(Vector2)
                || ViewportEnd == default(Vector2))
            {
                src_rect.x = 0;
                src_rect.y = 0;
                src_rect.w = (int)Size.X;
                src_rect.h = (int)Size.Y;

                dst_rect.x = Position.X;
                dst_rect.y = Position.Y;
                dst_rect.w = Position.X + Size.X;
                dst_rect.h = Position.Y + Size.Y;
            }
            else
            {
                src_rect.x = (int)ViewportStart.X;
                src_rect.y = (int)ViewportStart.Y;
                src_rect.w = (int)ViewportEnd.X - (int)ViewportStart.X;
                src_rect.h = (int)ViewportEnd.Y - (int)ViewportStart.Y;

                dst_rect.x = (int)Position.X;
                dst_rect.y = (int)Position.Y;
                dst_rect.w = (int)ViewportEnd.X - (int)ViewportStart.X;
                dst_rect.h = (int)ViewportEnd.Y - (int)ViewportStart.Y;
            }

            if (!SnapToScreen)
            {
                dst_rect.x -= cur_camera.Position.X;
                dst_rect.y -= cur_camera.Position.Y;
            }

            if (Repeat == default(Vector2))
            {
                // call to SDL - we are simply drawing it once.
                SDL_RenderCopyF(Win.Settings.RendererHandle, TextureHandle, ref src_rect, ref dst_rect);
            }
            else
            {
                SDL_FRect new_rect = new SDL_FRect(dst_rect.x, dst_rect.y, dst_rect.w, dst_rect.h);

                // Draws a tiled texture.
                for (int y = 0; y < Repeat.Y; y++)
                {
                    SDL_RenderCopyF(Win.Settings.RendererHandle, TextureHandle, ref src_rect, ref new_rect);

                    for (int x = 0; x < Repeat.X; x++)
                    {
                        SDL_RenderCopyF(Win.Settings.RendererHandle, TextureHandle, ref src_rect, ref new_rect);

                        new_rect.x += dst_rect.w;
                    }

                    new_rect.y += dst_rect.h; // we already set it up
                    new_rect.x = dst_rect.x;
                }
            }
        }

        /// <summary>
        /// Clears the texture with the colour specified in the <paramref name="Colour"/> parameter. If this is not set, it will default to ARGB <c>0,0,0,0</c>.
        /// </summary>
        /// <param name="Colour">The optional colour to clear the texture with.</param>
        public void Clear(Color4 Colour = null)
        {
            Color4 clear_colour = new Color4(0, 0, 0, 0);

            if (Colour != null) clear_colour = Colour;

            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    SetPixel(x, y, clear_colour);
                }
            }
        }

        public void SetBlendMode(SDL_BlendMode BlendMode) => SDL_SetTextureBlendMode(TextureHandle, BlendMode);
    }
}