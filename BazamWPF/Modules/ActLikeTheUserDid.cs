using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace BazamWPF.Modules
{
    public static class ActLikeTheUserDid
    {
        public static void AButtonClick(Button button)
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(button);
            IInvokeProvider provider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            provider.Invoke();
        }
    }
}