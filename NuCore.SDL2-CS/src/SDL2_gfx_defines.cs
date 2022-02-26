using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

/* SDL.NET - NuCore SDL2 Bindings
 * February 15, 2022
 * 
 * Copyright © 2022 starfrost.
 * Copyright © 2013-2021 Ethan Lee.
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
 *
 */

namespace NuCore.SDL2
{
    /// <summary>
    /// P/Invoke definitions for SDL2_gfx 
    /// </summary>
    public static partial class SDL_gfx
    {
		public const double M_PI = 3.1415926535897932384626433832795;

		#region SDL2_gfxPrimitives.h

		public const uint SDL2_GFXPRIMITIVES_MAJOR = 1;
		public const uint SDL2_GFXPRIMITIVES_MINOR = 0;
		public const uint SDL2_GFXPRIMITIVES_MICRO = 5;

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int pixelRGBA(IntPtr renderer, int x, int y, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int hlineRGBA(IntPtr renderer, int x1, int x2, int y, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int vlineRGBA(IntPtr renderer, int x, int y1, int y2, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int rectangleRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int roundedRectangleRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, int rad, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int boxRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int roundedBoxRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, int rad, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int lineRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int aalineRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, byte r, byte g, byte b, byte a);

		// 2022-2-26: Changed from uint8 to uint16 for really thick lines in C++
		// So changing this here. Don't fix this for mainline gfx
		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int thickLineRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, short width, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int circleRGBA(IntPtr renderer, int x, int y, int rad, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int arcRGBA(IntPtr renderer, int x, int y, int rad, int start, int end, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int aacircleRGBA(IntPtr renderer, int x, int y, int rad, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int filledCircleRGBA(IntPtr renderer, int x, int y, int rad, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int ellipseRGBA(IntPtr renderer, int x, int y, int rx, int ry, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int aaellipseRGBA(IntPtr renderer, int x, int y, int rx, int ry, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int filledEllipseRGBA(IntPtr renderer, int x, int y, int rx, int ry, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int pieRGBA(IntPtr renderer, int x, int y, int rad, int start, int end, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int filledPieRGBA(IntPtr renderer, int x, int y, int rad, int start, int end, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int trigonRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, int x3, int y3, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int aatrigonRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, int x3, int y3, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int filledTrigonRGBA(IntPtr renderer, int x1, int y1, int x2, int y2, int x3, int y3, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int polygonRGBA(IntPtr renderer, [In] short[] vx, [In] short[] vy, int n, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int aapolygonRGBA(IntPtr renderer, [In] short[] vx, [In] short[] vy, int n, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int filledPolygonRGBA(IntPtr renderer, [In] short[] vx, [In] short[] vy, int n, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int texturedPolygon(IntPtr renderer, [In] short[] vx, [In] short[] vy, int n, IntPtr texture, int texture_dx, int texture_dy);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int bezierRGBA(IntPtr renderer, [In] short[] vx, [In] short[] vy, int n, int s, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gfxPrimitivesSetFont([In] byte[] fontdata, uint cw, uint ch);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gfxPrimitivesSetFontRotation(uint rotation);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int characterColor(IntPtr renderer, short x, short y, char c, uint color);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int characterRGBA(IntPtr renderer, short x, short y, char c, byte r, byte g, byte b, byte a);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int stringRGBA(IntPtr renderer, short x, short y, string s, byte r, byte g, byte b, byte a);

		#endregion

		#region SDL2_rotozoom.h

		public const int SMOOTHING_OFF = 0;
		public const int SMOOTHING_ON = 1;

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr rotozoomSurface(IntPtr src, double angle, double zoom, int smooth);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr rotozoomSurfaceXY(IntPtr src, double angle, double zoomx, double zoomy, int smooth);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void rotozoomSurfaceSize(int width, int height, double angle, double zoom, out int dstwidth, out int dstheight);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void rotozoomSurfaceSizeXY(int width, int height, double angle, double zoomx, double zoomy, out int dstwidth, out int dstheight);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr zoomSurface(IntPtr src, double zoomx, double zoomy, int smooth);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void zoomSurfaceSize(int width, int height, double zoomx, double zoomy, out int dstwidth, out int dstheight);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr shrinkSurface(IntPtr src, int factorx, int factory);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr rotateSurface90Degrees(IntPtr src, int numClockwiseTurns);

		#endregion

		#region SDL2_framerate.h

		public const int FPS_UPPER_LIMIT = 200;
		public const int FPS_LOWER_LIMIT = 1;
		public const int FPS_DEFAULT = 30;

		[StructLayout(LayoutKind.Sequential)]
		public struct FPSmanager
		{
			public uint framecount;
			public float rateticks;
			public uint baseticks;
			public uint lastticks;
			public uint rate;
		}

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_initFramerate(ref FPSmanager manager);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_setFramerate(ref FPSmanager manager, uint rate);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_getFramerate(ref FPSmanager manager);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_getFramecount(ref FPSmanager manager);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint SDL_framerateDelay(ref FPSmanager manager);

		#endregion

#region SDL2_imageFilter.h

#if X86 // MMX supported for X64 only with GCC. As this is C# code...
		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterMMXdetect();

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_imageFilterMMXoff();

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_imageFilterMMXon();
#endif

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterAdd([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterMean([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterSub([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterAbsDiff([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterMult([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterMultNor([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterMultDivby2([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterMultDivby4([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterBitAnd([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterBitOr([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterDiv([In] byte[] src1, [In] byte[] src2, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterBitNegation([In] byte[] src1, [Out] byte[] dest, uint length);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterAddByte([In] byte[] src1, [Out] byte[] dest, uint length, byte c);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterAddUint([In] byte[] src1, [Out] byte[] dest, uint length, uint c);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterAddByteToHalf([In] byte[] src1, [Out] byte[] dest, uint length, byte c);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterSubByte([In] byte[] src1, [Out] byte[] dest, uint length, byte c);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterSubUint([In] byte[] src1, [Out] byte[] dest, uint length, uint c);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterShiftRight([In] byte[] src1, [Out] byte[] dest, uint length, byte n);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterShiftRightUint([In] byte[] src1, [Out] byte[] dest, uint length, byte n);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterMultByByte([In] byte[] src1, [Out] byte[] dest, uint length, byte c);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterShiftRightAndMultByByte([In] byte[] src1, [Out] byte[] dest, uint length, byte n, byte c);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterShiftLeftByte([In] byte[] src1, [Out] byte[] dest, uint length, byte n);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterShiftLeftUint([In] byte[] src1, [Out] byte[] dest, uint length, byte n);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterShiftLeft([In] byte[] src1, [Out] byte[] dest, uint length, byte n);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterBinarizeUsingThreshold([In] byte[] src1, [Out] byte[] dest, uint length, byte t);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterClipToRange([In] byte[] src1, [Out] byte[] dest, uint length, byte tmin, byte tmax);

		[DllImport(NativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_imageFilterNormalizeLinear([In] byte[] src1, [Out] byte[] dest, uint length, int cmin, int cmax, int nmin, int nmax);

#endregion
	}
}
