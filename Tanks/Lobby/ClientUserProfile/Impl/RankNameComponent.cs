namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class RankNameComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Text rankNameText;

        public string RankName
        {
            get => 
                this.rankNameText.text;
            set => 
                this.rankNameText.text = value;
        }
    }
}

