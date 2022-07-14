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

        internal void Write(BinaryWriter writer, int headerSize)
        {
            writer.Write(Magic);
            writer.Write(NumberOfEntries);
            
            // write each entry in the catalog
            foreach (PackageFileCatalogEntry entry in Entries)
            {
                entry.Write(writer);
            }

            NCLogging.Log("Wrote catalog entries, writing file data...");

            long entryPosition = headerSize;

            for (int curEntry = 0; curEntry < Entries.Count; curEntry++)
            {     
                PackageFileCatalogEntry entry = Entries[curEntry];
                entryPosition += entry.Length;

                NCLogging.Log($"Writing file {entry.Path} to WAD...")
                    ;
                byte[] fileData = File.ReadAllBytes(entry.Path);

                // write the file data.
                // first store the position so we know when the file starts
                long curPosition = writer.BaseStream.Position;

                writer.Write(fileData);

                // subtract 8 and write the real file position
                writer.BaseStream.Seek(entryPosition - 16, SeekOrigin.Begin);
                writer.Write(curPosition);

                curPosition += entry.Size;

                // get ready to write the next file
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
            }
        }
    }
}
