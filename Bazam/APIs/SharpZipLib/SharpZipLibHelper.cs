using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Bazam.APIs.SharpZipLib
{
    public static class SharpZipLibHelper
    {
        public static void Unzip(string from, string to, string password, bool deleteZipWhenDone)
        {
            using (FileStream fs = File.OpenRead(from)) {
                using (ZipFile zip = new ZipFile(fs)) {
                    zip.Password = password;

                    foreach (ZipEntry entry in zip) {
                        if (entry.IsFile) {
                            string entryFileName = entry.Name;
                            byte[] buffer = new byte[4096];

                            using (Stream zipStream = zip.GetInputStream(entry)) {
                                string fullZipPath = Path.Combine(to, entryFileName);
                                string directoryName = Path.GetDirectoryName(fullZipPath);
                                if (directoryName != string.Empty) {
                                    Directory.CreateDirectory(directoryName);
                                }

                                using (FileStream streamWriter = File.Create(fullZipPath)) {
                                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                                }
                            }
                        }
                    }
                }
            }

            if (deleteZipWhenDone) {
                File.Delete(from);
            }
        }

        public static void Zip(BazamZip zip, bool deleteComponentFilesWhenDone = false)
        {
            FileStream fsOut = File.Create(zip.ZipFileName);
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);

            zipStream.SetLevel(9);
            zipStream.Password = zip.Password;

            foreach (string fileName in zip.Files) {
                FileInfo file = new FileInfo(fileName);
                
                string entryName = fileName;

                if (!zip.PreserveFilePaths) {
                    entryName = Path.GetFileName(fileName);
                }
                else if (!string.IsNullOrEmpty(zip.FilesRelativeRootForZip)) {
                    entryName = entryName.Replace(zip.FilesRelativeRootForZip, string.Empty);
                }

                ZipEntry entry = new ZipEntry(entryName);
                entry.DateTime = file.LastWriteTime;
                entry.Size = file.Length;

                zipStream.PutNextEntry(entry);

                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(fileName)) {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }

            zipStream.IsStreamOwner = true;
            zipStream.Close();

            if (deleteComponentFilesWhenDone) {
                foreach (string fileName in zip.Files) {
                    File.Delete(fileName);
                }
            }
        }
    }
}