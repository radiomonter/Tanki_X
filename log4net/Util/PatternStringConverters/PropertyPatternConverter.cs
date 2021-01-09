namespace log4net.Util.PatternStringConverters
{
    using log4net;
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class PropertyPatternConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            CompositeProperties properties = new CompositeProperties();
            PropertiesDictionary dictionary = ThreadContext.Properties.GetProperties(false);
            if (dictionary != null)
            {
                properties.Add(dictionary);
            }
            properties.Add(GlobalContext.Properties.GetReadOnlyProperties());
            if (this.Option != null)
            {
                WriteObject(writer, null, properties[this.Option]);
            }
            else
            {
                WriteDictionary(writer, null, properties.Flatten());
            }
        }
    }
}

