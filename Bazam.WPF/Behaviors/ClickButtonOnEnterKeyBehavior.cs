using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Bazam.Wpf.Behaviors
{
    public class ClickButtonOnEnterKeyBehavior : Behavior<TextBox>
    {
        public Button Button
        {
            get { return (Button)GetValue(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
        }

        public static readonly DependencyProperty ButtonProperty = DependencyProperty.Register(
            "Button",
            typeof(Button), 
            typeof(ClickButtonOnEnterKeyBehavior), 
            new PropertyMetadata(null)
        );

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.KeyUp += (object sender, KeyEventArgs args) => {
                if ((args.Key == Key.Return || args.Key == Key.Enter) && this.Button != null) {
                    ButtonAutomationPeer peer = new ButtonAutomationPeer(this.Button);
                    (peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider).Invoke();
                }
            };
        }
    }
}