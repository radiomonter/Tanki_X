namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class BattleLevelsIndicatorComponent : BehaviourComponent
    {
        public void Hide()
        {
            base.gameObject.SetActive(false);
        }

        public void ShowText(string text)
        {
            base.gameObject.SetActive(true);
            base.GetComponent<Text>().text = text;
        }
    }
}

