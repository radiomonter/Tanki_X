namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class EntityImpl : EntityInternal, EntityUnsafe, IComparable<EntityImpl>, Entity
    {
        public static readonly List<Entity> EMPTY_LIST = new List<Entity>();
        private static readonly Type[] EmptyTypes = new Type[0];
        protected readonly EngineServiceInternal engineService;
        protected readonly Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescriptionStorage nodeDescriptionStorage;
        protected readonly EntityComponentStorage storage;
        private readonly NodeProvider nodeProvider;
        private readonly int hashCode;
        private readonly long id;
        private ICollection<EntityListener> entityListeners;
        protected NodeChangedEventMaker nodeAddedEventMaker;
        protected NodeChangedEventMaker nodeRemoveEventMaker;
        private NodeCache nodeCache;
        [CompilerGenerated]
        private static Action<Handler> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<Type, string> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache2;

        public EntityImpl(EngineServiceInternal engineService, long id, string name) : this(engineService, id, name, Optional<Platform.Kernel.ECS.ClientEntitySystem.Impl.TemplateAccessor>.empty())
        {
        }

        public EntityImpl(EngineServiceInternal engineService, long id, string name, Optional<Platform.Kernel.ECS.ClientEntitySystem.Impl.TemplateAccessor> templateAccessor)
        {
            this.engineService = engineService;
            this.id = id;
            this.Name = name;
            this.nodeCache = engineService.NodeCache;
            this.TemplateAccessor = templateAccessor;
            this.storage = new EntityComponentStorage(this, engineService.ComponentBitIdRegistry);
            this.nodeProvider = new NodeProvider(this);
            this.nodeDescriptionStorage = new Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescriptionStorage();
            this.hashCode = this.calcHashCode();
            this.nodeAddedEventMaker = new NodeChangedEventMaker(NodeAddedEvent.Instance, typeof(NodeAddedFireHandler), typeof(NodeAddedCompleteHandler), engineService.HandlerCollector);
            this.nodeRemoveEventMaker = new NodeChangedEventMaker(NodeRemoveEvent.Instance, typeof(NodeRemovedFireHandler), typeof(NodeRemovedCompleteHandler), engineService.HandlerCollector);
            this.Init();
            this.UpdateNodes(NodeDescriptionRegistry.GetNodeDescriptionsWithNotComponentsOnly());
        }

        public void AddComponent<T>() where T: Component, new()
        {
            this.AddComponent(typeof(T));
        }

        public void AddComponent(Component component)
        {
            if (component is GroupComponent)
            {
                component = GroupRegistry.FindOrRegisterGroup((GroupComponent) component);
            }
            this.AddComponent(component, true);
        }

        public void AddComponent(Type componentType)
        {
            Component component = this.CreateNewComponentInstance(componentType);
            this.AddComponent(component);
        }

        private void AddComponent(Component component, bool sendEvent)
        {
            Type componentClass = component.GetType();
            if (!this.storage.HasComponent(componentClass) || !this.IsSkipExceptionOnAddRemove(componentClass))
            {
                this.UpdateHandlers(component.GetType());
                this.NotifyAttachToEntity(component);
                this.storage.AddComponentImmediately(component.GetType(), component);
                this.MakeNodes(component.GetType(), component);
                if (sendEvent)
                {
                    this.NotifyAddComponent(component);
                }
                this.PrepareAndSendNodeAddedEvent(component);
            }
        }

        public T AddComponentAndGetInstance<T>()
        {
            Component component = this.CreateNewComponentInstance(typeof(T));
            this.AddComponent(component);
            return (T) component;
        }

        public void AddComponentIfAbsent<T>() where T: Component, new()
        {
            if (!this.HasComponent<T>())
            {
                this.AddComponent(typeof(T));
            }
        }

        public void AddComponentSilent(Component component)
        {
            this.AddComponent(component, false);
        }

        public void AddEntityListener(EntityListener entityListener)
        {
            this.entityListeners.Add(entityListener);
        }

        protected internal void AddNode(NodeDescription node)
        {
            Flow.Current.NodeCollector.Attach(this, node);
            this.nodeDescriptionStorage.AddNode(node);
            this.SendNodeAddedForCollectors(node);
        }

        private int calcHashCode() => 
            this.Id.GetHashCode();

        public bool CanCast(NodeDescription desc) => 
            this.nodeProvider.CanCast(desc);

        public void ChangeComponent(Component component)
        {
            bool flag = this.HasComponent(component.GetType()) && this.GetComponent(component.GetType()).Equals(component);
            this.storage.ChangeComponent(component);
            if (!flag)
            {
                this.nodeProvider.OnComponentChanged(component);
            }
            this.NotifyChangedInEntity(component);
        }

        private void ClearNodes()
        {
            new List<NodeDescription>(this.nodeDescriptionStorage.GetNodeDescriptions()).ForEach(new Action<NodeDescription>(this.RemoveNode));
            this.nodeProvider.CleanNodes();
        }

        public int CompareTo(EntityImpl other) => 
            (int) (this.id - other.id);

        public bool Contains(NodeDescription node)
        {
            BitSet componentsBitId = this.ComponentsBitId;
            return (componentsBitId.Mask(node.NodeComponentBitId) && componentsBitId.MaskNot(node.NotNodeComponentBitId));
        }

        public T CreateGroup<T>() where T: GroupComponent
        {
            T component = GroupRegistry.FindOrCreateGroup<T>(this.Id);
            this.AddComponent(component);
            return component;
        }

        public Component CreateNewComponentInstance(Type componentType)
        {
            Collections.Enumerator<ComponentConstructor> enumerator = Collections.GetEnumerator<ComponentConstructor>(this.engineService.ComponentConstructors);
            while (enumerator.MoveNext())
            {
                ComponentConstructor current = enumerator.Current;
                if (current.IsAcceptable(componentType, this))
                {
                    return current.GetComponentInstance(componentType, this);
                }
            }
            return (Component) componentType.GetConstructor(EmptyTypes).Invoke(Collections.EmptyArray);
        }

        protected bool Equals(EntityImpl other) => 
            this.id == other.id;

        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                obj = ((Node) obj).Entity;
            }
            return (!ReferenceEquals(null, obj) ? (!ReferenceEquals(this, obj) ? (ReferenceEquals(obj.GetType(), base.GetType()) ? this.Equals((EntityImpl) obj) : false) : true) : false);
        }

        public T GetComponent<T>() where T: Component => 
            (T) this.GetComponent(typeof(T));

        public Component GetComponent(Type componentType) => 
            this.storage.GetComponent(componentType);

        public Component GetComponentUnsafe(Type componentType) => 
            this.storage.GetComponentUnsafe(componentType);

        public override int GetHashCode() => 
            this.hashCode;

        public Node GetNode(NodeClassInstanceDescription instanceDescription) => 
            this.nodeProvider.GetNode(instanceDescription);

        public bool HasComponent<T>() where T: Component => 
            this.HasComponent(typeof(T));

        public bool HasComponent(Type type) => 
            this.storage.HasComponent(type);

        public void Init()
        {
            this.Alive = true;
            List<EntityListener> list = new List<EntityListener> {
                this.engineService.HandlerContextDataStorage,
                this.engineService.HandlerStateListener,
                this.engineService.BroadcastEventHandlerCollector
            };
            this.entityListeners = list;
        }

        public bool IsSameGroup<T>(Entity otherEntity) where T: GroupComponent => 
            (this.HasComponent<T>() && otherEntity.HasComponent<T>()) && this.GetComponent<T>().Key.Equals(otherEntity.GetComponent<T>().Key);

        private bool IsSkipExceptionOnAddRemove(Type componentType) => 
            componentType.IsDefined(typeof(SkipExceptionOnAddRemoveAttribute), true);

        protected void MakeNodes(Type componentType, Component component)
        {
            this.nodeProvider.OnComponentAdded(component, componentType);
            NodesToChange nodesToChange = this.nodeCache.GetNodesToChange(this, componentType);
            foreach (NodeDescription description in nodesToChange.NodesToAdd)
            {
                this.AddNode(description);
            }
            foreach (NodeDescription description2 in nodesToChange.NodesToRemove)
            {
                this.RemoveNode(description2);
            }
        }

        private void NotifyAddComponent(Component component)
        {
            Collections.Enumerator<ComponentListener> enumerator = Collections.GetEnumerator<ComponentListener>(this.engineService.ComponentListeners);
            while (enumerator.MoveNext())
            {
                enumerator.Current.OnComponentAdded(this, component);
            }
        }

        private void NotifyAttachToEntity(Component component)
        {
            AttachToEntityListener listener = component as AttachToEntityListener;
            if (listener != null)
            {
                listener.AttachedToEntity(this);
            }
        }

        private void NotifyChangedInEntity(Component component)
        {
            ComponentServerChangeListener listener = component as ComponentServerChangeListener;
            if (listener != null)
            {
                listener.ChangedOnServer(this);
            }
        }

        public void NotifyComponentChange(Type componentType)
        {
            Component component = this.GetComponent(componentType);
            Collections.Enumerator<ComponentListener> enumerator = Collections.GetEnumerator<ComponentListener>(this.engineService.ComponentListeners);
            while (enumerator.MoveNext())
            {
                enumerator.Current.OnComponentChanged(this, component);
            }
        }

        private void NotifyComponentRemove(Type componentType)
        {
            Component component = this.storage.GetComponent(componentType);
            Collections.Enumerator<ComponentListener> enumerator = Collections.GetEnumerator<ComponentListener>(this.engineService.ComponentListeners);
            while (enumerator.MoveNext())
            {
                enumerator.Current.OnComponentRemoved(this, component);
            }
        }

        private void NotifyDetachFromEntity(Component component)
        {
            DetachFromEntityListener listener = component as DetachFromEntityListener;
            if (listener != null)
            {
                listener.DetachedFromEntity(this);
            }
        }

        public void OnDelete()
        {
            this.Alive = false;
            this.ClearNodes();
            this.SendEntityDeletedForAllListeners();
            this.storage.OnEntityDelete();
        }

        private void PrepareAndSendNodeAddedEvent(Component component)
        {
            this.nodeAddedEventMaker.MakeIfNeed(this, component.GetType());
        }

        public void RemoveComponent<T>() where T: Component
        {
            this.RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type componentType)
        {
            this.UpdateHandlers(componentType);
            this.NotifyComponentRemove(componentType);
            this.RemoveComponentSilent(componentType);
        }

        public void RemoveComponentIfPresent<T>() where T: Component
        {
            if (this.HasComponent<T>())
            {
                this.RemoveComponent(typeof(T));
            }
        }

        public void RemoveComponentSilent(Type componentType)
        {
            if (this.HasComponent(componentType) || (!this.HasComponent<DeletedEntityComponent>() && !this.IsSkipExceptionOnAddRemove(componentType)))
            {
                this.SendNodeRemoved(componentType);
                Component component = this.storage.RemoveComponentImmediately(componentType);
                this.NotifyDetachFromEntity(component);
                this.UpdateNodesOnComponentRemoved(componentType);
            }
        }

        public virtual void RemoveEntityListener(EntityListener entityListener)
        {
            this.entityListeners.Remove(entityListener);
        }

        internal void RemoveNode(NodeDescription node)
        {
            this.SendNodeRemovedForCollectors(node);
            Flow.Current.NodeCollector.Detach(this, node);
            this.nodeDescriptionStorage.RemoveNode(node);
        }

        public void ScheduleEvent<T>() where T: Event, new()
        {
            EngineService.Engine.ScheduleEvent<T>(this);
        }

        public void ScheduleEvent(Event eventInstance)
        {
            EngineService.Engine.ScheduleEvent(eventInstance, this);
        }

        private void SendEntityDeletedForAllListeners()
        {
            Collections.Enumerator<EntityListener> enumerator = Collections.GetEnumerator<EntityListener>(this.entityListeners);
            while (enumerator.MoveNext())
            {
                enumerator.Current.OnEntityDeleted(this);
            }
            this.entityListeners.Clear();
        }

        public T SendEvent<T>(T eventInstance) where T: Event
        {
            EngineService.Engine.ScheduleEvent(eventInstance, this);
            return eventInstance;
        }

        private void SendNodeAddedForCollectors(NodeDescription nodeDescription)
        {
            Collections.Enumerator<EntityListener> enumerator = Collections.GetEnumerator<EntityListener>(this.entityListeners);
            while (enumerator.MoveNext())
            {
                enumerator.Current.OnNodeAdded(this, nodeDescription);
            }
        }

        private void SendNodeRemoved(Type componentType)
        {
            this.nodeRemoveEventMaker.MakeIfNeed(this, componentType);
        }

        private void SendNodeRemovedForCollectors(NodeDescription nodeDescription)
        {
            Collections.Enumerator<EntityListener> enumerator = Collections.GetEnumerator<EntityListener>(this.entityListeners);
            while (enumerator.MoveNext())
            {
                enumerator.Current.OnNodeRemoved(this, nodeDescription);
            }
        }

        public T ToNode<T>() where T: Node, new()
        {
            T local = Activator.CreateInstance<T>();
            local.Entity = this;
            return local;
        }

        public override string ToString() => 
            $"{this.Id}({this.Name})";

        public string ToStringWithComponentsClasses()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = c => c.Name;
            }
            <>f__am$cache2 ??= c => c;
            string[] strArray = this.ComponentClasses.Select<Type, string>(<>f__am$cache1).OrderBy<string, string>(<>f__am$cache2).ToArray<string>();
            return $"Entity[id={this.Id}, name={this.Name}, components={string.Join(",", strArray)}]";
        }

        private void UpdateHandlers(Type componentType)
        {
            if (componentType.IsSubclassOf(typeof(GroupComponent)))
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = h => h.ChangeVersion();
                }
                this.engineService.HandlerCollector.GetHandlersByGroupComponent(componentType).ForEach<Handler>(<>f__am$cache0);
            }
        }

        private void UpdateNodes(ICollection<NodeDescription> nodes)
        {
            BitSet componentsBitId = this.ComponentsBitId;
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator<NodeDescription>(nodes);
            while (enumerator.MoveNext())
            {
                NodeDescription current = enumerator.Current;
                if (componentsBitId.Mask(current.NodeComponentBitId))
                {
                    if (componentsBitId.MaskNot(current.NotNodeComponentBitId))
                    {
                        this.AddNode(current);
                        continue;
                    }
                    if (this.nodeDescriptionStorage.Contains(current))
                    {
                        this.RemoveNode(current);
                    }
                }
            }
        }

        private void UpdateNodesOnComponentRemoved(Type componentClass)
        {
            new List<NodeDescription>(this.nodeDescriptionStorage.GetNodeDescriptions(componentClass)).ForEach(new Action<NodeDescription>(this.RemoveNode));
            this.UpdateNodes(NodeDescriptionRegistry.GetNodeDescriptionsByNotComponent(componentClass));
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.Impl.GroupRegistry GroupRegistry { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.FlowInstancesCache FlowInstancesCache { get; set; }

        public long Id =>
            this.id;

        public string Name { get; set; }

        public string ConfigPath =>
            this.TemplateAccessor.Get().ConfigPath;

        public Optional<Platform.Kernel.ECS.ClientEntitySystem.Impl.TemplateAccessor> TemplateAccessor { get; set; }

        public bool Alive { get; private set; }

        public ICollection<Component> Components =>
            this.storage.Components;

        public ICollection<Type> ComponentClasses =>
            this.storage.ComponentClasses;

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescriptionStorage NodeDescriptionStorage =>
            this.nodeDescriptionStorage;

        public BitSet ComponentsBitId =>
            this.storage.bitId;
    }
}

