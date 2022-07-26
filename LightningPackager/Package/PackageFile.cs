using ManagedLzma.LZMA;
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
        public PackageFileHeader Header { get; set; }

        public PackageFileCatalog Catalog { get; set; }

        private byte Key = 0xD1;

        /// <summary>
        /// Chunk length used for deobfuscation.
        /// 1MB chunks used to reduce memory usage.
        /// </summary>
        private int DeobfuscateChunkLength = 1048576;

        public PackageFile(PackageFileMetadata metadata)
        {
            Header = new PackageFileHeader(metadata);
            Catalog = new PackageFileCatalog(); 
        }

        public void AddEntry(PackageFileCatalogEntry entry) => Catalog.AddEntry(entry);

        public bool Extract(string path, string outDir)
        {
            NCLogging.Log($"Loading WAD from {path}, extracting to {outDir}...");

            if (!File.Exists(path)) _ = new NCException($"The file at {path} does not exist!", 98, "PackageFile::Read path parameter is a non-existent file!", NCExceptionSeverity.FatalError, null, true);

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
            string magic = reader.ReadString();

            if (magic != PackageFileHeader.Magic) _ = new NCException($"{path} is not a WAD file or magic is corrupt (expected {PackageFileHeader.Magic}, got {magic}!", 99, $"PackageFile::Read - could not identify obfuscated or non-obfuscated header!", NCExceptionSeverity.Error, null, true);

            return true;
        }

        private BinaryReader Deobfuscate(string path, BinaryReader reader)
        {
            // Deobfuscate the file.
            // Seek to 0 then read until the end of the file.

            // use the already open reader to reduce reopening/closing of the file 
            // only read 32kbytes at a time to reduce memory usage

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

            File.WriteAllBytes(path, deobfuscatedBytes.ToArray());

            reader = new BinaryReader(new FileStream(path, FileMode.Open));
            return reader; 
        }

        public void Read(StreamReader reader)
        {
            
        }

        public void Write(string path)
        {
            NCLogging.Log($"Generating WAD file and writing it to {path}...");

            using (BinaryWriter stream = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                NCLogging.Log("Writing header...");
                Header.Write(stream);

                NCLogging.Log("Writing file catalog...");
                Catalog.Write(stream, Header.HeaderSize);
            }

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

                // decrement by 1 and enforce wraparound
                xorByte -= 3;

                xorBytes.Add(xorByte);
            }

            File.WriteAllBytes(path, xorBytes.ToArray());
        }

    }
}
