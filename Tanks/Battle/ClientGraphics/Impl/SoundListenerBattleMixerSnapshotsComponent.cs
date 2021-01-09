namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class SoundListenerBattleMixerSnapshotsComponent : BehaviourComponent
    {
        [SerializeField]
        private int loudSnapshotIndex;
        [SerializeField]
        private int silentSnapshotIndex = 1;
        [SerializeField]
        private int selfUserSnapshotIndex = 2;
        [SerializeField]
        private int melodySilentSnapshotIndex = 3;

        public int LoudSnapshotIndex =>
            this.loudSnapshotIndex;

        public int SilentSnapshotIndex =>
            this.silentSnapshotIndex;

        public int SelfUserSnapshotIndex =>
            this.selfUserSnapshotIndex;

        public int MelodySilentSnapshotIndex =>
            this.melodySilentSnapshotIndex;
    }
}

