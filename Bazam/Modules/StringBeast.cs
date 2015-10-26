using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Bazam.Modules
{
    public static class StringBeast
    {
        public static string Capitalize(string input)
        {
            return Capitalize(input, false);
        }

        public static string Capitalize(string input, bool lowercaseRest)
        {
            string output = string.Empty;
            if (input.Length > 0) {
                output = input.Substring(0, 1).ToUpper();
                if (input.Length > 1) {
                    output += (lowercaseRest ? input.Substring(1).ToLower() : input.Substring(1));
                }
            }
            return output;
        }

        public static string StripIntegers(string input)
        {
            StringBuilder output = new StringBuilder();
            int dummy;
            foreach (Char c in input.ToCharArray()) {
                if (Int32.TryParse(c.ToString(), out dummy))
                    output.Append(c);
            }
            return output.ToString();
        }

        #region Compression
        // this made possible by this SO question: http://stackoverflow.com/questions/7343465/compression-decompression-string-with-c-sharp
        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string Compress(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                    CopyTo(msi, gs);
                }

                return Convert.ToBase64String(mso.ToArray());
            }
        }

        public static string Decompress(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                    CopyTo(gs, mso);
                }

                return Convert.ToBase64String(mso.ToArray());
            }
        }
        #endregion
    }
}