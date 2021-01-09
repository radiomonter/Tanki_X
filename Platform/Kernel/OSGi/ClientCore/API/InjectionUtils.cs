namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class InjectionUtils
    {
        [CompilerGenerated]
        private static Action<PropertyInfo> <>f__mg$cache0;
        [CompilerGenerated]
        private static Action<PropertyInfo> <>f__mg$cache1;

        private static void Clear(PropertyInfo propertyInfo)
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod(true);
            if (setMethod.IsStatic)
            {
                if (setMethod.ContainsGenericParameters)
                {
                    Debug.LogError("Fail to inject to generic class " + setMethod.ReflectedType);
                }
                setMethod.Invoke(null, new object[1]);
            }
        }

        public static void ClearInjectionPoints(List<PropertyInfo> props)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Action<PropertyInfo>(InjectionUtils.Clear);
            }
            props.ForEach(<>f__mg$cache1);
        }

        public static void ClearInjectionPoints(Type injectionAttributeType)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (<>f__mg$cache0 == null)
                {
                    <>f__mg$cache0 = new Action<PropertyInfo>(InjectionUtils.Clear);
                }
                ProcessInjectionPoints(injectionAttributeType, GetTypes(assembly), <>f__mg$cache0);
            }
        }

        public static List<PropertyInfo> GetInjectableProps(Assembly assembly, Type injectAttributeType)
        {
            <GetInjectableProps>c__AnonStorey0 storey = new <GetInjectableProps>c__AnonStorey0 {
                props = new List<PropertyInfo>()
            };
            Type[] types = GetTypes(assembly);
            ProcessInjectionPoints(injectAttributeType, types, new Action<PropertyInfo>(storey.<>m__0));
            return storey.props;
        }

        public static Type[] GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (Exception)
            {
                return new Type[0];
            }
        }

        private static void ProcessInjectionPoints(Type injectAttributeType, Type[] types, Action<PropertyInfo> action)
        {
            Type[] typeArray = types;
            int index = 0;
            while (index < typeArray.Length)
            {
                Type type = typeArray[index];
                BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
                PropertyInfo[] infoArray2 = type.GetProperties(bindingAttr);
                int num2 = 0;
                while (true)
                {
                    if (num2 >= infoArray2.Length)
                    {
                        index++;
                        break;
                    }
                    PropertyInfo info = infoArray2[num2];
                    if (info.IsDefined(injectAttributeType, false))
                    {
                        action(info);
                    }
                    num2++;
                }
            }
        }

        public static void RegisterInjectionPoints(Type injectAttributeType, ServiceRegistry serviceRegistry)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                RegisterInjectionPoints(injectAttributeType, serviceRegistry, GetTypes(assembly));
            }
        }

        public static void RegisterInjectionPoints(Type injectAttributeType, ServiceRegistry serviceRegistry, Type[] types)
        {
            ProcessInjectionPoints(injectAttributeType, types, new Action<PropertyInfo>(serviceRegistry.RegisterConsumer));
        }

        [CompilerGenerated]
        private sealed class <GetInjectableProps>c__AnonStorey0
        {
            internal List<PropertyInfo> props;

            internal void <>m__0(PropertyInfo pi)
            {
                this.props.Add(pi);
            }
        }
    }
}

