using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace BazamWPF.Behaviors
{
    public class KeepCenteredBehavior : Behavior<Window>
    {
        private bool _DoShit = true;
        private Point _ScreenMidpoint;

        protected override void OnAttached()
        {
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            _ScreenMidpoint = new Point(screenWidth / 2, screenHeight / 2);

            AssociatedObject.SizeChanged += (omg, soAwesome) => {
                if (_DoShit && AssociatedObject.DesiredSize.Height > 0) {
                    _DoShit = false;
                    
                    double newLeft = _ScreenMidpoint.X - (AssociatedObject.DesiredSize.Width / 2);
                    double newTop = _ScreenMidpoint.Y - (AssociatedObject.DesiredSize.Height / 2);

                    Storyboard sb = new Storyboard();
                    DoubleAnimation anim = new DoubleAnimation(AssociatedObject.Top, newTop, new Duration(TimeSpan.FromMilliseconds(150)));
                    anim.EasingFunction = new QuadraticEase();
                    anim.SetValue(Storyboard.TargetProperty, AssociatedObject);
                    anim.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Top"));

                    Storyboard.SetTarget(anim, AssociatedObject);
                    sb.Children.Add(anim);
                    sb.Completed += (fuck, it) => {
                        _DoShit = true;
                        sb.Stop();
                    };
                    sb.Begin();

                    //AssociatedObject.Left = newLeft;
                    //AssociatedObject.Top = newTop;
                }
            };
        }
    }
}
