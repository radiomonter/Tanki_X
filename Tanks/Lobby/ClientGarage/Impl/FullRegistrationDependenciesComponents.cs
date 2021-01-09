namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System.Collections.Generic;
    using UnityEngine;

    public class FullRegistrationDependenciesComponents : MonoBehaviour, Component
    {
        [SerializeField]
        private List<GameObject> dependecies;

        public List<GameObject> Dependecies =>
            this.dependecies;
    }
}

