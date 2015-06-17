using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Bazam.Wpf.Behaviors
{
    public class SmoothSizeChangedBehavior : Behavior<FrameworkElement>
    {
        private bool _DoShit = true;

        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += (object omg, SizeChangedEventArgs soAwesome) => {
                if (_DoShit) {
                    _DoShit = false;
                    Storyboard sb = new Storyboard();

                    DoubleAnimation animHeight = new DoubleAnimation(soAwesome.PreviousSize.Height, soAwesome.NewSize.Height, new Duration(TimeSpan.FromMilliseconds(300)));
                    animHeight.EasingFunction = new QuadraticEase();
                    animHeight.SetValue(Storyboard.TargetProperty, AssociatedObject);
                    animHeight.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Height"));

                    Storyboard.SetTarget(animHeight, AssociatedObject);
                    sb.Children.Add(animHeight);

                    DoubleAnimation animWidth = new DoubleAnimation(soAwesome.PreviousSize.Width, soAwesome.NewSize.Width, new Duration(TimeSpan.FromMilliseconds(300)));
                    animWidth.EasingFunction = new QuadraticEase();
                    animWidth.SetValue(Storyboard.TargetProperty, AssociatedObject);
                    animWidth.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Width"));

                    Storyboard.SetTarget(animWidth, AssociatedObject);
                    sb.Children.Add(animWidth);

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