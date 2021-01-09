namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public class ESMComponent : Component, AttachToEntityListener
    {
        public EntityStateMachine esm = new EntityStateMachineImpl();

        public void AttachedToEntity(Entity entity)
        {
            this.Esm.AttachToEntity(entity);
        }

        public EntityStateMachine Esm =>
            this.esm;
    }
}

