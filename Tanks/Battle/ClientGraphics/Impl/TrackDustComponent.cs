namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class TrackDustComponent : MonoBehaviour, Component
    {
        [NonSerialized]
        public float[] leftTrackDustDelay;
        [NonSerialized]
        public float[] rightTrackDustDelay;
    }
}

