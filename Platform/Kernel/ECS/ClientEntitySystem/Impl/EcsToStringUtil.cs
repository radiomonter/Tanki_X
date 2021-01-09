namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class EcsToStringUtil
    {
        private static Type[] emptyTypes = new Type[0];

        public static string AttributesToString(object[] annotations)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            bool flag = true;
            foreach (object obj2 in annotations)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    builder.Append(',');
                }
                builder.Append(obj2.GetType().Name);
            }
            builder.Append(']');
            return builder.ToString();
        }

        public static string EnumerableToString(IEnumerable enumerable) => 
            EnumerableToString(enumerable, ",");

        public static string EnumerableToString(IEnumerable enumerable, string separator) => 
            enumerable.GetType().Name + EnumerableWithoutTypeToString(enumerable, separator);

        public static string EnumerableWithoutTypeToString(IEnumerable enumerable, string separator)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            int num = 0;
            IEnumerator enumerator = enumerable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (num > 0)
                    {
                        builder.Append(separator);
                    }
                    builder.Append(current);
                    num++;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            builder.Append(']');
            return builder.ToString();
        }

        private static bool HasToStringMethod(Type type) => 
            !ReferenceEquals(type.GetMethod("ToString", emptyTypes).DeclaringType, typeof(object));

        private static bool NeedShow(PropertyInfo property) => 
            typeof(Component).IsAssignableFrom(property.DeclaringType) ? (property.GetCustomAttributes(typeof(ObsoleteAttribute), false).Count<object>() <= 0) : false;

        public static object PropertyToString(object obj, PropertyInfo property)
        {
            object obj2 = property.GetValue(obj, BindingFlags.Default, null, null, null);
            return ((obj2 != null) ? (!(obj2 is string) ? ((!typeof(IEnumerable).IsAssignableFrom(property.PropertyType) || HasToStringMethod(property.PropertyType)) ? obj2 : EnumerableToString((IEnumerable) obj2, ",")) : obj2) : "null");
        }

        public static string ToString(Entity entity) => 
            !(entity is EntityStub) ? $"[Name={entity.Name},	id={entity.Id}]" : "[EntityStub]";

        public static string ToString(Handler handler)
        {
            StringBuilder builder = new StringBuilder();
            MethodInfo method = handler.Method;
            builder.Append(AttributesToString(method.GetCustomAttributes(true)));
            builder.Append(" ");
            builder.Append(method.DeclaringType.Name + "." + method.Name);
            builder.Append("(" + ToString(method) + ")");
            builder.Append(" ");
            builder.Append("\n");
            return builder.ToString();
        }

        public static string ToString(ICollection<Handler> handlers)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            bool flag = true;
            foreach (Handler handler in handlers)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    builder.Append(',');
                }
                builder.Append(handler.Method.Name);
            }
            builder.Append(']');
            return builder.ToString();
        }

        public static string ToString(ICollection<Type> components)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            bool flag = true;
            foreach (Type type in components)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    builder.Append(',');
                }
                builder.Append(type.Name);
            }
            builder.Append(']');
            return builder.ToString();
        }

        public static object ToString(object[] objects)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            bool flag = true;
            foreach (object obj2 in objects)
            {
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    builder.Append(',');
                }
                builder.Append(obj2);
            }
            builder.Append(']');
            return builder.ToString();
        }

        public static string ToString(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(method.DeclaringType.Name).Append("::").Append(method.Name).Append("(");
            ParameterInfo[] parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo info = parameters[i];
                if (i > 0)
                {
                    builder.Append(", ");
                }
                object[] customAttributes = info.GetCustomAttributes(true);
                if (customAttributes.Length > 0)
                {
                    builder.Append(AttributesToString(customAttributes));
                    builder.Append(" ");
                }
                if (info.ParameterType.IsSubclassOf(typeof(ICollection)))
                {
                    builder.Append("Collection<" + info.ParameterType.GetGenericArguments()[0].Name + ">");
                }
                else
                {
                    builder.Append(info.ParameterType.Name);
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        public static string ToStringWithComponents(EntityInternal entity) => 
            !(entity is EntityStub) ? $"[{entity.Name},	{entity.Id},	{ToString(entity.ComponentClasses)}]" : "[EntityStub]";

        public static string ToStringWithProperties(object obj, string delimeter = ", ")
        {
            string str2;
            string name = obj.GetType().Name;
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length == 0)
            {
                return name;
            }
            name = name + " [";
            PropertyInfo info = null;
            try
            {
                int num = 0;
                foreach (PropertyInfo info2 in properties)
                {
                    num++;
                    if (NeedShow(info2))
                    {
                        info = info2;
                        str2 = name;
                        object[] objArray1 = new object[] { str2, info2.Name, "=", PropertyToString(obj, info2) };
                        name = string.Concat(objArray1);
                        if (num < properties.Length)
                        {
                            name = name + delimeter;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str2 = name;
                object[] objArray2 = new object[] { str2, exception.Message, " property=", info };
                name = string.Concat(objArray2);
            }
            return (name + "]");
        }
    }
}

