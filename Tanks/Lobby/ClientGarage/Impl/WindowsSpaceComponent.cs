namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System.Collections.Generic;
    using UnityEngine;

    public class WindowsSpaceComponent : BehaviourComponent
    {
        [SerializeField]
        private List<Animator> animators;

        public List<Animator> Animators =>
            this.animators;
    }
}

