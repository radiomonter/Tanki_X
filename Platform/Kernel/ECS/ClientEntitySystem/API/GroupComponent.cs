namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [SkipAutoRemove]
    public class GroupComponent : Component, AttachToEntityListener, DetachFromEntityListener, EntityListener
    {
        private readonly NodeCollector nodeCollector;
        private HashSet<Entity> members;

        public GroupComponent(Entity keyEntity) : this(keyEntity.Id)
        {
        }

        public GroupComponent(long key)
        {
            this.Key = key;
            this.nodeCollector = new GroupNodeCollectorImpl();
            this.members = new HashSet<Entity>();
        }

        public GroupComponent Attach(Entity entity)
        {
            entity.AddComponent(this);
            return this;
        }

        public void AttachedToEntity(Entity entity)
        {
            EntityInternal item = (EntityInternal) entity;
            this.members.Add(item);
            item.AddEntityListener(this);
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator<NodeDescription>(item.NodeDescriptionStorage.GetNodeDescriptions());
            while (enumerator.MoveNext())
            {
                this.nodeCollector.Attach(item, enumerator.Current);
            }
        }

        public void Detach(Entity entity)
        {
            entity.RemoveComponent(base.GetType());
        }

        public void DetachedFromEntity(Entity entity)
        {
            EntityInternal member = (EntityInternal) entity;
            this.OnRemoveMemberWithoutRemovingListener(member);
            member.RemoveEntityListener(this);
        }

        public ICollection<Entity> GetGroupMembers() => 
            this.members;

        public ICollection<Entity> GetGroupMembers(NodeDescription nodeDescription) => 
            this.nodeCollector.GetEntities(nodeDescription) ?? Collections.EmptyList<Entity>();

        public void OnEntityDeleted(Entity entity)
        {
            this.OnRemoveMemberWithoutRemovingListener((EntityInternal) entity);
        }

        public void OnNodeAdded(Entity entity, NodeDescription nodeDescription)
        {
            this.nodeCollector.Attach(entity, nodeDescription);
        }

        public void OnNodeRemoved(Entity entity, NodeDescription nodeDescription)
        {
            this.nodeCollector.Detach(entity, nodeDescription);
        }

        private void OnRemoveMemberWithoutRemovingListener(EntityInternal member)
        {
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator<NodeDescription>(member.NodeDescriptionStorage.GetNodeDescriptions());
            while (enumerator.MoveNext())
            {
                this.nodeCollector.Detach(member, enumerator.Current);
            }
            this.members.Remove(member);
        }

        public override string ToString()
        {
            object[] objArray1 = new object[] { base.GetType().Name, "[key=", this.Key, ']' };
            return string.Concat(objArray1);
        }

        public long Key { get; private set; }
    }
}

