namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class EntityUnshareCommand : EntityCommand
    {
        private void DeleteEntity(Engine engine)
        {
            engine.DeleteEntity(base.Entity);
            SharedEntityRegistry.SetUnshared(base.Entity.Id);
        }

        public override void Execute(Engine engine)
        {
            this.DeleteEntity(engine);
        }

        public override string ToString() => 
            $"EntityUnshareCommand: Entity={base.Entity}";

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.Impl.SharedEntityRegistry SharedEntityRegistry { get; set; }
    }
}

