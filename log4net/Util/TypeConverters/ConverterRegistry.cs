namespace log4net.Util.TypeConverters
{
    using log4net.Layout;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Net;
    using System.Text;

    public sealed class ConverterRegistry
    {
        private static readonly Type declaringType = typeof(ConverterRegistry);
        private static Hashtable s_type2converter = new Hashtable();

        static ConverterRegistry()
        {
            AddConverter(typeof(bool), typeof(BooleanConverter));
            AddConverter(typeof(Encoding), typeof(EncodingConverter));
            AddConverter(typeof(Type), typeof(TypeConverter));
            AddConverter(typeof(PatternLayout), typeof(PatternLayoutConverter));
            AddConverter(typeof(PatternString), typeof(PatternStringConverter));
            AddConverter(typeof(IPAddress), typeof(IPAddressConverter));
        }

        private ConverterRegistry()
        {
        }

        public static void AddConverter(Type destinationType, object converter)
        {
            if ((destinationType != null) && (converter != null))
            {
                lock (s_type2converter)
                {
                    s_type2converter[destinationType] = converter;
                }
            }
        }

        public static void AddConverter(Type destinationType, Type converterType)
        {
            AddConverter(destinationType, CreateConverterInstance(converterType));
        }

        private static object CreateConverterInstance(Type converterType)
        {
            if (converterType == null)
            {
                throw new ArgumentNullException("converterType", "CreateConverterInstance cannot create instance, converterType is null");
            }
            if (!typeof(IConvertFrom).IsAssignableFrom(converterType) && !typeof(IConvertTo).IsAssignableFrom(converterType))
            {
                LogLog.Error(declaringType, "Cannot CreateConverterInstance of type [" + converterType.FullName + "], type does not implement IConvertFrom or IConvertTo");
            }
            else
            {
                try
                {
                    return Activator.CreateInstance(converterType);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Cannot CreateConverterInstance of type [" + converterType.FullName + "], Exception in call to Activator.CreateInstance", exception);
                }
            }
            return null;
        }

        private static object GetConverterFromAttribute(Type destinationType)
        {
            object[] customAttributes = destinationType.GetCustomAttributes(typeof(TypeConverterAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                TypeConverterAttribute attribute = customAttributes[0] as TypeConverterAttribute;
                if (attribute != null)
                {
                    return CreateConverterInstance(SystemInfo.GetTypeFromString(destinationType, attribute.ConverterTypeName, false, true));
                }
            }
            return null;
        }

        public static IConvertFrom GetConvertFrom(Type destinationType)
        {
            lock (s_type2converter)
            {
                IConvertFrom converterFromAttribute = s_type2converter[destinationType] as IConvertFrom;
                if (converterFromAttribute == null)
                {
                    converterFromAttribute = GetConverterFromAttribute(destinationType) as IConvertFrom;
                    if (converterFromAttribute != null)
                    {
                        s_type2converter[destinationType] = converterFromAttribute;
                    }
                }
                return converterFromAttribute;
            }
        }

        public static IConvertTo GetConvertTo(Type sourceType, Type destinationType)
        {
            lock (s_type2converter)
            {
                IConvertTo converterFromAttribute = s_type2converter[sourceType] as IConvertTo;
                if (converterFromAttribute == null)
                {
                    converterFromAttribute = GetConverterFromAttribute(sourceType) as IConvertTo;
                    if (converterFromAttribute != null)
                    {
                        s_type2converter[sourceType] = converterFromAttribute;
                    }
                }
                return converterFromAttribute;
            }
        }
    }
}

