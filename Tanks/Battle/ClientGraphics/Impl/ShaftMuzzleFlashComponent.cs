namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ShaftMuzzleFlashComponent : MonoBehaviour, Component
    {
        public GameObject muzzleFlashPrefab;
        public float duration = 0.5f;
    }
}

