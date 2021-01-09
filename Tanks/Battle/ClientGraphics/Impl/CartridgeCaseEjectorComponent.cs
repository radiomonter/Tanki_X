namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class CartridgeCaseEjectorComponent : MonoBehaviour, Component
    {
        public GameObject casePrefab;
        public float initialAngularSpeed;
        public float initialSpeed;
    }
}

