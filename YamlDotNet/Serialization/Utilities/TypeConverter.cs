namespace YamlDotNet.Serialization.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security.Permissions;
    using YamlDotNet;

    public static class TypeConverter
    {
        [CompilerGenerated]
        private static bool <RegisterTypeConverter`2>m__0<TConvertible, TConverter>(TypeConverterAttribute a) where TConverter: TypeConverter => 
            a.ConverterTypeName == typeof(TConverter).AssemblyQualifiedName;

        public static T ChangeType<T>(object value) => 
            (T) ChangeType(value, typeof(T));

        public static T ChangeType<T>(object value, CultureInfo culture) => 
            (T) ChangeType(value, typeof(T), culture);

        public static T ChangeType<T>(object value, IFormatProvider provider) => 
            (T) ChangeType(value, typeof(T), provider);

        public static object ChangeType(object value, Type destinationType) => 
            ChangeType(value, destinationType, CultureInfo.InvariantCulture);

        public static object ChangeType(object value, Type destinationType, CultureInfo culture)
        {
            if ((value == null) || (value is DBNull))
            {
                return (!destinationType.IsValueType() ? null : Activator.CreateInstance(destinationType));
            }
            Type c = value.GetType();
            if (destinationType.IsAssignableFrom(c))
            {
                return value;
            }
            if (destinationType.IsGenericType() && ReferenceEquals(destinationType.GetGenericTypeDefinition(), typeof(Nullable<>)))
            {
                Type type3 = destinationType.GetGenericArguments()[0];
                object obj2 = ChangeType(value, type3, culture);
                object[] args = new object[] { obj2 };
                return Activator.CreateInstance(destinationType, args);
            }
            if (destinationType.IsEnum())
            {
                string str = value as string;
                return ((str == null) ? value : Enum.Parse(destinationType, str, true));
            }
            if (ReferenceEquals(destinationType, typeof(bool)))
            {
                if ("0".Equals(value))
                {
                    return false;
                }
                if ("1".Equals(value))
                {
                    return true;
                }
            }
            TypeConverter converter = TypeDescriptor.GetConverter(value);
            if ((converter != null) && converter.CanConvertTo(destinationType))
            {
                return converter.ConvertTo(null, culture, value, destinationType);
            }
            TypeConverter converter2 = TypeDescriptor.GetConverter(destinationType);
            if ((converter2 != null) && converter2.CanConvertFrom(c))
            {
                return converter2.ConvertFrom(null, culture, value);
            }
            Type[] typeArray = new Type[] { c, destinationType };
            int index = 0;
            goto TR_0022;
        TR_000B:
            return (!ReferenceEquals(destinationType, typeof(TimeSpan)) ? Convert.ChangeType(value, destinationType, CultureInfo.InvariantCulture) : TimeSpan.Parse((string) ChangeType(value, typeof(string), CultureInfo.InvariantCulture)));
        TR_0022:
            while (true)
            {
                object obj3;
                if (index < typeArray.Length)
                {
                    Type type = typeArray[index];
                    using (IEnumerator<MethodInfo> enumerator = type.GetPublicStaticMethods().GetEnumerator())
                    {
                        while (true)
                        {
                            if (enumerator.MoveNext())
                            {
                                MethodInfo current = enumerator.Current;
                                if (!((current.IsSpecialName && ((current.Name == "op_Implicit") || (current.Name == "op_Explicit"))) && destinationType.IsAssignableFrom(current.ReturnParameter.ParameterType)))
                                {
                                    continue;
                                }
                                ParameterInfo[] parameters = current.GetParameters();
                                if (!((parameters.Length == 1) && parameters[0].ParameterType.IsAssignableFrom(c)))
                                {
                                    continue;
                                }
                                try
                                {
                                    object[] objArray2 = new object[] { value };
                                    obj3 = current.Invoke(null, objArray2);
                                }
                                catch (TargetInvocationException exception1)
                                {
                                    throw exception1.Unwrap();
                                }
                            }
                            else
                            {
                                break;
                            }
                            break;
                        }
                    }
                    return obj3;
                }
                else
                {
                    if (ReferenceEquals(c, typeof(string)))
                    {
                        try
                        {
                            Type[] parameterTypes = new Type[] { typeof(string), typeof(IFormatProvider) };
                            MethodInfo info2 = destinationType.GetPublicStaticMethod("Parse", parameterTypes);
                            if (info2 == null)
                            {
                                Type[] typeArray3 = new Type[] { typeof(string) };
                                info2 = destinationType.GetPublicStaticMethod("Parse", typeArray3);
                                if (info2 == null)
                                {
                                    goto TR_000B;
                                }
                                else
                                {
                                    object[] parameters = new object[] { value };
                                    obj3 = info2.Invoke(null, parameters);
                                }
                            }
                            else
                            {
                                object[] parameters = new object[] { value, culture };
                                obj3 = info2.Invoke(null, parameters);
                            }
                            return obj3;
                        }
                        catch (TargetInvocationException exception3)
                        {
                            throw exception3.Unwrap();
                        }
                    }
                    goto TR_000B;
                }
                break;
            }
            index++;
            goto TR_0022;
        }

        public static object ChangeType(object value, Type destinationType, IFormatProvider provider) => 
            ChangeType(value, destinationType, (CultureInfo) new CultureInfoAdapter(CultureInfo.CurrentCulture, provider));

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public static void RegisterTypeConverter<TConvertible, TConverter>() where TConverter: TypeConverter
        {
            if (!TypeDescriptor.GetAttributes(typeof(TConvertible)).OfType<TypeConverterAttribute>().Any<TypeConverterAttribute>(new Func<TypeConverterAttribute, bool>(TypeConverter.<RegisterTypeConverter`2>m__0<TConvertible, TConverter>)))
            {
                Attribute[] attributes = new Attribute[] { new TypeConverterAttribute(typeof(TConverter)) };
                TypeDescriptor.AddAttributes(typeof(TConvertible), attributes);
            }
        }
    }
}

