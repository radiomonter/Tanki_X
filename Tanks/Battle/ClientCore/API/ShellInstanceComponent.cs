namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShellInstanceComponent : Component
    {
        public ShellInstanceComponent()
        {
        }

        public ShellInstanceComponent(GameObject shellInstance)
        {
            this.ShellInstance = shellInstance;
        }

        public GameObject ShellInstance { get; set; }
    }
}

