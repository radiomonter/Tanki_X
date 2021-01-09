namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(-1853333282151870933L)]
    public class RotationComponent : Component
    {
        public RotationComponent()
        {
        }

        public RotationComponent(Vector3 rotationEuler)
        {
            this.RotationEuler = rotationEuler;
        }

        public Vector3 RotationEuler { get; set; }
    }
}

