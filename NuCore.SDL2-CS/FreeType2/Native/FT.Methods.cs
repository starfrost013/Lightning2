using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LightningBase
{
#if __IOS__
    [Foundation.Preserve(AllMembers=true)]
#endif
    public static unsafe partial class FreeTypeApi
    {

#if __IOS__
        public const string FreeTypeLibaryName = "__Internal";
#else
        public const string FreeTypeLibraryName = "freetype";
#endif

#if NETCOREAPP3_1_OR_GREATER && !__IOS__
        static FreeTypeApi()
        {
            NativeLibrary.SetDllImportResolver(typeof(FreeTypeApi).Assembly, ImportResolver);
        }

        private static nint ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != FreeTypeLibraryName) return default;

            nint handle = default;
            bool success = false;

            bool isWindows = false, isMacOS = false, isLinux = false, isAndroid = false;

            if (OperatingSystem.IsWindows())
                isWindows = true;
            else if (OperatingSystem.IsMacOS())
                isMacOS = true;
            else if (OperatingSystem.IsLinux())
                isLinux = true;
            else if (OperatingSystem.IsAndroid())
                isAndroid = true;
            string ActualLibraryName;
            if (isWindows)
                ActualLibraryName = "freetype.dll";
            else if (isMacOS)
                ActualLibraryName = "libfreetype.dylib";
            else if (isLinux)
                ActualLibraryName = "libfreetype.so";
            else if (isAndroid)
                ActualLibraryName = "libfreetype.so";
            else
                throw new PlatformNotSupportedException();

            string rootDirectory = AppContext.BaseDirectory;

            if (isWindows)
            {
                string arch = Environment.Is64BitProcess ? "win-x64" : "win-x86";
                var searchPaths = new[]
                {
                    // This is where native libraries in our nupkg should end up
                    Path.Combine(rootDirectory, "runtimes", arch, "native", ActualLibraryName),
                    Path.Combine(rootDirectory, Environment.Is64BitProcess ? "x64" : "x86", ActualLibraryName),
                    Path.Combine(rootDirectory, ActualLibraryName)
                };

                foreach (var path in searchPaths)
                {
                    success = NativeLibrary.TryLoad(path, out handle);

                    if (success)
                        return handle;
                }

                // Fallback to system installed freetype
                success = NativeLibrary.TryLoad(libraryName, typeof(FreeTypeApi).Assembly,
                    DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories | DllImportSearchPath.UseDllDirectoryForDependencies,
                    out handle);

                if (success)
                    return handle;

                throw new FileLoadException("Failed to load native freetype library!");
            }

            if (isLinux || isMacOS)
            {
                string arch = isMacOS ? "osx" : "linux-" + (Environment.Is64BitProcess ? "x64" : "x86");

                var searchPaths = new[]
                {
                    // This is where native libraries in our nupkg should end up
                    Path.Combine(rootDirectory, "runtimes", arch, "native", ActualLibraryName),
                    // The build output folder
                    Path.Combine(rootDirectory, ActualLibraryName),
                    Path.Combine("/usr/local/lib", ActualLibraryName),
                    Path.Combine("/usr/lib", ActualLibraryName)
                };

                foreach (var path in searchPaths)
                {
                    success = NativeLibrary.TryLoad(path, out handle);

                    if (success)
                        return handle;
                }

                // Fallback to system installed freetype
                success = NativeLibrary.TryLoad(libraryName, typeof(FreeTypeApi).Assembly,
                    DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories | DllImportSearchPath.UseDllDirectoryForDependencies,
                    out handle);

                if (success)
                    return handle;

                throw new FileLoadException("Failed to load native freetype library!");
            }

            if (isAndroid)
            {
                success = NativeLibrary.TryLoad(ActualLibraryName, typeof(FreeTypeApi).Assembly,
                    DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories | DllImportSearchPath.UseDllDirectoryForDependencies, 
                    out handle);

                if (!success)
                    success = NativeLibrary.TryLoad(ActualLibraryName, out handle);

                if (success)
                    return handle;

                // Fallback to system installed freetype
                success = NativeLibrary.TryLoad(libraryName, typeof(FreeTypeApi).Assembly,
                    DllImportSearchPath.ApplicationDirectory | DllImportSearchPath.UserDirectories | DllImportSearchPath.UseDllDirectoryForDependencies,
                    out handle);

                if (success)
                    return handle;

                throw new FileLoadException("Failed to load native freetype library!");
            }

            return handle;
        }
