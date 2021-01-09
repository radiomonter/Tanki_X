namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UIListComponent : MonoBehaviour, Component
    {
        private void Awake()
        {
            this.List = base.GetComponent<IUIList>();
        }

        public IUIList List { get; private set; }
    }
}

