namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class SoundListenerMusicSnapshotsComponent : BehaviourComponent
    {
        [SerializeField]
        private int hymnLoopSnapshot;
        [SerializeField]
        private int battleResultMusicSnapshot = 1;
        [SerializeField]
        private int cardsContainerMusicSnapshot = 2;
        [SerializeField]
        private int garageModuleMusicSnapshot = 3;

        public int HymnLoopSnapshot =>
            this.hymnLoopSnapshot;

        public int BattleResultMusicSnapshot =>
            this.battleResultMusicSnapshot;

        public int CardsContainerMusicSnapshot =>
            this.cardsContainerMusicSnapshot;

        public int GarageModuleMusicSnapshot =>
            this.garageModuleMusicSnapshot;
    }
}

