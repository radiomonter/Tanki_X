namespace Tanks.Battle.ClientGraphics.impl
{
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class HammerDecalProjectorComponent : DynamicDecalProjectorComponent
    {
        [SerializeField]
        private float combineHalfSize = 5f;

        public float CombineHalfSize
        {
            get => 
                this.combineHalfSize;
            set => 
                this.combineHalfSize = value;
        }
    }
}

