namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EntityComponentStorage
    {
        private readonly EntityInternal entity;
        private ComponentBitIdRegistry componentBitIdRegistry;
        private readonly IDictionary<Type, Component> components = new Dictionary<Type, Component>();

        public EntityComponentStorage(EntityInternal entity, ComponentBitIdRegistry componentBitIdRegistry)
        {
            this.entity = entity;
            this.componentBitIdRegistry = componentBitIdRegistry;
            this.bitId = new BitSet();
        }

        public void AddComponentImmediately(Type comType, Component component)
        {
            try
            {
                this.components.Add(comType, component);
                this.bitId.Set(this.componentBitIdRegistry.GetComponentBitId(comType));
            }
            catch (ArgumentException)
            {
                throw new ComponentAlreadyExistsInEntityException(this.entity, comType);
            }
        }

        public void AddComponentsImmediately(IList<Component> addedComponents)
        {
            addedComponents.ForEach<Component>(component => this.AddComponentImmediately(component.GetType(), component));
        }

        private void AssertComponentFound(Type componentClass)
        {
            if (!this.components.ContainsKey(componentClass))
            {
                throw new ComponentNotFoundException(this.entity, componentClass);
            }
        }

        public void ChangeComponent(Component component)
        {
            Type componentClass = component.GetType();
            this.AssertComponentFound(componentClass);
            this.components[componentClass] = component;
        }

        public Component GetComponent(Type componentClass)
        {
            Component component;
            try
            {
                component = this.components[componentClass];
            }
            catch (KeyNotFoundException)
            {
                throw new ComponentNotFoundException(this.entity, componentClass);
            }
            return component;
        }

        public Component GetComponentUnsafe(Type componentType)
        {
            Component component;
            return (!this.components.TryGetValue(componentType, out component) ? null : component);
        }

        public bool HasComponent(Type componentClass) => 
            this.components.ContainsKey(componentClass);

        public void OnEntityDelete()
        {
            this.components.Clear();
            this.bitId.Clear();
        }

        public Component RemoveComponentImmediately(Type componentClass)
        {
            Component component2;
            try
            {
                Component component = this.components[componentClass];
                this.components.Remove(componentClass);
                this.bitId.Clear(this.componentBitIdRegistry.GetComponentBitId(componentClass));
                component2 = component;
            }
            catch (KeyNotFoundException)
            {
                throw new ComponentNotFoundException(this.entity, componentClass);
            }
            return component2;
        }

        public BitSet bitId { get; private set; }

        public ICollection<Type> ComponentClasses =>
            this.components.Keys;

        public ICollection<Component> Components =>
            this.components.Values;
    }
}

