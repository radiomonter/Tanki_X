namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class StreamingDecalProjectorComponent : DynamicDecalProjectorComponent
    {
        [SerializeField]
        private float decalCreationPeriod = 0.2f;

        public float DecalCreationPeriod =>
            this.decalCreationPeriod;

        public float LastDecalCreationTime { get; set; }
    }
}

