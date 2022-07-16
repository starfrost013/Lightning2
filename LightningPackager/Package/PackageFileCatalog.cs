using NuCore.Utilities;

namespace LightningPackager
{
    public class PackageFileCatalog
    {
        public const string Magic = "here!";
        public int NumberOfEntries => Entries.Count;
        public List<PackageFileCatalogEntry> Entries { get; private set; }

        public PackageFileCatalog()
        {
            Entries = new List<PackageFileCatalogEntry>();
        }

        internal void AddEntry(PackageFileCatalogEntry entry)
        {
            NCLogging.Log($"Adding WAD catalog entry for filename {entry.Path}...");
            Entries.Add(entry);
        }

        /// <summary>
        /// Writes the file catalog and data.
        /// </summary>
        /// <param name="writer">The stream, in this case a <see cref="BinaryWriter"/>, to write the file cataloog and data to.</param>
        /// <param name="headerSize">The size of the header to write the file to.</param>
        internal void Write(BinaryWriter writer, int headerSize)
        {
            // May need to do a bit of refactoring to make this a bit easier to read
            writer.Write(Magic);
            writer.Write(NumberOfEntries);
            
            // write each entry in the catalog 
            foreach (PackageFileCatalogEntry entry in Entries) entry.Write(writer);

            NCLogging.Log("Written catalog entries, writing file data...");

            long entryPosition = headerSize + (Magic.Length + 1);

            // iterate through all of the entries.
            for (int curEntry = 0; curEntry < Entries.Count; curEntry++)
            {     
                PackageFileCatalogEntry entry = Entries[curEntry];
                entryPosition += entry.Length;

                NCLogging.Log($"Writing file {entry.Path} to WAD...");
                
                // Read the file that this entry corresponds to.
                byte[] fileData = File.ReadAllBytes(entry.Path);

                // Calculate the file's CRC32.
                CRC32.NextBytes(fileData);
                uint fileCrc = CRC32.Result;

                // write the file data.
                // first store the position so we know when the file starts
                long curPosition = writer.BaseStream.Position;

                writer.Write(fileData);

                // subtract 20 bytes and write the crc32 and file position
                writer.BaseStream.Seek(entryPosition - 20, SeekOrigin.Begin);
                writer.Write(fileCrc);
                writer.Write(curPosition);
                // increment current position
                curPosition += entry.Size;

                // get ready to write the next file
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
            }
        }
    }
}
