using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace BazamWPF.Behaviors
{
    public class FadeInBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.IsVisibleChanged += (sender, e) => {
                if (AssociatedObject.IsVisible) {
                    DoubleAnimation opacityAnim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(500)));
                    opacityAnim.EasingFunction = new QuadraticEase();
                    opacityAnim.SetValue(Storyboard.TargetProperty, AssociatedObject);
                    opacityAnim.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Opacity"));

                    Storyboard opacitySb = new Storyboard();
                    Storyboard.SetTarget(opacityAnim, AssociatedObject);
                    opacitySb.Children.Add(opacityAnim);

                    ThicknessAnimation marginAnim = new ThicknessAnimation(new Thickness(AssociatedObject.Margin.Left + 8, AssociatedObject.Margin.Top, AssociatedObject.Margin.Right, AssociatedObject.Margin.Bottom), AssociatedObject.Margin, new Duration(TimeSpan.FromMilliseconds(500)), FillBehavior.Stop);
                    marginAnim.EasingFunction = new QuadraticEase();
                    marginAnim.SetValue(Storyboard.TargetProperty, AssociatedObject);
                    marginAnim.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Margin"));

                    Storyboard marginSb = new Storyboard();
                    Storyboard.SetTarget(marginAnim, AssociatedObject);
                    marginSb.Children.Add(marginAnim);

                    opacitySb.Begin();
                    marginSb.Begin();
                }
            };

            AssociatedObject.Loaded += (bitch, turd) => {
                DoubleAnimation opacityAnim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(500)));
                opacityAnim.EasingFunction = new QuadraticEase();
                opacityAnim.SetValue(Storyboard.TargetProperty, AssociatedObject);
                opacityAnim.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Opacity"));

                Storyboard opacitySb = new Storyboard();
                Storyboard.SetTarget(opacityAnim, AssociatedObject);
                opacitySb.Children.Add(opacityAnim);

                ThicknessAnimation marginAnim = new ThicknessAnimation(new Thickness(AssociatedObject.Margin.Left + 8, AssociatedObject.Margin.Top, AssociatedObject.Margin.Right, AssociatedObject.Margin.Bottom), AssociatedObject.Margin, new Duration(TimeSpan.FromMilliseconds(500)), FillBehavior.Stop);
                marginAnim.EasingFunction = new QuadraticEase();
                marginAnim.SetValue(Storyboard.TargetProperty, AssociatedObject);
                marginAnim.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("Margin"));

                Storyboard marginSb = new Storyboard();
                Storyboard.SetTarget(marginAnim, AssociatedObject);
                marginSb.Children.Add(marginAnim);

                opacitySb.Begin();
                marginSb.Begin();
            };
        }
    }
}