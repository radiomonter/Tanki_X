namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EntityStateMachineImpl : EntityStateMachine
    {
        private Entity entity;
        private readonly IDictionary<Type, EntityState> entityStates = new Dictionary<Type, EntityState>();
        private readonly HashSet<Type> removedComponents = new HashSet<Type>();
        public EntityState currentState;

        public void AddState<T>() where T: Node, new()
        {
            Type key = typeof(T);
            if (this.entityStates.ContainsKey(key))
            {
                throw new EntityStateAlreadyRegisteredException(key);
            }
            EntityState state = new EntityState(key, NodeDescriptionRegistry.GetOrCreateNodeClassDescription(key, null).NodeDescription);
            if (this.entity != null)
            {
                state.Entity = this.entity;
            }
            this.entityStates[key] = state;
        }

        public void AttachToEntity(Entity entity)
        {
            this.entity = entity;
            foreach (EntityState state in this.entityStates.Values)
            {
                state.Entity = entity;
            }
        }

        public T ChangeState<T>() where T: Node
        {
            Type stateType = typeof(T);
            return (T) this.ChangeState(stateType);
        }

        public Node ChangeState(Type stateType)
        {
            if (!this.entityStates.ContainsKey(stateType))
            {
                throw new EntityStateNotRegisteredException(stateType);
            }
            EntityState objB = this.entityStates[stateType];
            Node nextState = objB.Node;
            if (!ReferenceEquals(this.currentState, objB))
            {
                this.ClearComponents(nextState);
                this.EnterState(nextState);
                this.currentState = objB;
            }
            return nextState;
        }

        private void ClearComponents(Node nextState)
        {
            ICollection<Type> components = this.entityStates[nextState.GetType()].Components;
            foreach (EntityState state in this.entityStates.Values)
            {
                ICollection<Type> is3 = state.Components;
                foreach (Type type in is3)
                {
                    if (this.entity.HasComponent(type) && (!components.Contains(type) && !this.removedComponents.Contains(type)))
                    {
                        this.entity.RemoveComponent(type);
                        this.removedComponents.Add(type);
                    }
                }
            }
            this.removedComponents.Clear();
        }

        private void EnterState(Node nextState)
        {
            EntityState state = this.entityStates[nextState.GetType()];
            foreach (Type type in state.Components)
            {
                if (this.entity.HasComponent(type))
                {
                    state.AssignValue(type, ((EntityInternal) this.entity).GetComponent(type));
                    continue;
                }
                Component component = this.entity.CreateNewComponentInstance(type);
                state.AssignValue(type, component);
                this.entity.AddComponent(component);
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }
    }
}

