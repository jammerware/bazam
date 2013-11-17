using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace BazamWPF.Behaviors
{
    public class SmoothSizeChangedBehavior : Behavior<FrameworkElement>
    {
        private bool _DoShit = true;

        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += (omg, soAwesome) => {
                if (_DoShit && AssociatedObject.DesiredSize.Height > 0) {
                    _DoShit = false;
                    Storyboard sb = new Storyboard();

                    if (AssociatedObject.DesiredSize.Height > 0) {
                        DoubleAnimation anim = new DoubleAnimation(soAwesome.PreviousSize.Height, AssociatedObject.DesiredSize.Height, new Duration(TimeSpan.FromMilliseconds(300)));
                        anim.EasingFunction = new QuadraticEase();
                        anim.SetValue(Storyboard.TargetProperty, AssociatedObject);
                        anim.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Height"));

                        Storyboard.SetTarget(anim, AssociatedObject);
                        sb.Children.Add(anim);
                    }

                    if (AssociatedObject.DesiredSize.Width > 0) {
                        DoubleAnimation anim = new DoubleAnimation(soAwesome.PreviousSize.Width, AssociatedObject.DesiredSize.Width, new Duration(TimeSpan.FromMilliseconds(300)));
                        anim.EasingFunction = new QuadraticEase();
                        anim.SetValue(Storyboard.TargetProperty, AssociatedObject);
                        anim.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Width"));

                        Storyboard.SetTarget(anim, AssociatedObject);
                        sb.Children.Add(anim);
                    }

                    sb.Completed += (fuck, it) => {
                        _DoShit = true;
                        sb.Stop();
                    };
                    sb.Begin();
                }
            };
        }
    }
}
