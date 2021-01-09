namespace log4net.Layout
{
    using log4net.Util.TypeConverters;
    using System;

    public class RawLayoutConverter : IConvertFrom
    {
        public bool CanConvertFrom(Type sourceType) => 
            typeof(ILayout).IsAssignableFrom(sourceType);

        public object ConvertFrom(object source)
        {
            ILayout layout = source as ILayout;
            if (layout == null)
            {
                throw ConversionNotSupportedException.Create(typeof(IRawLayout), source);
            }
            return new Layout2RawLayoutAdapter(layout);
        }
    }
}

