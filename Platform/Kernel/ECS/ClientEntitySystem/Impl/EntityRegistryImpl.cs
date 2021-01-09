namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class EntityRegistryImpl : EntityRegistry
    {
        private readonly IDictionary<long, Entity> entities = new Dictionary<long, Entity>();

        public bool ContainsEntity(long id) => 
            this.entities.ContainsKey(id);

        public ICollection<Entity> GetAllEntities() => 
            this.entities.Values;

        public Entity GetEntity(long id)
        {
            Entity entity;
            try
            {
                entity = this.entities[id];
            }
            catch (KeyNotFoundException)
            {
                throw new EntityByIdNotFoundException(id);
            }
            return entity;
        }

        public Entity GetEntityForTest(string name)
        {
            Entity entity2;
            using (IEnumerator<Entity> enumerator = this.entities.Values.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        Entity current = enumerator.Current;
                        if (!name.Equals(current.Name))
                        {
                            continue;
                        }
                        entity2 = current;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return entity2;
        }

        public void RegisterEntity(Entity entity)
        {
            try
            {
                this.entities.Add(entity.Id, entity);
            }
            catch (ArgumentException)
            {
                throw new EntityAlreadyRegisteredException(entity);
            }
        }

        public void Remove(long id)
        {
            if (!this.entities.Remove(id))
            {
                throw new EntityByIdNotFoundException(id);
            }
        }
    }
}

