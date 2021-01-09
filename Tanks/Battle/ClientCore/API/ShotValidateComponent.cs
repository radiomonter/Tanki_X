namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShotValidateComponent : Component
    {
        public ShotValidateComponent()
        {
            this.BlockValidateMask = LayerMasks.STATIC;
            this.UnderGroundValidateMask = LayerMasks.STATIC;
        }

        public int BlockValidateMask { get; set; }

        public int UnderGroundValidateMask { get; set; }

        public GameObject[] RaycastExclusionGameObjects { get; set; }
    }
}

