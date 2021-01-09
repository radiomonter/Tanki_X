namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;

    public class StreakTerminationUIComponent : BehaviourComponent
    {
        public TextMeshProUGUI streakTerminationText;
        public LocalizedField streakTerminationLocalization;
    }
}

