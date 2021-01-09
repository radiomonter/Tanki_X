namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class MuzzleFlashComponent : BehaviourComponent
    {
        public GameObject muzzleFlashPrefab;
        public float duration = 0.5f;
    }
}

