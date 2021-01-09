namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class ItemPackButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private int count;

        public int Count
        {
            get => 
                this.count;
            set => 
                this.count = value;
        }
    }
}

