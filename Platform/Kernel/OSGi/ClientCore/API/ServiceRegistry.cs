namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ServiceRegistry
    {
        private static ServiceRegistry current;
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
        private readonly Dictionary<Type, List<PropertyInfo>> waitingConsumers = new Dictionary<Type, List<PropertyInfo>>();

        static ServiceRegistry()
        {
            Reset();
        }

        private void InjectIntoConsumer(PropertyInfo propertyInfo, object service)
        {
            object[] parameters = new object[] { service };
            propertyInfo.GetSetMethod(true).Invoke(null, parameters);
        }

        private void InjectIntoWaitingConsumers(Type type)
        {
            List<PropertyInfo> list = this.waitingConsumers[type];
            this.waitingConsumers.Remove(type);
            foreach (PropertyInfo info in list)
            {
                this.InjectIntoConsumer(info, this.services[type]);
            }
        }

        public void RegisterConsumer(PropertyInfo consumer)
        {
            if (!consumer.GetSetMethod(true).IsStatic)
            {
                string name = consumer.DeclaringType.Name;
                throw new ArgumentException($"Property {name}::{consumer.Name} has to be static", "consumer");
            }
            Type propertyType = consumer.PropertyType;
            if (this.services.ContainsKey(propertyType))
            {
                this.InjectIntoConsumer(consumer, this.services[propertyType]);
            }
            else
            {
                this.StoreWaitingConsumer(consumer, propertyType);
            }
        }

        public void RegisterService<T>(T service)
        {
            Type key = typeof(T);
            this.services[key] = service;
            if (this.waitingConsumers.ContainsKey(key))
            {
                this.InjectIntoWaitingConsumers(key);
            }
        }

        public static void Reset()
        {
            Current = new ServiceRegistry();
        }

        private void StoreWaitingConsumer(PropertyInfo consumer, Type type)
        {
            List<PropertyInfo> list;
            if (!this.waitingConsumers.TryGetValue(type, out list))
            {
                list = new List<PropertyInfo>();
                this.waitingConsumers.Add(type, list);
            }
            list.Add(consumer);
        }

        public static ServiceRegistry Current
        {
            get
            {
                if (current == null)
                {
                    throw new Exception("Service registry is not set");
                }
                return current;
            }
            set => 
                current = value;
        }
    }
}

