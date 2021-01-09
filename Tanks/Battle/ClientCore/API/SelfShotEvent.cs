namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using UnityEngine;

    [Shared, SerialVersionUID(0x4b7ee5751a216347L)]
    public class SelfShotEvent : BaseShotEvent
    {
        public SelfShotEvent()
        {
        }

        public SelfShotEvent(Vector3 shotDirection) : base(shotDirection)
        {
        }
    }
}

