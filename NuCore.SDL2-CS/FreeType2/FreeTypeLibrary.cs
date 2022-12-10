using System;
using LightningBase;

namespace LightningBase
{
    /// <summary>
    /// Encapsulates the native FreeType2 library object.
    /// </summary>
    public sealed unsafe class FreeTypeLibrary : IDisposable
    {
        private Boolean disposed;

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public Boolean Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeLibrary"/> class.
        /// </summary>
        public FreeTypeLibrary()
        {
            nint lib;
            var err = FreeTypeApi.FT_Init_FreeType(out lib);
            if (err != FT_Error.FT_Err_Ok)
                throw new FreeTypeException(err);

            Native = lib;
        }

        /// <summary>
        /// Gets the native pointer to the FreeType2 library object.
        /// </summary>
        public nint Native { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        void Dispose(bool disposing)
        {
            if (Native != nint.Zero)
            {
                var err = FreeTypeApi.FT_Done_FreeType(Native);
                if (err != FT_Error.FT_Err_Ok)
                    throw new FreeTypeException(err);

                Native = nint.Zero;
            }

            disposed = true;
        }
    }
}
