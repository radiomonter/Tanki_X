namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Shared, SerialVersionUID(0x1fd5d16fd50b46L)]
    public class SpatialGeometryComponent : Component
    {
        public SpatialGeometryComponent()
        {
        }

        public SpatialGeometryComponent(Vector3 postion, Vector3 rotation)
        {
            this.Position = postion;
            this.Rotation = rotation;
        }

        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }
    }
}

