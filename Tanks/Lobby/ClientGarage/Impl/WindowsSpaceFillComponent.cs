namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System.Collections.Generic;
    using UnityEngine;

    public class WindowsSpaceFillComponent : BehaviourComponent
    {
        [SerializeField]
        private List<Animator> animators = new List<Animator>();

        public List<Animator> Animators =>
            this.animators;
    }
}

