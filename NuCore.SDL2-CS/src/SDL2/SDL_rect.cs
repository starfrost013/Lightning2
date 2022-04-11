#region License
/* Lightning SDL2 Wrapper
 * 
 * Version 3.0 (NuRender/Lightning) + SDL2_gfx
 * Copyright © 2022 starfrost
 * February 13, 2022
 * 
 * This software is based on the open-source SDL2# - C# Wrapper for SDL2 library.
 *
 * Copyright (c) 2013-2021 Ethan Lee.
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
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
#endregion

namespace NuCore.SDL2
{
    public static partial class SDL
    {
		#region SDL_rect.h

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_Point
		{
			public int x;
			public int y;

			public SDL_Point(int nx, int ny)
			{
				x = nx;
				y = ny;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_Rect
		{
			public int x;
			public int y;
			public int w;
			public int h;

			public SDL_Rect(int nx, int ny, int nw, int nh)
			{
				x = nx;
				y = ny;
				w = nw;
				h = nh;
			}
		}

		/* Only available in 2.0.10 or higher. */
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_FPoint
		{
			public float x;
			public float y;

			public SDL_FPoint(float NX, float NY)
			{
				x = NX;
				y = NY;
			}
		}

		/* Only available in 2.0.10 or higher. */
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_FRect
		{
			public float x;
			public float y;
			public float w;
			public float h;

			public SDL_FRect(float NX, float NY, float NW, float NH)
			{
				x = NX;
				y = NY;
				w = NH;
				h = NH;
			}
		}

		/* Only available in 2.0.4 or higher. */
		public static SDL_bool SDL_PointInRect(ref SDL_Point p, ref SDL_Rect r)
		{
			return ((p.x >= r.x) &&
					(p.x < (r.x + r.w)) &&
					(p.y >= r.y) &&
					(p.y < (r.y + r.h))) ?
				SDL_bool.SDL_TRUE :
				SDL_bool.SDL_FALSE;
		}

		[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_bool SDL_EnclosePoints(
			[In] SDL_Point[] points,
			int count,
			ref SDL_Rect clip,
			out SDL_Rect result
		);

		[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_bool SDL_HasIntersection(
			ref SDL_Rect A,
			ref SDL_Rect B
		);

		[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_bool SDL_IntersectRect(
			ref SDL_Rect A,
			ref SDL_Rect B,
			out SDL_Rect result
		);

		[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_bool SDL_IntersectRectAndLine(
			ref SDL_Rect rect,
			ref int X1,
			ref int Y1,
			ref int X2,
			ref int Y2
		);

		public static SDL_bool SDL_RectEmpty(ref SDL_Rect r)
		{
			return ((r.w <= 0) || (r.h <= 0)) ?
				SDL_bool.SDL_TRUE :
				SDL_bool.SDL_FALSE;
		}

		public static SDL_bool SDL_RectEquals(
			ref SDL_Rect a,
			ref SDL_Rect b
		)
		{
			return ((a.x == b.x) &&
					(a.y == b.y) &&
					(a.w == b.w) &&
					(a.h == b.h)) ?
				SDL_bool.SDL_TRUE :
				SDL_bool.SDL_FALSE;
		}

		[DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_UnionRect(
			ref SDL_Rect A,
			ref SDL_Rect B,
			out SDL_Rect result
		);

		#endregion
	}
}
