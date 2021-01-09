namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class BonusRegionClientConfigComponent : MonoBehaviour, Component
    {
        public float maxOpacityRadius = 20f;
        public float minOpacityRadius = 30f;
        public float opacityChangingSpeed = 1f;
    }
}

