namespace Tanks.Battle.ClientCore.API
{
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public abstract class TankPartVisualRootComponent : MonoBehaviour
    {
        [SerializeField]
        private VisualTriggerMarkerComponent visualTriggerMarker;

        protected TankPartVisualRootComponent()
        {
        }

        public VisualTriggerMarkerComponent VisualTriggerMarker =>
            this.visualTriggerMarker;
    }
}

