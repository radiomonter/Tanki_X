namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e1567ba59aL)]
    public class AssembledTankComponent : Component
    {
        public AssembledTankComponent()
        {
        }

        public AssembledTankComponent(GameObject assemblyRoot)
        {
            this.AssemblyRoot = assemblyRoot;
        }

        public GameObject AssemblyRoot { get; set; }
    }
}

