namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class AssemblyTypeCollector
    {
        public static void CollectEmptyEventTypes(List<Type> eventTypes)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            int index = 0;
            while (index < assemblies.Length)
            {
                Type[] types = assemblies[index].GetTypes();
                int num2 = 0;
                while (true)
                {
                    if (num2 >= types.Length)
                    {
                        index++;
                        break;
                    }
                    Type type = types[num2];
                    if (type.IsSubclassOf(typeof(Event)) && (!type.IsAbstract && IsEmptyType(type)))
                    {
                        eventTypes.Add(type);
                    }
                    num2++;
                }
            }
        }

        private static bool IsEmptyType(Type type)
        {
            BindingFlags bindingAttr = BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            return ((type.GetFields(bindingAttr).Length == 0) && (type.GetProperties(bindingAttr).Length == 0));
        }
    }
}

