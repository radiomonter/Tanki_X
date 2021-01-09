namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    [SerialVersionUID(0x15ef132478cL)]
    public class UserLabelWaitIntiveResponseIconComponent : BehaviourComponent
    {
        [SerializeField]
        public GameObject icon;

        public bool Wait
        {
            set => 
                this.icon.SetActive(value);
        }
    }
}

