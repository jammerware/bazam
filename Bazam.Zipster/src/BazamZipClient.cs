using System.IO;
using System.IO.Compression;

namespace Bazam.Zipster
{
    public class BazamZipClient
    {
        public void Unzip(string from, string to, bool deleteZipWhenDone = false)
        {
            using (var zipArchive = ZipFile.OpenRead(from))
            {
                zipArchive.ExtractToDirectory(to);
            }

            if (deleteZipWhenDone)
            {
                File.Delete(from);
            }
        }

        public void Zip(BazamZip zipDefinition, bool deleteComponentFilesWhenDone = false)
        {
            using (var zip = ZipFile.Open(zipDefinition.ZipFileName, ZipArchiveMode.Create))
            {
                foreach (var fileName in zipDefinition.Files)
                {
                    var fileInfo = new FileInfo(fileName);
                    var entryName = fileName;

                    if (!zipDefinition.PreserveFilePaths)
                    {
                        entryName = Path.GetFileName(fileName);
                    }
                    else if (!string.IsNullOrEmpty(zipDefinition.FilesRelativeRootForZip))
                    {
                        entryName = entryName.Replace(zipDefinition.FilesRelativeRootForZip, string.Empty);
                    }
                    
                    zip.CreateEntryFromFile(fileName, entryName, CompressionLevel.Optimal);
                }                
            }

            if (deleteComponentFilesWhenDone)
            {
                foreach (string fileName in zipDefinition.Files)
                {
                    File.Delete(fileName);
                }
            }
        }
    }
}