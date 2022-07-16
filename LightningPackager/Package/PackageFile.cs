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
        public PackageHeader Header { get; set; }

        public PackageFileCatalog Catalog { get; set; }

        private byte xorKey = 0xD1;

        public PackageFile(PackageFileMetadata metadata)
        {
            Header = new PackageHeader(metadata);
            Catalog = new PackageFileCatalog(); 
        }

        public void AddEntry(PackageFileCatalogEntry entry) => Catalog.AddEntry(entry);

        public void Read(string path, string outDir)
        {
            NCLogging.Log($"Loading WAD from {path}, extracting to {outDir}..."); 
            
            using (BinaryReader reader = new BinaryReader(new FileStream(path)))
            {

            }
        }

        private void Deobfuscate(byte[] obfuscatedBytes)
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
                byte xorByte = Convert.ToByte(curByte ^ xorKey);

                // decrement by 1 and enforce wraparound
                xorByte -= 3;

                xorBytes.Add(xorByte);
            }

            File.WriteAllBytes(path, xorBytes.ToArray());
        }

    }
}
