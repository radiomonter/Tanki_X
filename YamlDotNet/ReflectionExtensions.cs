namespace YamlDotNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class ReflectionExtensions
    {
        private static readonly FieldInfo remoteStackTraceField = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.NonPublic | BindingFlags.Instance);

        public static MethodInfo GetPublicInstanceMethod(this Type type, string name) => 
            type.GetMethod(name, BindingFlags.Public | BindingFlags.Instance);

        public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
        {
            PropertyInfo[] properties;
            <GetPublicProperties>c__AnonStorey0 storey = new <GetPublicProperties>c__AnonStorey0 {
                instancePublic = BindingFlags.Public | BindingFlags.Instance
            };
            if (!type.IsInterface)
            {
                properties = type.GetProperties(storey.instancePublic);
            }
            else
            {
                Type[] first = new Type[] { type };
                properties = (PropertyInfo[]) first.Concat<Type>(type.GetInterfaces()).SelectMany<Type, PropertyInfo>(new Func<Type, IEnumerable<PropertyInfo>>(storey.<>m__0));
            }
            return properties;
        }

        public static PropertyInfo GetPublicProperty(this Type type, string name) => 
            type.GetProperty(name);

        public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] parameterTypes) => 
            type.GetMethod(name, BindingFlags.Public | BindingFlags.Static, null, parameterTypes, null);

        public static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type) => 
            type.GetMethods(BindingFlags.Public | BindingFlags.Static);

        public static TypeCode GetTypeCode(this Type type) => 
            Type.GetTypeCode(type);

        public static bool HasDefaultConstructor(this Type type) => 
            type.IsValueType || !ReferenceEquals(type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null), null);

        public static bool IsEnum(this Type type) => 
            type.IsEnum;

        public static bool IsGenericType(this Type type) => 
            type.IsGenericType;

        public static bool IsInstanceOf(this Type type, object o) => 
            type.IsInstanceOfType(o);

        public static bool IsInterface(this Type type) => 
            type.IsInterface;

        public static bool IsValueType(this Type type) => 
            type.IsValueType;

        public static Exception Unwrap(this TargetInvocationException ex)
        {
            Exception innerException = ex.InnerException;
            if (remoteStackTraceField != null)
            {
                remoteStackTraceField.SetValue(ex.InnerException, ex.InnerException.StackTrace + "\r\n");
            }
            return innerException;
        }

        [CompilerGenerated]
        private sealed class <GetPublicProperties>c__AnonStorey0
        {
            internal BindingFlags instancePublic;

            internal IEnumerable<PropertyInfo> <>m__0(Type i) => 
                i.GetProperties(this.instancePublic);
        }
    }
}

