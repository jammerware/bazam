using System.Linq;
using System.Windows;

namespace BazamWPF.UIHelpers
{
    public static sealed class WindowHelper
    {
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return Application.Current.Windows.OfType<T>().Any(w => (string.IsNullOrEmpty(name) || w.Name.Equals(name)));
        }
    }
}
