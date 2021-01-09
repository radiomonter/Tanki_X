namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class UidColorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private UnityEngine.Color friendColor;
        [SerializeField]
        private UnityEngine.Color moderatorColor;
        [SerializeField]
        private UnityEngine.Color color;

        public UnityEngine.Color FriendColor
        {
            get => 
                this.friendColor;
            set => 
                this.friendColor = value;
        }

        public UnityEngine.Color ModeratorColor
        {
            get => 
                this.moderatorColor;
            set => 
                this.moderatorColor = value;
        }

        public UnityEngine.Color Color
        {
            get => 
                this.color;
            set => 
                this.color = value;
        }
    }
}

