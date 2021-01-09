namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ScreenForegroundComponent : MonoBehaviour, Component
    {
        public int Count { get; set; }
    }
}

