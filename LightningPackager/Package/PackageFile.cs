using NuCore.Utilities;

namespace LightningPackager
{
    /// <summary>
    /// PackageFile
    /// 
    /// basic (.wad) file format for packaging Lightning games. Uses LZMA + a basic file catalog. Supports folders.  
    /// </summary>
    public class PackageFile
    {
        internal PackageFileHeader Header { get; set; }

        internal PackageFileCatalog Catalog { get; set; }

        private byte Key = 0xD1;

        /// <summary>
        /// Chunk length used for deobfuscation.
        /// 1MB chunks used to reduce memory usage.
        /// </summary>
        private int DeobfuscateChunkLength = 1048576;

        /// <summary>
        /// Private: Path used for the deobfuscated file.
        /// </summary>
        private string DeobfuscatedPath { get; set; }

        /// <summary>
        /// Stores the input folder of the package. Used to create relative paths.
        /// </summary>
        public static string InFolder { get; set; }

        public PackageFile(PackageFileMetadata metadata)
        {
            Header = new PackageFileHeader(metadata);
            Catalog = new PackageFileCatalog();
        }

        public void AddEntry(PackageFileCatalogEntry entry) => Catalog.AddEntry(entry);

        internal bool Extract(string path, string outDir)
        {
            NCLogging.Log($"Loading WAD from {path}, extracting to {outDir}...");

            if (!File.Exists(path)) NCError.Throw($"The file at {path} does not exist!", 98, "PackageFile::Read path parameter is a non-existent file!", NCErrorSeverity.FatalError, null, true);

            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));

            // read series of bytes
            byte[] bytes = reader.ReadBytes(PackageFileHeader.ObfuscatedMagic.Length);

            // if it's equal to the obfuscated magic...
            if (bytes.FastEqual(PackageFileHeader.ObfuscatedMagic))
            {
                NCLogging.Log($"File is obfuscated, deobfuscating...");

                // deobfuscate
                reader = Deobfuscate(path, reader);
            }

            // seek to zero as we've deobfuscated
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            PackageFileHeader header = PackageFileHeader.Read(reader);

            if (header == null) NCError.Throw($"{path} is invalid: Package header is invalid", 105, "PackageFileHeader::Read returned null", NCErrorSeverity.FatalError, null, true);

            PackageFile file = new PackageFile(header.Metadata);

            PackageFileCatalog catalog = PackageFileCatalog.Read(reader);

            // extract files
            catalog.Extract(reader, outDir);

            // close the header
            reader.Close();

            file.Header = header;
            file.Catalog = catalog;

            // delete the deobfuscated file
            File.Delete(DeobfuscatedPath);

            return true;
        }

        private BinaryReader Deobfuscate(string path, BinaryReader reader)
        {
            // Deobfuscate the file.
            // Seek to 0 then read until the end of the file.

            // use the already open reader to reduce reopening/closing of the file 
            // only read 1 megabyte at a time to reduce memory usage

            // seek to zero so we don't skip the first few bytes
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            long numOfChunks = reader.BaseStream.Length / DeobfuscateChunkLength;

            List<byte> deobfuscatedBytes = new List<byte>();

            for (int curChunk = 0; curChunk <= numOfChunks; curChunk++)
            {
                byte[] fileBytes = reader.ReadBytes(DeobfuscateChunkLength);

                // read that chunk
                foreach (byte b in fileBytes)
                {
                    byte deobfuscatedByte = b;
                    deobfuscatedByte += 3; // increment by 3
                    deobfuscatedBytes.Add(Convert.ToByte(deobfuscatedByte ^ Key)); // use key 
                }
            }

            reader.Close();

            DeobfuscatedPath = $"d_{path}"; // prepend d_
            File.WriteAllBytes(DeobfuscatedPath, deobfuscatedBytes.ToArray());

            reader = new BinaryReader(new FileStream(DeobfuscatedPath, FileMode.Open));
            return reader;
        }

        internal void Write(string path)
        {
            NCLogging.Log($"Generating WAD file and writing it to {path}...");

            using (BinaryWriter stream = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                NCLogging.Log("Writing header...");
                Header.Write(stream);

                NCLogging.Log("Writing file catalog...");
                Catalog.Write(stream, Header.HeaderSize);
            }

            // obfuscate if the header specifies so
            if (Header.Metadata.CompressionMode.HasFlag(PackageFileCompressionMode.XOR)) Obfuscate(path);
        }

        private void Obfuscate(string path)
        {
            NCLogging.Log("Obfuscating (XOR key=0xD1)...");

            byte[] allBytes = File.ReadAllBytes(path);

            List<byte> xorBytes = new List<byte>();

            foreach (byte curByte in allBytes)
            {
                byte xorByte = Convert.ToByte(curByte ^ Key);

                // decrement by 3 and enforce wraparound
                xorByte -= 3;

                xorBytes.Add(xorByte);
            }

            File.WriteAllBytes(path, xorBytes.ToArray());
        }
    }
}