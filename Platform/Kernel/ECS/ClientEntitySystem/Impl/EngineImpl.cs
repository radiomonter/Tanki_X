namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class EngineImpl : Engine
    {
        private DelayedEventManager delayedEventManager;
        private EntityCloner entityCloner;
        private TemplateRegistry templateRegistry;
        private readonly Entity _fakeEntity = new EntityStub();

        public Entity CloneEntity(string name, Entity entity) => 
            this.entityCloner.Clone(name, (EntityInternal) entity, EngineService.CreateEntityBuilder());

        private IList ConvertNodeCollection(NodeClassInstanceDescription nodeClassInstanceDescription, ICollection<Entity> entities)
        {
            int count = entities.Count;
            IList genericListInstance = Cache.GetGenericListInstance(nodeClassInstanceDescription.NodeClass, count);
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(entities);
            while (enumerator.MoveNext())
            {
                Node node = this.GetNode(enumerator.Current, nodeClassInstanceDescription);
                genericListInstance.Add(node);
            }
            return genericListInstance;
        }

        public ICollection<Entity> CreateEntities<T>(string configPathWithWildcard) where T: Template
        {
            List<Entity> list = new List<Entity>();
            foreach (string str in ConfigurationService.GetPathsByWildcard(configPathWithWildcard))
            {
                Entity item = EngineService.CreateEntityBuilder().SetTemplate(typeof(T)).SetConfig(str).Build(true);
                list.Add(item);
            }
            return list;
        }

        public Entity CreateEntity<T>() where T: Template => 
            EngineService.CreateEntityBuilder().SetTemplate(typeof(T)).Build(true);

        public Entity CreateEntity<T>(YamlNode yamlNode) where T: Template => 
            EngineService.CreateEntityBuilder().SetTemplate(typeof(T)).SetTemplateYamlNode(yamlNode).Build(true);

        public Entity CreateEntity(string name) => 
            EngineService.CreateEntityBuilder().SetName(name).Build(true);

        public Entity CreateEntity<T>(string configPath) where T: Template => 
            EngineService.CreateEntityBuilder().SetConfig(configPath).SetTemplate(typeof(T)).Build(true);

        public Entity CreateEntity(long templateId, string configPath) => 
            EngineService.CreateEntityBuilder().SetTemplate(this.templateRegistry.GetTemplateInfo(templateId)).SetConfig(configPath).Build(true);

        public Entity CreateEntity<T>(string configPath, long id) where T: Template => 
            EngineService.CreateEntityBuilder().SetId(id).SetConfig(configPath).SetTemplate(typeof(T)).Build(true);

        public Entity CreateEntity(Type templateType, string configPath) => 
            EngineService.CreateEntityBuilder().SetConfig(configPath).SetTemplate(templateType).Build(true);

        public Entity CreateEntity(long templateId, string configPath, long id) => 
            EngineService.CreateEntityBuilder().SetId(id).SetTemplate(this.templateRegistry.GetTemplateInfo(templateId)).SetConfig(configPath).Build(true);

        private T CreateOrReuseEventInstance<T>() where T: Event, new()
        {
            Event event2;
            return (!EngineService.EventInstancesStorageForReuse.TryGetInstance(typeof(T), out event2) ? Activator.CreateInstance<T>() : ((T) event2));
        }

        public void DeleteEntity(Entity entity)
        {
            entity.AddComponent<DeletedEntityComponent>();
            this.ScheduleEvent<DeleteEntityEvent>(entity);
            Flow.Current.EntityRegistry.Remove(entity.Id);
            ((EntityInternal) entity).OnDelete();
        }

        private IList<N> DoSelect<N>(Entity entity, Type groupComponentType) where N: Node
        {
            ICollection<Entity> is2;
            GroupComponent componentUnsafe = (GroupComponent) ((EntityUnsafe) entity).GetComponentUnsafe(groupComponentType);
            if (componentUnsafe == null)
            {
                return Collections.EmptyList<N>();
            }
            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(N), null);
            NodeDescriptionRegistry.AssertRegister(orCreateNodeClassDescription.NodeDescription);
            return (((is2 = componentUnsafe.GetGroupMembers(orCreateNodeClassDescription.NodeDescription)).Count != 0) ? ((is2.Count != 1) ? ((IList<N>) this.ConvertNodeCollection(orCreateNodeClassDescription, is2)) : Collections.SingletonList<N>((N) this.GetNode(Collections.GetOnlyElement<Entity>(is2), orCreateNodeClassDescription))) : Collections.EmptyList<N>());
        }

        private Node GetNode(Entity entity, NodeClassInstanceDescription nodeClassInstanceDescription) => 
            ((EntityInternal) entity).GetNode(nodeClassInstanceDescription);

        public virtual void Init(TemplateRegistry templateRegistry, DelayedEventManager delayedEventManager)
        {
            this.templateRegistry = templateRegistry;
            this.delayedEventManager = delayedEventManager;
            this.entityCloner = new EntityCloner();
        }

        public EventBuilder NewEvent<T>() where T: Event, new() => 
            this.NewEvent(this.CreateOrReuseEventInstance<T>());

        public EventBuilder NewEvent(Event eventInstance) => 
            Cache.eventBuilder.GetInstance().Init(this.delayedEventManager, Flow.Current, eventInstance);

        public void OnDeleteEntity(Entity entity)
        {
            Flow.Current.EntityRegistry.Remove(entity.Id);
            ((EntityInternal) entity).OnDelete();
        }

        public void ScheduleEvent<T>() where T: Event, new()
        {
            this.ScheduleEvent<T>(this._fakeEntity);
        }

        public void ScheduleEvent<T>(Entity entity) where T: Event, new()
        {
            this.NewEvent(this.CreateOrReuseEventInstance<T>()).Attach(entity).Schedule();
        }

        public void ScheduleEvent(Event eventInstance)
        {
            this.ScheduleEvent(eventInstance, this._fakeEntity);
        }

        public void ScheduleEvent<T>(GroupComponent group) where T: Event, new()
        {
            ICollection<Entity> groupMembers = group.GetGroupMembers();
            this.NewEvent(this.CreateOrReuseEventInstance<T>()).AttachAll(groupMembers).Schedule();
        }

        public void ScheduleEvent<T>(Node node) where T: Event, new()
        {
            this.ScheduleEvent<T>(node.Entity);
        }

        public void ScheduleEvent(Event eventInstance, Entity entity)
        {
            this.NewEvent(eventInstance).Attach(entity).Schedule();
        }

        public void ScheduleEvent(Event eventInstance, GroupComponent group)
        {
            ICollection<Entity> groupMembers = group.GetGroupMembers();
            this.NewEvent(eventInstance).AttachAll(groupMembers).Schedule();
        }

        public void ScheduleEvent(Event eventInstance, Node node)
        {
            this.ScheduleEvent(eventInstance, node.Entity);
        }

        public IList<N> Select<N, G>(Entity entity) where N: Node where G: GroupComponent => 
            this.DoSelect<N>(entity, typeof(G));

        public IList<N> Select<N>(Entity entity, Type groupComponentType) where N: Node
        {
            if (!typeof(GroupComponent).IsAssignableFrom(groupComponentType))
            {
                throw new NotGroupComponentException(groupComponentType);
            }
            return this.DoSelect<N>(entity, groupComponentType);
        }

        public ICollection<N> SelectAll<N>() where N: Node
        {
            <SelectAll>c__AnonStorey0<N> storey = new <SelectAll>c__AnonStorey0<N> {
                nodeDesc = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(N), null)
            };
            return this.SelectAllEntities<N>().Select<Entity, N>(new Func<Entity, N>(storey.<>m__0)).ToList<N>();
        }

        public ICollection<Entity> SelectAllEntities<N>() where N: Node
        {
            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(N), null);
            return Flow.Current.NodeCollector.GetEntities(orCreateNodeClassDescription.NodeDescription);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        [CompilerGenerated]
        private sealed class <SelectAll>c__AnonStorey0<N> where N: Node
        {
            internal NodeClassInstanceDescription nodeDesc;

            internal N <>m__0(Entity e) => 
                (N) ((EntityInternal) e).GetNode(this.nodeDesc);
        }
    }
}

