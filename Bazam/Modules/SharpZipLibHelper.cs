using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Bazam.Modules
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
    }
}
