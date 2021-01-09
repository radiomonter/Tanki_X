namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UpdateLoadGearProgressEvent : Event
    {
        public UpdateLoadGearProgressEvent(float value)
        {
            this.Value = Mathf.Clamp01(value);
        }

        public float Value { get; set; }
    }
}

