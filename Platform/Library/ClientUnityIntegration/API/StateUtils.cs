namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public static class StateUtils
    {
        private static void RemoveNonStateComponents(Entity entity, Type targetState, HashSet<Type> states)
        {
            HashSet<Type>.Enumerator enumerator = states.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Type current = enumerator.Current;
                if (!ReferenceEquals(current, targetState) && entity.HasComponent(current))
                {
                    entity.RemoveComponent(current);
                }
            }
        }

        public static void SwitchEntityState<T>(Entity entity, HashSet<Type> states) where T: Component
        {
            SwitchEntityState(entity, typeof(T), states);
        }

        public static void SwitchEntityState(Entity entity, Component component, HashSet<Type> states)
        {
            Type targetState = component.GetType();
            RemoveNonStateComponents(entity, targetState, states);
            if (!entity.HasComponent(targetState))
            {
                entity.AddComponent(component);
            }
        }

        public static void SwitchEntityState(Entity entity, Type targetState, HashSet<Type> states)
        {
            RemoveNonStateComponents(entity, targetState, states);
            if (!entity.HasComponent(targetState))
            {
                entity.AddComponent(entity.CreateNewComponentInstance(targetState));
            }
        }
    }
}

