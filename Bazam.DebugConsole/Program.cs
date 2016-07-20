using System.IO;
using Bazam.Zipster;
using Microsoft.Extensions.PlatformAbstractions;

namespace Bazam.DebugConsole
{
    // because unit tests are things for people who have time
    public class Program
    {
        public static void Main(string[] args)
        {
            var envDir = PlatformServices.Default.Application.ApplicationBasePath;
            var pathThings = Path.Combine(envDir, "things.txt");
            var pathStuff = Path.Combine(envDir, "stuff.txt");

            using (var fileStreamThings = File.CreateText(pathThings))
            {
                fileStreamThings.WriteLine("Things lol");
            }

            using (var fileStreamStuff = File.CreateText(pathStuff))
            {
                fileStreamStuff.WriteLine("Stuff lol");
            }

            var zipDef = new BazamZip()
            {
                Files = new string[]
                {
                    pathThings,
                    pathStuff
                },
                ZipFileName = Path.Combine(envDir, "blarpleslamp.zip")
            };

            var bazamZipClient = new BazamZipClient();
            bazamZipClient.Zip(zipDef);
            bazamZipClient.Unzip(zipDef.ZipFileName, Path.Combine(envDir, "unzipped"), false);
        }
    }
}