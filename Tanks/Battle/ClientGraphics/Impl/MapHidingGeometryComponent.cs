namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class MapHidingGeometryComponent : Component
    {
        public Renderer[] hidingRenderers;

        public MapHidingGeometryComponent()
        {
        }

        public MapHidingGeometryComponent(Renderer[] hidingRenderers)
        {
            this.hidingRenderers = hidingRenderers;
        }
    }
}

