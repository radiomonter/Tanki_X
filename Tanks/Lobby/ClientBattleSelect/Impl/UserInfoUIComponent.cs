namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class UserInfoUIComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject defaultInfo;
        [SerializeField]
        private GameObject squadInfo;

        public void SwitchSquadInfo(bool value)
        {
            this.squadInfo.SetActive(value);
            this.defaultInfo.SetActive(!value);
        }
    }
}

