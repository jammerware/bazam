using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Jammerware.Utilities
{
    public static class AnimationBuddy
    {
        private static TimeSpan DEFAULT_DURATION = TimeSpan.FromMilliseconds(300);

        private static Storyboard GetDefaultStoryboard()
        {
            Storyboard retVal = new Storyboard();
            retVal.Duration = DEFAULT_DURATION;

            DoubleAnimation anim = new DoubleAnimation();
            anim.Duration = DEFAULT_DURATION;
            anim.EasingFunction = new QuadraticEase();
            retVal.Children.Add(anim);

            return retVal;
        }

        private static DoubleAnimation GetDefaultDoubleAnimation()
        {
            DoubleAnimation retVal = new DoubleAnimation();
            retVal.Duration = DEFAULT_DURATION;
            retVal.EasingFunction = new QuadraticEase();
            return retVal;
        }

        public static Storyboard GetAnimation(DependencyObject target, string path, Double to)
        {
            return GetAnimation(target, path, to, 0);
        }

        public static Storyboard GetAnimation(DependencyObject target, string path, Double to, Double duration)
        {
            Storyboard retVal = GetDefaultStoryboard();
            DoubleAnimation anim = retVal.Children[0] as DoubleAnimation;
            anim.To = to;
            Duration d = new Duration(duration > 0 ? TimeSpan.FromMilliseconds(duration) : DEFAULT_DURATION);
            anim.Duration = d;
            retVal.Duration = d;
            retVal.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(path));
            Storyboard.SetTarget(anim, target);

            return retVal;
        }

        public static void Animate(DependencyObject target, string path, Double to)
        {
            Animate(target, path, to, 0, null);
        }

        public static void Animate(DependencyObject target, string path, Double to, Double duration)
        {
            Animate(target, path, to, duration, null);
        }

        public static void Animate(DependencyObject target, string path, Double to, Double duration, EventHandler completionHandler)
        {
            Storyboard sb = GetAnimation(target, path, to, duration);
            if (completionHandler != null)
                sb.Completed += completionHandler;
            if (Storyboard.GetTargetName(sb) != string.Empty) {
                target.Dispatcher.BeginInvoke(new Action(() => { sb.Begin(); }));
            }
        }
    }
}