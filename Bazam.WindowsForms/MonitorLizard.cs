using System.Drawing;
using System.Windows.Forms;

namespace Bazam.Modules
{
    public static class MonitorLizard
    {
        public static Screen[] GetDisplays()
        {
            return Screen.AllScreens;
        }

        public static Point GetMidpointForDisplay(Screen screen)
        {
            Rectangle screenBounds = screen.Bounds;
            return new Point((screenBounds.Right - screenBounds.Left) / 2, (screenBounds.Bottom - screenBounds.Top) / 2);
        }
    }
}