namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class AutoRemoveComponentsSystem : ECSSystem
    {
        private static AutoRemoveComponentsRegistry registry;
        [CompilerGenerated]
        private static Func<Type, bool> <>f__am$cache0;

        public AutoRemoveComponentsSystem(AutoRemoveComponentsRegistry autoRemoveComponentsRegistry)
        {
            registry = autoRemoveComponentsRegistry;
        }

        [OnEventComplete]
        public void AutoRemoveComponentsIfNeed(NodeAddedEvent e, SingleNode<DeletedEntityComponent> node)
        {
            List<Type> componentsToRemove = GetComponentsToRemove(node);
            if (componentsToRemove.Count > 0)
            {
                base.ScheduleEvent(new AutoRemoveComponentsEvent(componentsToRemove), node);
            }
        }

        private static List<Type> GetComponentsToRemove(Node node)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = componentType => registry.IsComponentAutoRemoved(componentType) && !ReferenceEquals(componentType, typeof(DeletedEntityComponent));
            }
            return ((EntityInternal) node.Entity).ComponentClasses.Where<Type>(<>f__am$cache0).ToList<Type>();
        }

        [OnEventFire]
        public void RemoveComponents(AutoRemoveComponentsEvent e, Node node)
        {
            List<Type> componentsToRemove = e.ComponentsToRemove;
            componentsToRemove.Sort(new ComponentRemoveOrderComparer());
            foreach (Type type in componentsToRemove)
            {
                if (node.Entity.HasComponent(type))
                {
                    node.Entity.RemoveComponent(type);
                }
            }
        }

        [OnEventComplete]
        public void RepeatRemoveComponents(AutoRemoveComponentsEvent e, Node node)
        {
            List<Type> componentsToRemove = GetComponentsToRemove(node);
            if (componentsToRemove.Count > 0)
            {
                base.ScheduleEvent(new AutoRemoveComponentsEvent(componentsToRemove), node);
            }
        }
    }
}

