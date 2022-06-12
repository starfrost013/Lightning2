using System;
using System.Runtime.InteropServices;

namespace NuCore.SDL2
{
    public static partial class SDL
    {
		public const int SDL_NONSHAPEABLE_WINDOW = -1;
		public const int SDL_INVALID_SHAPE_ARGUMENT = -2;
		public const int SDL_WINDOW_LACKS_SHAPE = -3;

		[DllImport(nativeLibName, EntryPoint = "SDL_CreateShapedWindow", CallingConvention = CallingConvention.Cdecl)]
		private static unsafe extern IntPtr INTERNAL_SDL_CreateShapedWindow(
			byte* title,
			uint x,
			uint y,
			uint w,
			uint h,
			SDL_WindowFlags flags
		);

		public static unsafe IntPtr SDL_CreateShapedWindow(string title, uint x, uint y, uint w, uint h, SDL_WindowFlags flags)
		{
			byte* utf8Title = Utf8EncodeHeap(title);
			IntPtr result = INTERNAL_SDL_CreateShapedWindow(utf8Title, x, y, w, h, flags);
			Marshal.FreeHGlobal((IntPtr)utf8Title);
			return result;
		}

		[DllImport(nativeLibName, EntryPoint = "SDL_IsShapedWindow", CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_bool SDL_IsShapedWindow(IntPtr window);

		public enum WindowShapeMode
		{
			ShapeModeDefault,
			ShapeModeBinarizeAlpha,
			ShapeModeReverseBinarizeAlpha,
			ShapeModeColorKey
		}

		public static bool SDL_SHAPEMODEALPHA(WindowShapeMode mode)
		{
			switch (mode)
			{
				case WindowShapeMode.ShapeModeDefault:
				case WindowShapeMode.ShapeModeBinarizeAlpha:
				case WindowShapeMode.ShapeModeReverseBinarizeAlpha:
					return true;
				default:
					return false;
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct SDL_WindowShapeParams
		{
			[FieldOffset(0)]
			public byte binarizationCutoff;
			[FieldOffset(0)]
			public SDL_Color colorKey;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_WindowShapeMode
		{
			public WindowShapeMode mode;
			public SDL_WindowShapeParams parameters;
		}

		// window refers to an SDL_Window*
		[DllImport(nativeLibName, EntryPoint = "SDL_SetWindowShape", CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowShape(
			IntPtr window,
			IntPtr shape,
			ref SDL_WindowShapeMode shapeMode
		);

		// window refers to an SDL_Window*
		[DllImport(nativeLibName, EntryPoint = "SDL_GetShapedWindowMode", CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetShapedWindowMode(
			IntPtr window,
			out SDL_WindowShapeMode shapeMode
		);

		// window refers to an SDL_Window*
		[DllImport(nativeLibName, EntryPoint = "SDL_GetShapedWindowMode", CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetShapedWindowMode(
			IntPtr window,
			IntPtr shape_mode
		);


	}
}
