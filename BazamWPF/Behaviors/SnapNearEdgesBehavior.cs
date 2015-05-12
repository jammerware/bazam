using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace BazamWPF.Behaviors
{
    public class SnapNearEdgesBehavior : Behavior<Window>
    {
        public int SnapMargin
        {
            get { return (int)GetValue(SnapMarginProperty); }
            set { SetValue(SnapMarginProperty, value); }
        }

        public static readonly DependencyProperty SnapMarginProperty = DependencyProperty.Register(
            "SnapMargin", 
            typeof(int), 
            typeof(SnapNearEdgesBehavior), 
            new PropertyMetadata(20)
        );

        
        protected override void OnAttached()
        {
            AssociatedObject.LocationChanged += (weFreakingMoved, theStupidWindow) => {
                if (AssociatedObject.WindowState == WindowState.Normal) {
                    Screen currentScreen = Screen.FromHandle(new WindowInteropHelper(AssociatedObject).Handle);
                    Console.WriteLine(currentScreen.Bounds.Left.ToString() + ", " + currentScreen.Bounds.Top.ToString());

                    if (AssociatedObject.Left - currentScreen.Bounds.Left <= SnapMargin) {
                        AssociatedObject.Left = currentScreen.Bounds.Left;
                    }

                    if (AssociatedObject.Top - currentScreen.Bounds.Top <= SnapMargin) {
                        AssociatedObject.Top = currentScreen.Bounds.Top;
                    }
                }
            };
        }
    }
}