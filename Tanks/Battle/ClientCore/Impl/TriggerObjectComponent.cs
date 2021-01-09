namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TriggerObjectComponent : Component
    {
        public TriggerObjectComponent()
        {
        }

        public TriggerObjectComponent(GameObject triggerObject)
        {
            this.TriggerObject = triggerObject;
        }

        public GameObject TriggerObject { get; set; }
    }
}

