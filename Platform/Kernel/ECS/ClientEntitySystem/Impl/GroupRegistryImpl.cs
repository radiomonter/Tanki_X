namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class GroupRegistryImpl : GroupRegistry
    {
        private IDictionary<Type, IDictionary<long, GroupComponent>> groups = new Dictionary<Type, IDictionary<long, GroupComponent>>();

        private GroupComponent CreateGroupComponent(Type groupClass, long key)
        {
            Type[] types = new Type[] { typeof(long) };
            ConstructorInfo constructor = groupClass.GetConstructor(types);
            if (constructor == null)
            {
                throw new ComponentInstantiatingException(groupClass);
            }
            object[] parameters = new object[] { key };
            return (GroupComponent) constructor.Invoke(parameters);
        }

        public T FindOrCreateGroup<T>(long key) where T: GroupComponent => 
            (T) this.FindOrCreateGroup(typeof(T), key);

        public GroupComponent FindOrCreateGroup(Type groupClass, long key)
        {
            if (!this.groups.ContainsKey(groupClass))
            {
                this.groups[groupClass] = new Dictionary<long, GroupComponent>();
            }
            IDictionary<long, GroupComponent> dictionary = this.groups[groupClass];
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = this.CreateGroupComponent(groupClass, key);
            }
            return dictionary[key];
        }

        public GroupComponent FindOrRegisterGroup(GroupComponent groupComponent)
        {
            Type key = groupComponent.GetType();
            long num = groupComponent.Key;
            if (!this.groups.ContainsKey(key))
            {
                this.groups[key] = new Dictionary<long, GroupComponent>();
            }
            IDictionary<long, GroupComponent> dictionary = this.groups[key];
            if (!dictionary.ContainsKey(num))
            {
                dictionary[num] = groupComponent;
            }
            return dictionary[num];
        }

        public T FindOrRegisterGroup<T>(T groupComponent) where T: GroupComponent => 
            (T) this.FindOrRegisterGroup(groupComponent);
    }
}

