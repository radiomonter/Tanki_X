namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GraphicEffectComponent : Component
    {
        public GraphicEffectComponent()
        {
        }

        public GraphicEffectComponent(GameObject effectObject)
        {
            this.EffectObject = effectObject;
        }

        public GameObject EffectObject { get; set; }
    }
}

