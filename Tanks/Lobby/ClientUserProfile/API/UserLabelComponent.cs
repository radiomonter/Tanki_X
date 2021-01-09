namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d3106d6647d924L)]
    public class UserLabelComponent : BehaviourComponent
    {
        [SerializeField]
        private long userId;
        public GameObject inBattleIcon;

        public bool SkipLoadUserFromServer { get; set; }

        public bool AllowInBattleIcon { get; set; }

        public long UserId
        {
            get => 
                this.userId;
            set => 
                this.userId = value;
        }
    }
}

