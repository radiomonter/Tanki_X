namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class EffectInstanceComponent : Component
    {
        public EffectInstanceComponent(UnityEngine.GameObject gameObject)
        {
            this.GameObject = gameObject;
            Object.DontDestroyOnLoad(gameObject);
        }

        public UnityEngine.GameObject GameObject { get; set; }
    }
}

