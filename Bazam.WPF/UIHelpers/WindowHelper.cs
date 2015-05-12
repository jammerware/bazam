using System.Linq;
using System.Windows;

namespace Bazam.WPF.UIHelpers
{
    public static class WindowHelper
    {
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return Application.Current.Windows.OfType<T>().Any(w => (string.IsNullOrEmpty(name) || w.Name.Equals(name)));
        }
    }
}