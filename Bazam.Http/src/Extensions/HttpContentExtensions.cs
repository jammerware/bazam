using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bazam.Http
{
    public static class HttpContentExtensions
    {
        public async static Task ReadAsFileAsync(this HttpContent content, string fileName)
        {
            string pathName = Path.GetFullPath(fileName);
            
            using(FileStream fileStream = new FileStream(pathName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                await content.CopyToAsync(fileStream);
            }
        }
    }
}