using System.Collections.Generic;

namespace Bazam.APIs.SharpZipLib
{
    public class BazamZip
    {
        /// <summary>
        /// The list of files to include in the zip.
        /// </summary>
        public IList<string> Files { get; set; }

        /// <summary>
        /// The root path that will be stripped from each file when it's placed into the zip. This lets you do things like zipping up
        /// "C:\Temp\log.txt" and "C:\Temp\My\stuff.txt" and getting a zip containing "log.txt" and "My\stuff.txt" by setting this
        /// property to "C:\Temp".
        /// 
        /// Note that this property has no effect unless the PreserveFilePaths property is set to true.
        /// </summary>
        public string FilesRelativeRootForZip { get; set; }

        /// <summary>
        /// The password to the zip - setting this to null, empty string, or just not setting it are all identical.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// If this is set to true, files in the Files list property will preserve their original file paths when zipped. If
        /// false, the zip will be "flat" - all files will be placed in the root of the zip.
        /// </summary>
        public bool PreserveFilePaths { get; set; }

        /// <summary>
        /// The file name and path at which this zip will be written.
        /// </summary>
        public string ZipFileName { get; set; }
    }
}