#endif

        #region Core API

        #region FreeType Version

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Library_Version(nint library, out int amajor, out int aminor, out int apatch);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FT_Face_CheckTrueTypePatents(nint face);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool FT_Face_SetUnpatentedHinting(nint face, [MarshalAs(UnmanagedType.U1)] bool value);
        

        #endregion

        #region Base Interface

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Init_FreeType(out nint alibrary);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Done_FreeType(nint library);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern FT_Error FT_New_Face(nint library, string filepathname, int face_index, out nint aface);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Memory_Face(nint library, nint file_base, int file_size, int face_index, out nint aface);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Open_Face(nint library, nint args, int face_index, out nint aface);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern FT_Error FT_Attach_File(nint face, string filepathname);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Attach_Stream(nint face, nint parameters);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Reference_Face(nint face);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Done_Face(nint face);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Select_Size(nint face, int strike_index);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Request_Size(nint face, nint req);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Set_Char_Size(nint face, nint char_width, nint char_height, uint horz_resolution, uint vert_resolution);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Set_Pixel_Sizes(nint face, uint pixel_width, uint pixel_height);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Load_Glyph(nint face, uint glyph_index, int load_flags);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Load_Char(nint face, uint char_code, int load_flags);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Set_Transform(nint face, nint matrix, nint delta);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Render_Glyph(nint slot, FT_Render_Mode render_mode);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_Kerning(nint face, uint left_glyph, uint right_glyph, uint kern_mode, out FT_Vector akerning);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_Track_Kerning(nint face, nint point_size, int degree, out nint akerning);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_Glyph_Name(nint face, uint glyph_index, nint buffer, uint buffer_max);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Get_Postscript_Name(nint face);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Select_Charmap(nint face, FT_Encoding encoding);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Set_Charmap(nint face, nint charmap);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int FT_Get_Charmap_Index(nint charmap);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FT_Get_Char_Index(nint face, uint charcode);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FT_Get_First_Char(nint face, out uint agindex);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FT_Get_Next_Char(nint face, uint char_code, out uint agindex);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FT_Get_Name_Index(nint face, nint glyph_name);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_SubGlyph_Info(nint glyph, uint sub_index, out int p_index, out uint p_flags, out int p_arg1, out int p_arg2, out FT_Matrix p_transform);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort FT_Get_FSType_Flags(nint face);
        

        #endregion

        #region Glyph Variants

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FT_Face_GetCharVariantIndex(nint face, uint charcode, uint variantSelector);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int FT_Face_GetCharVariantIsDefault(nint face, uint charcode, uint variantSelector);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Face_GetVariantSelectors(nint face);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Face_GetVariantsOfChar(nint face, uint charcode);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Face_GetCharsOfVariant(nint face, uint variantSelector);
        

        #endregion

        #region Glyph Management

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_Glyph(nint slot, out nint aglyph);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Glyph_Copy(nint source, out nint target);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Glyph_Transform(nint glyph, ref FT_Matrix matrix, ref FT_Vector delta);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Glyph_Get_CBox(nint glyph, FT_Glyph_BBox_Mode bbox_mode, out FT_BBox acbox);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Glyph_To_Bitmap(ref nint the_glyph, FT_Render_Mode render_mode, ref FT_Vector origin, [MarshalAs(UnmanagedType.U1)] bool destroy);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Done_Glyph(nint glyph);
        

        #endregion

        #region Mac Specific Interface - check for macOS before calling these methods.

        // No methods in iOS
        //[DllImport(FreeTypeLibaryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern FT_Error FT_New_Face_From_FOND(nint library, nint fond, int face_index, out nint aface);
        

        // No methods in iOS
        //[DllImport(FreeTypeLibaryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        //public static extern FT_Error FT_GetFile_From_Mac_Name(string fontName, out nint pathSpec, out int face_index);
        
        // No methods in iOS
        //[DllImport(FreeTypeLibaryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        //public static extern FT_Error FT_GetFile_From_Mac_ATS_Name(string fontName, out nint pathSpec, out int face_index);
        

        // No methods in iOS
        //[DllImport(FreeTypeLibaryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        //public static extern FT_Error FT_GetFilePath_From_Mac_ATS_Name(string fontName, nint path, int maxPathSize, out int face_index);
        

        // No methods in iOS
        //[DllImport(FreeTypeLibaryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern FT_Error FT_New_Face_From_FSSpec(nint library, nint spec, int face_index, out nint aface);
        

        // No methods in iOS
        //[DllImport(FreeTypeLibaryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern FT_Error FT_New_Face_From_FSRef(nint library, nint @ref, int face_index, out nint aface);
        

        #endregion

        #region Size Management

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Size(nint face, out nint size);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Done_Size(nint size);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Activate_Size(nint size);
        

        #endregion

        #endregion

        #region Support API

        #region Computations

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_MulDiv(nint a, nint b, nint c);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_MulFix(nint a, nint b);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_DivFix(nint a, nint b);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_RoundFix(nint a);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_CeilFix(nint a);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_FloorFix(nint a);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Vector_Transform(ref FT_Vector vec, ref FT_Matrix matrix);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Matrix_Multiply(ref FT_Matrix a, ref FT_Matrix b);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Matrix_Invert(ref FT_Matrix matrix);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Sin(nint angle);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Cos(nint angle);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Tan(nint angle);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Atan2(nint x, nint y);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Angle_Diff(nint angle1, nint angle2);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Vector_Unit(out FT_Vector vec, nint angle);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Vector_Rotate(ref FT_Vector vec, nint angle);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Vector_Length(ref FT_Vector vec);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Vector_Polarize(ref FT_Vector vec, out nint length, out nint angle);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Vector_From_Polar(out FT_Vector vec, nint length, nint angle);
        
        #endregion

        #region List Processing

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_List_Find(nint list, nint data);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_List_Add(nint list, nint node);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_List_Insert(nint list, nint node);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_List_Remove(nint list, nint node);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_List_Up(nint list, nint node);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_List_Iterate(nint list, FT_List_Iterator iterator, nint user);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_List_Finalize(nint list, FT_List_Destructor destroy, nint memory, nint user);
        

        #endregion

        #region Outline Processing

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_New(nint library, uint numPoints, int numContours, out nint anoutline);
        

        // No methods in iOS
        //[DllImport(FreeTypeLibaryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern FT_Error FT_Outline_New_Internal(nint memory, uint numPoints, int numContours, out nint anoutline);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_Done(nint library, nint outline);
        

        // No methods in iOS
        //[DllImport(FreeTypeLibaryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern FT_Error FT_Outline_Done_Internal(nint memory, nint outline);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_Copy(nint source, ref nint target);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Outline_Translate(nint outline, int xOffset, int yOffset);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Outline_Transform(nint outline, ref FT_Matrix matrix);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_Embolden(nint outline, nint strength);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_EmboldenXY(nint outline, int xstrength, int ystrength);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Outline_Reverse(nint outline);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_Check(nint outline);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_Get_BBox(nint outline, out FT_BBox abbox);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_Decompose(nint outline, ref FT_Outline_Funcs func_interface, nint user);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Outline_Get_CBox(nint outline, out FT_BBox acbox);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_Get_Bitmap(nint library, nint outline, nint abitmap);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Outline_Render(nint library, nint outline, nint @params);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Orientation FT_Outline_Get_Orientation(nint outline);
        

        #endregion

        #region Quick retrieval of advance values

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_Advance(nint face, uint gIndex, uint load_flags, out nint padvance);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_Advances(nint face, uint start, uint count, uint load_flags, out nint padvance);
        

        #endregion

        #region Bitmap Handling

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Bitmap_New(nint abitmap);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Bitmap_Copy(nint library, nint source, nint target);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Bitmap_Embolden(nint library, nint bitmap, nint xStrength, nint yStrength);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Bitmap_Convert(nint library, nint source, nint target, int alignment);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_GlyphSlot_Own_Bitmap(nint slot);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Bitmap_Done(nint library, nint bitmap);
        

        #endregion

        #region Glyph Stroker

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_StrokerBorder FT_Outline_GetInsideBorder(nint outline);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_StrokerBorder FT_Outline_GetOutsideBorder(nint outline);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_New(nint library, out nint astroker);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Stroker_Set(nint stroker, int radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, nint miter_limit);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Stroker_Rewind(nint stroker);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_ParseOutline(nint stroker, nint outline, [MarshalAs(UnmanagedType.U1)] bool opened);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_BeginSubPath(nint stroker, ref FT_Vector to, [MarshalAs(UnmanagedType.U1)] bool open);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_EndSubPath(nint stroker);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_LineTo(nint stroker, ref FT_Vector to);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_ConicTo(nint stroker, ref FT_Vector control, ref FT_Vector to);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_CubicTo(nint stroker, ref FT_Vector control1, ref FT_Vector control2, ref FT_Vector to);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_GetBorderCounts(nint stroker, FT_StrokerBorder border, out uint anum_points, out uint anum_contours);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Stroker_ExportBorder(nint stroker, FT_StrokerBorder border, nint outline);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_GetCounts(nint stroker, out uint anum_points, out uint anum_contours);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Stroker_Export(nint stroker, nint outline);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Stroker_Done(nint stroker);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Glyph_Stroke(ref nint pglyph, nint stoker, [MarshalAs(UnmanagedType.U1)] bool destroy);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Glyph_StrokeBorder(ref nint pglyph, nint stoker, [MarshalAs(UnmanagedType.U1)] bool inside, [MarshalAs(UnmanagedType.U1)] bool destroy);
        

        #endregion

        #region Module Management

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Add_Module(nint library, nint clazz);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern nint FT_Get_Module(nint library, string module_name);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Remove_Module(nint library, nint module);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern FT_Error FT_Property_Set(nint library, string module_name, string property_name, nint value);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern FT_Error FT_Property_Get(nint library, string module_name, string property_name, nint value);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Reference_Library(nint library);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Library(nint memory, out nint alibrary);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Done_Library(nint library);
        

        //TODO figure out the method signature for debug_hook. (FT_DebugHook_Func)
        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Set_Debug_Hook(nint library, uint hook_index, nint debug_hook);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Add_Default_Modules(nint library);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern nint FT_Get_Renderer(nint library, FT_Glyph_Format format);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Set_Renderer(nint library, nint renderer, uint num_params, nint parameters);
        

        #endregion

        #region GZIP Streams

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stream_OpenGzip(nint stream, nint source);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Gzip_Uncompress(nint memory, nint output, ref nint output_len, nint input, nint input_len);
        

        #endregion

        #region LZW Streams

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stream_OpenLZW(nint stream, nint source);
        

        #endregion

        #region BZIP2 Streams

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stream_OpenBzip2(nint stream, nint source);
        

        #endregion

        #region LCD Filtering

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Library_SetLcdFilter(nint library, FT_LcdFilter filter);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Library_SetLcdFilterWeights(nint library, byte[] weights);
        

        #endregion

        #endregion

        #region Caching Sub-system

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_Manager_New(nint library, uint max_faces, uint max_sizes, ulong maxBytes, FTC_Face_Requester requester, nint req_data, out nint amanager);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FTC_Manager_Reset(nint manager);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FTC_Manager_Done(nint manager);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_Manager_LookupFace(nint manager, nint face_id, out nint aface);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_Manager_LookupSize(nint manager, nint scaler, out nint asize);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FTC_Node_Unref(nint node, nint manager);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FTC_Manager_RemoveFaceID(nint manager, nint face_id);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_CMapCache_New(nint manager, out nint acache);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint FTC_CMapCache_Lookup(nint cache, nint face_id, int cmap_index, uint char_code);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_ImageCache_New(nint manager, out nint acache);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_ImageCache_Lookup(nint cache, nint type, uint gindex, out nint aglyph, out nint anode);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_ImageCache_LookupScaler(nint cache, nint scaler, uint load_flags, uint gindex, out nint aglyph, out nint anode);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_SBitCache_New(nint manager, out nint acache);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_SBitCache_Lookup(nint cache, nint type, uint gindex, out nint sbit, out nint anode);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FTC_SBitCache_LookupScaler(nint cache, nint scaler, uint load_flags, uint gindex, out nint sbit, out nint anode);
        

        #endregion

        #region Miscellaneous

        #region OpenType Validation

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_OpenType_Validate(nint face, uint validation_flags, out nint base_table, out nint gdef_table, out nint gpos_table, out nint gsub_table, out nint jsft_table);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_OpenType_Free(nint face, nint table);
        

        #endregion

        #region The TrueType Engine

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_TrueTypeEngineType FT_Get_TrueType_Engine_Type(nint library);
        

        #endregion

        #region TrueTypeGX/AAT Validation

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_TrueTypeGX_Validate(nint face, uint validation_flags, ref byte[] tables, uint tableLength);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_TrueTypeGX_Free(nint face, nint table);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_ClassicKern_Validate(nint face, uint validation_flags, out nint ckern_table);
        

        [DllImport(FreeTypeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_ClassicKern_Free(nint face, nint table);
        

        #endregion

        #endregion

    }
}
