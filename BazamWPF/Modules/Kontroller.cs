using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;

namespace BazamWPF.Modules
{
    public static class Kontroller
    {
        public static void Blur(FrameworkElement element)
        {
            FrameworkElement parent = (FrameworkElement)element.Parent;
            while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable) {
                parent = (FrameworkElement)parent.Parent;
            }
            DependencyObject scope = FocusManager.GetFocusScope(element);
            FocusManager.SetFocusedElement(scope, parent as IInputElement);
        }

        public static void Click(Button button)
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(button);
            IInvokeProvider provider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            provider.Invoke();
        }
    }
}
