namespace log4net.Util
{
    using log4net.Core;
    using log4net.Util.TypeConverters;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;
    using System.Text;

    public sealed class OptionConverter
    {
        private static readonly Type declaringType = typeof(OptionConverter);
        private const string DELIM_START = "${";
        private const char DELIM_STOP = '}';
        private const int DELIM_START_LEN = 2;
        private const int DELIM_STOP_LEN = 1;

        private OptionConverter()
        {
        }

        public static bool CanConvertTypeTo(Type sourceType, Type targetType)
        {
            if ((sourceType == null) || (targetType == null))
            {
                return false;
            }
            if (targetType.IsAssignableFrom(sourceType))
            {
                return true;
            }
            IConvertTo convertTo = ConverterRegistry.GetConvertTo(sourceType, targetType);
            if ((convertTo != null) && convertTo.CanConvertTo(targetType))
            {
                return true;
            }
            IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(targetType);
            return ((convertFrom != null) && convertFrom.CanConvertFrom(sourceType));
        }

        public static object ConvertStringTo(Type target, string txt)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (ReferenceEquals(typeof(string), target) || ReferenceEquals(typeof(object), target))
            {
                return txt;
            }
            IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(target);
            if ((convertFrom != null) && convertFrom.CanConvertFrom(typeof(string)))
            {
                return convertFrom.ConvertFrom(txt);
            }
            if (target.IsEnum)
            {
                return ParseEnum(target, txt, true);
            }
            Type[] types = new Type[] { typeof(string) };
            MethodInfo method = target.GetMethod("Parse", types);
            if (method == null)
            {
                return null;
            }
            object[] parameters = new object[] { txt };
            return method.Invoke(null, BindingFlags.InvokeMethod, null, parameters, CultureInfo.InvariantCulture);
        }

        public static object ConvertTypeTo(object sourceInstance, Type targetType)
        {
            Type c = sourceInstance.GetType();
            if (targetType.IsAssignableFrom(c))
            {
                return sourceInstance;
            }
            IConvertTo convertTo = ConverterRegistry.GetConvertTo(c, targetType);
            if ((convertTo != null) && convertTo.CanConvertTo(targetType))
            {
                return convertTo.ConvertTo(sourceInstance, targetType);
            }
            IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(targetType);
            if ((convertFrom != null) && convertFrom.CanConvertFrom(c))
            {
                return convertFrom.ConvertFrom(sourceInstance);
            }
            string[] textArray1 = new string[] { "Cannot convert source object [", sourceInstance.ToString(), "] to target type [", targetType.Name, "]" };
            throw new ArgumentException(string.Concat(textArray1), "sourceInstance");
        }

        public static object InstantiateByClassName(string className, Type superClass, object defaultValue)
        {
            if (className != null)
            {
                try
                {
                    object obj2;
                    Type c = SystemInfo.GetTypeFromString(className, true, true);
                    if (superClass.IsAssignableFrom(c))
                    {
                        obj2 = Activator.CreateInstance(c);
                    }
                    else
                    {
                        string[] textArray1 = new string[] { "OptionConverter: A [", className, "] object is not assignable to a [", superClass.FullName, "] variable." };
                        LogLog.Error(declaringType, string.Concat(textArray1));
                        obj2 = defaultValue;
                    }
                    return obj2;
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Could not instantiate class [" + className + "].", exception);
                }
            }
            return defaultValue;
        }

        private static object ParseEnum(Type enumType, string value, bool ignoreCase) => 
            Enum.Parse(enumType, value, ignoreCase);

        public static string SubstituteVariables(string value, IDictionary props)
        {
            StringBuilder builder = new StringBuilder();
            int startIndex = 0;
            while (true)
            {
                int index = value.IndexOf("${", startIndex);
                if (index == -1)
                {
                    if (startIndex == 0)
                    {
                        return value;
                    }
                    builder.Append(value.Substring(startIndex, value.Length - startIndex));
                    return builder.ToString();
                }
                builder.Append(value.Substring(startIndex, index - startIndex));
                int num3 = value.IndexOf('}', index);
                if (num3 == -1)
                {
                    object[] objArray1 = new object[] { "[", value, "] has no closing brace. Opening brace at position [", index, "]" };
                    throw new LogException(string.Concat(objArray1));
                }
                index += 2;
                string str = value.Substring(index, num3 - index);
                string str2 = props[str] as string;
                if (str2 != null)
                {
                    builder.Append(str2);
                }
                startIndex = num3 + 1;
            }
        }

        public static bool ToBoolean(string argValue, bool defaultValue)
        {
            if ((argValue != null) && (argValue.Length > 0))
            {
                try
                {
                    return bool.Parse(argValue);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "[" + argValue + "] is not in proper bool form.", exception);
                }
            }
            return defaultValue;
        }

        public static long ToFileSize(string argValue, long defaultValue)
        {
            if (argValue != null)
            {
                string s = argValue.Trim().ToUpper(CultureInfo.InvariantCulture);
                long num = 1L;
                int index = s.IndexOf("KB");
                if (index != -1)
                {
                    num = 0x400L;
                    s = s.Substring(0, index);
                }
                else
                {
                    index = s.IndexOf("MB");
                    if (index != -1)
                    {
                        num = 0x100000L;
                        s = s.Substring(0, index);
                    }
                    else
                    {
                        index = s.IndexOf("GB");
                        if (index != -1)
                        {
                            num = 0x40000000L;
                            s = s.Substring(0, index);
                        }
                    }
                }
                if (s != null)
                {
                    long num3;
                    s = s.Trim();
                    if (SystemInfo.TryParse(s, out num3))
                    {
                        return (num3 * num);
                    }
                    LogLog.Error(declaringType, "OptionConverter: [" + s + "] is not in the correct file size syntax.");
                }
            }
            return defaultValue;
        }
    }
}

