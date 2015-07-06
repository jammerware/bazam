using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Bazam.Wpf.UIHelpers
{
    public static class ImageFactory
    {
        public async static Task<BitmapImage> FromUri(Uri uri)
        {
            return await Task.Run<BitmapImage>(() => {
                try {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.UriSource = uri;
                    img.EndInit();
                    img.Freeze();
                    return img;
                }
                catch (Exception) {
                    // we'll come back to this, but I think MtGBar will break if i don't catch this
                }
                return null;
            });
        }
    }
}
