namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CollisionDustComponent : Component
    {
        public CollisionDustComponent()
        {
        }

        public CollisionDustComponent(Tanks.Battle.ClientGraphics.Impl.CollisionDustBehaviour collisionDustBehaviour)
        {
            this.CollisionDustBehaviour = collisionDustBehaviour;
        }

        public Tanks.Battle.ClientGraphics.Impl.CollisionDustBehaviour CollisionDustBehaviour { get; set; }
    }
}

