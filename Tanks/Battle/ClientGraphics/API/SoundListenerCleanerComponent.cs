namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class SoundListenerCleanerComponent : BehaviourComponent
    {
        [SerializeField]
        private float tankPartCleanTimeSec = 2f;
        [SerializeField]
        private float mineCleanTimeSec = 2.2f;
        [SerializeField]
        private float ctfCleanTimeSec = 5.2f;

        public float TankPartCleanTimeSec =>
            this.tankPartCleanTimeSec;

        public float MineCleanTimeSec =>
            this.mineCleanTimeSec;

        public float CTFCleanTimeSec =>
            this.ctfCleanTimeSec;
    }
}

