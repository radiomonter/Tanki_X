namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SelectedLocaleComponent : MonoBehaviour, Component
    {
        public string Code { get; set; }
    }
}

