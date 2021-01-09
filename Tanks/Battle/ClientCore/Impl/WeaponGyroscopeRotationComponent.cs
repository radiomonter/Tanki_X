namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WeaponGyroscopeRotationComponent : Component
    {
        public float weaponTurnCoeff = 1f;

        public float GyroscopePower { get; set; }

        public Vector3 ForwardDir { get; set; }

        public Vector3 UpDir { get; set; }

        public float WeaponTurnCoeff
        {
            get => 
                this.weaponTurnCoeff;
            set => 
                this.weaponTurnCoeff = value;
        }

        public float DeltaAngleOfHullRotation { get; set; }
    }
}

