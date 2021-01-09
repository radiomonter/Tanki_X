namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MyShadowInternal : MonoBehaviour
    {
        public UnityEngine.Projector Projector { get; set; }

        public Material CasterMaterial { get; set; }

        public float BaseAlpha { get; set; }

        public Bounds ProjectionBoundInLightSpace { get; set; }
    }
}

