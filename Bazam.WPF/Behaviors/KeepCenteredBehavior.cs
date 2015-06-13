using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace Bazam.WPF.Behaviors
{
    public class KeepCenteredBehavior : Behavior<Window>
    {
        private bool _DoShit = true;

        private Point GetMidPoint()
        {
            Screen screen = Screen.FromHandle(new WindowInteropHelper(AssociatedObject).Handle);
            double screenHeight = screen.WorkingArea.Height;
            double screenWidth = screen.WorkingArea.Width;
            return new Point(screenWidth / 2, screen.Bounds.Y + (screenHeight / 2));
        }

        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += (omg, soAwesome) => {
                if (_DoShit && AssociatedObject.DesiredSize.Height > 0) {
                    _DoShit = false;

                    Point midpoint = GetMidPoint();
                    double newTop = midpoint.Y - (AssociatedObject.DesiredSize.Height / 2);

                    Storyboard sb = new Storyboard();
                    DoubleAnimation anim = new DoubleAnimation(AssociatedObject.Top, newTop, new Duration(TimeSpan.FromMilliseconds(150)));
                    anim.EasingFunction = new QuadraticEase();
                    anim.SetValue(Storyboard.TargetProperty, AssociatedObject);
                    anim.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Top"));

                    Storyboard.SetTarget(anim, AssociatedObject);
                    sb.Children.Add(anim);
                    sb.Completed += (fuck, it) => {
                        AssociatedObject.Top = newTop;
                        _DoShit = true;
                        sb.Stop();
                    };
                    sb.Begin();
                }
            };
        }
    }
}