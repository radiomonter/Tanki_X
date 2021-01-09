namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;

    [SerialVersionUID(0x8d31cf3d34d1182L)]
    public class ScoreTableUserLabelIndicatorComponent : MonoBehaviour, Component
    {
        public GameObject userLabel;

        public void Awake()
        {
            this.userLabel = UserLabelBuilder.CreateDefaultLabel();
            this.userLabel.transform.SetParent(base.gameObject.transform, false);
        }
    }
}

