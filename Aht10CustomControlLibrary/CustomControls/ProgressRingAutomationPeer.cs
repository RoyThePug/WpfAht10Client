using System.Windows.Automation.Peers;

namespace Aht10CustomControlLibrary.CustomControls
{
    public class ProgressRingAutomationPeer : FrameworkElementAutomationPeer
    {
        public ProgressRingAutomationPeer(ProgressRing.ProgressRing owner)
            : base(owner)
        {
        }

        protected override string GetClassNameCore()
        {
            return nameof(ProgressRing);
        }

        protected override string GetNameCore()
        {
            string? nameCore = base.GetNameCore();

            if (Owner is ProgressRing.ProgressRing { IsActive: true })
            {
                return nameof(ProgressRing.ProgressRing.IsActive) + nameCore;
            }

            return nameCore!;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ProgressBar;
        }
    }
}