namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class SmartConsoleActivator : MonoBehaviour, Component
    {
        [SerializeField]
        private GameObject smartConsole;

        public GameObject SmartConsole
        {
            get => 
                this.smartConsole;
            set => 
                this.smartConsole = value;
        }
    }
}

