using LightningUtil;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LightningPackager
{
    /// <summary>
    /// PackageFile
    /// 
    /// basic (.wad) file format for packaging Lightning games. Uses LZMA + a basic file catalog. Supports folders.  
    /// </summary>
    public class PackageFile
    {
        /// <summary>
        /// The header of this package file.
        /// </summary>
        internal PackageFileHeader Header { get; set; }

        /// <summary>
        /// The catalog of this file
        /// </summary>
        internal PackageFileCatalog Catalog { get; set; }

        /// <summary>
        /// Rotating xor cipher used for obfuscation.
        /// </summary>
        private byte[] Key = { 0x8D, 0x8C, 0x9E, 0xC0, 0xDF, 0x91, 0x90, 0xDF, 0x88, 0x9E, 0x86,
            0xDE, 0x29, 0x83, 0x7D, 0xAA };

        /// <summary>
        /// Stores the input folder of the package. Used to create relative paths.
        /// </summary>
        public static string InFolder { get; set; }

        /// <summary>
        /// Modifier for obfuscated data.
        /// 
        /// Decrement when obfuscating,
        /// increment while deobfuscating
        /// </summary>
        private byte MODIFIER_AMOUNT = 7;

        public PackageFile(PackageFileMetadata metadata)
        {
            Header = new PackageFileHeader(metadata);
            Catalog = new PackageFileCatalog();
        }

        public void AddEntry(PackageFileCatalogEntry entry) => Catalog.AddEntry(entry);

        internal bool Extract(string path, string outDir)
        {
            Logger.Log($"Loading WAD from {path}, extracting to {outDir}...");

            if (!File.Exists(path)) Logger.LogError($"The file at {path} does not exist!", 98, LoggerSeverity.FatalError, null, true);

            byte[] fileBytes = File.ReadAllBytes(path);

            byte[] magic = fileBytes[0..PackageFileHeader.ObfuscatedMagic.Length];
            BinaryReader reader = new(new MemoryStream(fileBytes));

            if (magic.FastEqual(PackageFileHeader.ObfuscatedMagic))
            {
                Logger.Log($"File is obfuscated, deobfuscating...");

                // deobfuscate
                Deobfuscate(fileBytes);
            }
            else if (magic.FastEqual(PackageFileHeader.ObfuscatedMagicOld))
            {
                Logger.LogError($"Tried to load an obfuscated Lightning 1.x (WADv2.2) WAD file - this is not supported!", 300, LoggerSeverity.Error, null, false);
                return false;
            }

            // seek to zero as we've deobfuscated
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            PackageFileHeader header = PackageFileHeader.Read(reader);

            if (header == null) Logger.LogError($"{path} is invalid: Package header is invalid", 105, LoggerSeverity.FatalError, null, true);

            PackageFile file = new(header.Metadata);

            PackageFileCatalog catalog = PackageFileCatalog.Read(reader);

            // extract files
            catalog.Extract(reader, outDir);

            // close the header
            reader.Close();

            file.Header = header;
            file.Catalog = catalog;

            return true;
        }

        internal void Write(string path)
        {
            Logger.Log($"Generating WAD file and writing it to {path}...");

            using (BinaryWriter stream = new (new FileStream(path, FileMode.Create)))
            {
                Logger.Log("Writing header...");
                Header.Write(stream);

                Logger.Log("Writing file catalog...");
                Catalog.Write(stream, Header.HeaderSize);
            }

            // obfuscate if the header specifies so
            if (Header.Metadata.CompressionMode.HasFlag(PackageFileCompressionMode.XOR)) Obfuscate(path);
        }

        private void Obfuscate(string path)
        {
            Logger.Log("Obfuscating...");

            byte[] allBytes = File.ReadAllBytes(path);

            List<byte> xorBytes = new();

            byte[] key = DeobfuscateKey(Key);

            for (int curByteNumber = 0; curByteNumber < allBytes.Length; curByteNumber++)
            {
                byte curByte = allBytes[curByteNumber]; 

                byte xorByte = Convert.ToByte(curByte ^ key[curByteNumber % key.Length]);

                // decrement by 7 and enforce wraparound
                xorByte -= MODIFIER_AMOUNT;

                xorBytes.Add(xorByte);
            }

            File.WriteAllBytes(path, xorBytes.ToArray());
        }

        private BinaryReader Deobfuscate(byte[] byteArray)
        {
            // Deobfuscate the file.
            // Seek to 0 then read until the end of the file.

            byte[] key = DeobfuscateKey(Key);

            for (int curByteNumber = 0; curByteNumber < byteArray.Length; curByteNumber++)
            {
                byte curByte = byteArray[curByteNumber];

                curByte += MODIFIER_AMOUNT; // we have to increment first because we did it after xoring in the obfuscation step

                byte deobfuscatedByte = (byte)(curByte ^ key[curByteNumber % key.Length]);

                byteArray[curByteNumber] = deobfuscatedByte;

            }

            return new BinaryReader(new MemoryStream(byteArray));
        }

        /// <summary>
        /// Deobfuscates the wad xor key.
        /// 
        /// Aggressively inlined and obfuscated.
        /// </summary>
        /// <param name="key">The key to deobfuscate.</param>
        /// <returns>A byte array containing the deobfuscated key</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        private byte[] DeobfuscateKey(byte[] key)
        {
            for (int keyByte = 0; keyByte < key.Length; keyByte++)
            {
                // we do a little obfuscation
                key[keyByte] = (byte)(key[keyByte] ^ Convert.ToByte(0xFF)); // 255
            }

            return key; 
        }
    }
}