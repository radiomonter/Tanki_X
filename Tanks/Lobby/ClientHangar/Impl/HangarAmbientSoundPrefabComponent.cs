namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class HangarAmbientSoundPrefabComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Tanks.Lobby.ClientHangar.Impl.HangarAmbientSoundController hangarAmbientSoundController;

        public Tanks.Lobby.ClientHangar.Impl.HangarAmbientSoundController HangarAmbientSoundController
        {
            get => 
                this.hangarAmbientSoundController;
            set => 
                this.hangarAmbientSoundController = value;
        }
    }
}

