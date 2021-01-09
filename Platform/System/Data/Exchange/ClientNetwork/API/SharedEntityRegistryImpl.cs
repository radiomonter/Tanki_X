namespace Platform.System.Data.Exchange.ClientNetwork.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SharedEntityRegistryImpl : SharedEntityRegistry
    {
        public static float CLEAN_PERIOD_SEC = 60f;
        public static float UNSHARE_EXPIRE_PERIOD_SEC = 60f;
        private readonly EngineServiceInternal engineService;
        private Dictionary<long, EntityEntry> registry = new Dictionary<long, EntityEntry>();
        private List<long> entityToClean = new List<long>(100);
        private double lastCleanTime;

        public SharedEntityRegistryImpl(EngineServiceInternal engineService)
        {
            this.engineService = engineService;
        }

        private void Clean()
        {
            Dictionary<long, EntityEntry>.Enumerator enumerator = this.registry.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<long, EntityEntry> current = enumerator.Current;
                EntityEntry entry = current.Value;
                if (entry.IsEntryExpired())
                {
                    this.entityToClean.Add(enumerator.Current.Key);
                }
            }
            for (int i = 0; i < this.entityToClean.Count; i++)
            {
                this.registry.Remove(this.entityToClean[i]);
            }
            this.entityToClean.Clear();
        }

        public EntityInternal CreateEntity(long entityId)
        {
            EntityInternal entity = this.engineService.CreateEntityBuilder().SetId(entityId).Build(false);
            this.RegisterEntity(entity, EntityState.Created);
            return entity;
        }

        public EntityInternal CreateEntity(long entityId, Optional<TemplateAccessor> templateAccessor)
        {
            EntityInternal entity = this.engineService.CreateEntityBuilder().SetId(entityId).SetTemplateAccessor(templateAccessor).Build(false);
            this.RegisterEntity(entity, EntityState.Created);
            return entity;
        }

        public EntityInternal CreateEntity(long templateId, string configPath, long entityId)
        {
            EntityInternal entity = this.engineService.CreateEntityBuilder().SetId(entityId).SetName(configPath).SetConfig(configPath).SetTemplate(TemplateRegistry.GetTemplateInfo(templateId)).Build(false);
            this.RegisterEntity(entity, EntityState.Created);
            return entity;
        }

        public bool IsShared(long entityId) => 
            this.registry.ContainsKey(entityId) && (this.registry[entityId].state == EntityState.Shared);

        private bool IsTimeToClean()
        {
            if ((PreciseTime.Time - this.lastCleanTime) < CLEAN_PERIOD_SEC)
            {
                return false;
            }
            this.lastCleanTime = PreciseTime.Time;
            return true;
        }

        private void RegisterEntity(EntityInternal entity, EntityState state)
        {
            if (this.registry.ContainsKey(entity.Id))
            {
                throw new EntityAlreadyRegisteredException(entity);
            }
            this.registry.Add(entity.Id, new EntityEntry(EntityState.Created, entity));
        }

        public void SetShared(long entityId)
        {
            EntityEntry entry;
            if (!this.registry.TryGetValue(entityId, out entry))
            {
                throw new EntityByIdNotFoundException(entityId);
            }
            if (entry.state == EntityState.Shared)
            {
                throw new EntityAlreadySharedException(entry.entity);
            }
            entry.entity.Init();
            Flow.Current.EntityRegistry.RegisterEntity(entry.entity);
            entry.state = EntityState.Shared;
            entry.entity.AddComponent<SharedEntityComponent>();
        }

        public void SetUnshared(long entityId)
        {
            EntityEntry entry;
            if (!this.registry.TryGetValue(entityId, out entry) || (entry.state != EntityState.Shared))
            {
                throw new EntityByIdNotFoundException(entityId);
            }
            entry.state = EntityState.Unshared;
            entry.unsharedTime = PreciseTime.Time;
            if (this.IsTimeToClean())
            {
                this.Clean();
            }
        }

        public bool TryGetEntity(long entityId, out EntityInternal entity)
        {
            EntityEntry entry;
            entity = null;
            if (!this.registry.TryGetValue(entityId, out entry))
            {
                return false;
            }
            entity = entry.entity;
            return true;
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }

        private class EntityEntry
        {
            public EntityInternal entity;
            public SharedEntityRegistryImpl.EntityState state;
            public double unsharedTime;

            public EntityEntry(SharedEntityRegistryImpl.EntityState state, EntityInternal entity)
            {
                this.entity = entity;
                this.state = state;
                this.unsharedTime = 0.0;
            }

            public bool IsEntryExpired() => 
                (this.state == SharedEntityRegistryImpl.EntityState.Unshared) && ((PreciseTime.Time - this.unsharedTime) > SharedEntityRegistryImpl.UNSHARE_EXPIRE_PERIOD_SEC);
        }

        private enum EntityState
        {
            Created,
            Shared,
            Unshared
        }
    }
}

