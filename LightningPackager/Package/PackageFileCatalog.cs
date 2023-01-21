using NuCore.Utilities;

namespace LightningPackager
{
    public class PackageFileCatalog
    {
        public const string Magic = "here!";
        public int NumberOfEntries => Entries.Count;
        public List<PackageFileCatalogEntry> Entries { get; private set; }

        internal PackageFileCatalog()
        {
            Entries = new List<PackageFileCatalogEntry>();
        }

        internal void AddEntry(PackageFileCatalogEntry entry)
        {
            NCLogging.Log($"Adding WAD catalog entry for filename {entry.FilePath}...");
            Entries.Add(entry);
        }

        /// <summary>
        /// Reads the file catalog.
        /// </summary>
        /// <param name="reader">The stream to read the file catalog from.</param>
        /// <returns></returns>
        internal static PackageFileCatalog Read(BinaryReader reader)
        {
            string magic = reader.ReadString();

            if (magic != Magic)
            {
                NCLogging.LogError($"Invalid magic for file catalog - expected {magic}, got {Magic}!", 102, NCLoggingSeverity.Error, null, true);
                return null;
            }

            PackageFileCatalog catalog = new PackageFileCatalog();

            // read the number of entries.
            int numberOfEntries = reader.ReadInt32();

            NCLogging.Log($"Number of entries = {numberOfEntries}");

            for (int entryId = 0; entryId < numberOfEntries; entryId++)
            {
                PackageFileCatalogEntry entry = PackageFileCatalogEntry.Read(reader);

                NCLogging.Log($"Read Entry {entryId + 1}/{numberOfEntries}\n" +
                    $"Path: {entry.FilePath}\n" +
                    $"Timestamp: {entry.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
                    $"Crc32: 0x{entry.Crc32.ToString("X")}\n" +
                    $"Start: (offset to start of file): {entry.Start}\n" +
                    $"Size: (offset to size of file): {entry.Size}\n");

                catalog.AddEntry(entry);
            }

            return catalog;
        }

        /// <summary>
        /// Extracts all the files in this <see cref="PackageFileCatalog"/>.
        /// </summary>
        /// <param name="reader">A <see cref="BinaryReader"/> to read the files from.</param>
        /// <param name="outFolder">The output folder to output the extracted files to.</param>
        internal void Extract(BinaryReader reader, string outFolder)
        {
            foreach (PackageFileCatalogEntry entry in Entries)
            {
                entry.Extract(reader, outFolder);
            }
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

            long entryPosition = headerSize + (Magic.Length + 1) + 4;

            // iterate through all of the entries.
            for (int curEntry = 0; curEntry < Entries.Count; curEntry++)
            {
                PackageFileCatalogEntry entry = Entries[curEntry];
                entryPosition += entry.Length;

                NCLogging.Log($"Writing file {entry.FilePath} to WAD...");

                // Read the file that this entry corresponds to.
                byte[] fileData = File.ReadAllBytes(entry.FilePath);

                // Calculate the file's CRC32.
                CRC32.NextBytes(fileData);
                uint fileCrc = CRC32.Result;

                // write the file data.
                // first store the position so we know when the file starts
                long curPosition = writer.BaseStream.Position;

                writer.Write(fileData);

                // subtract 20 bytes (so that we write it in the right place) and write the crc32 and file position
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
