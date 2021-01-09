namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;

    public class MatchSearchingGUIComponent : BehaviourComponent
    {
        public LocalizedField newbieSearchTime;
        public LocalizedField regularSearchTime;
        public TextMeshProUGUI text;

        public void SetWaitingTime(bool newbieMode)
        {
            this.text.text = !newbieMode ? this.regularSearchTime.Value : this.newbieSearchTime.Value;
        }
    }
}

