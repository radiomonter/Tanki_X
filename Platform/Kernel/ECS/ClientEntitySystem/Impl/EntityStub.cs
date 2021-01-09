namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EntityStub : EntityInternal, Entity
    {
        private Node node = new Node();

        public EntityStub()
        {
            this.node.Entity = this;
        }

        public void AddComponent<T>() where T: Component, new()
        {
            throw new NotSupportedException("ComponentType: " + typeof(T));
        }

        public void AddComponent(Component component)
        {
            throw new NotSupportedException("ComponentType: " + component.GetType());
        }

        public void AddComponent(Type componentType)
        {
            throw new NotSupportedException("ComponentType: " + componentType);
        }

        public T AddComponentAndGetInstance<T>()
        {
            throw new NotSupportedException("ComponentType: " + typeof(T));
        }

        public void AddComponentIfAbsent<T>() where T: Component, new()
        {
            throw new NotSupportedException("ComponentType: " + typeof(T));
        }

        public void AddComponentSilent(Component component)
        {
            throw new NotSupportedException();
        }

        public void AddEntityListener(EntityListener entityListener)
        {
            throw new NotSupportedException();
        }

        public bool CanCast(NodeDescription desc) => 
            desc.IsEmpty;

        public void ChangeComponent(Component component)
        {
            throw new NotSupportedException("ComponentType: " + component.GetType());
        }

        public bool Contains(NodeDescription node) => 
            node.IsEmpty;

        public T CreateGroup<T>() where T: GroupComponent
        {
            throw new NotSupportedException("ComponentType: " + typeof(T));
        }

        public Component CreateNewComponentInstance(Type componentType)
        {
            throw new NotSupportedException("ComponentType: " + componentType);
        }

        public T GetComponent<T>() where T: Component
        {
            throw new NotSupportedException("ComponentType: " + typeof(T));
        }

        public Component GetComponent(Type componentType)
        {
            throw new NotSupportedException("ComponentType: " + componentType);
        }

        public Node GetNode(NodeClassInstanceDescription instanceDescription)
        {
            if (!instanceDescription.NodeDescription.IsEmpty)
            {
                throw new NotSupportedException();
            }
            return this.node;
        }

        public bool HasComponent<T>() where T: Component => 
            false;

        public bool HasComponent(Type type) => 
            false;

        public void Init()
        {
            throw new NotSupportedException();
        }

        public bool IsSameGroup<T>(Entity otherEntity) where T: GroupComponent
        {
            throw new NotImplementedException();
        }

        public void NotifyComponentChange(Type componentType)
        {
            throw new NotSupportedException();
        }

        public void OnDelete()
        {
            throw new NotSupportedException();
        }

        public void RemoveComponent<T>() where T: Component
        {
            throw new NotSupportedException("ComponentType: " + typeof(T));
        }

        public void RemoveComponent(Type componentType)
        {
            throw new NotSupportedException("ComponentType: " + componentType);
        }

        public void RemoveComponentIfPresent<T>() where T: Component
        {
            throw new NotSupportedException("ComponentType: " + typeof(T));
        }

        public void RemoveComponentSilent(Type componentType)
        {
            throw new NotSupportedException();
        }

        public void RemoveEntityListener(EntityListener entityListener)
        {
            throw new NotSupportedException();
        }

        public void ScheduleEvent<T>() where T: Event, new()
        {
            throw new NotImplementedException();
        }

        public void ScheduleEvent(Event eventInstance)
        {
            throw new NotImplementedException();
        }

        public T SendEvent<T>(T eventInstance) where T: Event
        {
            EngineService.Engine.ScheduleEvent(eventInstance, this);
            return eventInstance;
        }

        public T ToNode<T>() where T: Node, new()
        {
            if (!ReferenceEquals(typeof(T), typeof(Node)))
            {
                throw new NotSupportedException();
            }
            return (T) this.node;
        }

        public string ToStringWithComponentsClasses()
        {
            throw new NotSupportedException();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public long Id =>
            -1L;

        public string Name
        {
            get => 
                "Stub";
            set
            {
                throw new NotSupportedException();
            }
        }

        public string ConfigPath =>
            string.Empty;

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeDescriptionStorage NodeDescriptionStorage
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public BitSet ComponentsBitId
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public ICollection<Type> ComponentClasses
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public ICollection<Component> Components
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public Optional<Platform.Kernel.ECS.ClientEntitySystem.Impl.TemplateAccessor> TemplateAccessor
        {
            get => 
                Optional<Platform.Kernel.ECS.ClientEntitySystem.Impl.TemplateAccessor>.empty();
            set
            {
                throw new NotSupportedException();
            }
        }

        public bool Alive =>
            true;
    }
}

