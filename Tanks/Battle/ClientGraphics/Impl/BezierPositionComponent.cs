namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class BezierPositionComponent : Component
    {
        public BezierPositionComponent()
        {
            this.BezierPosition = new Tanks.Battle.ClientGraphics.Impl.BezierPosition();
        }

        public Tanks.Battle.ClientGraphics.Impl.BezierPosition BezierPosition { get; set; }
    }
}

