namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public interface EntityRegistry
    {
        bool ContainsEntity(long id);
        ICollection<Entity> GetAllEntities();
        Entity GetEntity(long id);
        Entity GetEntityForTest(string name);
        void RegisterEntity(Entity entity);
        void Remove(long id);
    }
}

