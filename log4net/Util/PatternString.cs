namespace log4net.Util
{
    using log4net.Core;
    using log4net.Util.PatternStringConverters;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;

    public class PatternString : IOptionHandler
    {
        private static Hashtable s_globalRulesRegistry = new Hashtable(15);
        private string m_pattern;
        private PatternConverter m_head;
        private Hashtable m_instanceRulesRegistry;

        static PatternString()
        {
            s_globalRulesRegistry.Add("appdomain", typeof(AppDomainPatternConverter));
            s_globalRulesRegistry.Add("date", typeof(DatePatternConverter));
            s_globalRulesRegistry.Add("env", typeof(EnvironmentPatternConverter));
            s_globalRulesRegistry.Add("envFolderPath", typeof(EnvironmentFolderPathPatternConverter));
            s_globalRulesRegistry.Add("identity", typeof(IdentityPatternConverter));
            s_globalRulesRegistry.Add("literal", typeof(LiteralPatternConverter));
            s_globalRulesRegistry.Add("newline", typeof(NewLinePatternConverter));
            s_globalRulesRegistry.Add("processid", typeof(ProcessIdPatternConverter));
            s_globalRulesRegistry.Add("property", typeof(PropertyPatternConverter));
            s_globalRulesRegistry.Add("random", typeof(RandomStringPatternConverter));
            s_globalRulesRegistry.Add("username", typeof(UserNamePatternConverter));
            s_globalRulesRegistry.Add("utcdate", typeof(UtcDatePatternConverter));
            s_globalRulesRegistry.Add("utcDate", typeof(UtcDatePatternConverter));
            s_globalRulesRegistry.Add("UtcDate", typeof(UtcDatePatternConverter));
        }

        public PatternString()
        {
            this.m_instanceRulesRegistry = new Hashtable();
        }

        public PatternString(string pattern)
        {
            this.m_instanceRulesRegistry = new Hashtable();
            this.m_pattern = pattern;
            this.ActivateOptions();
        }

        public virtual void ActivateOptions()
        {
            this.m_head = this.CreatePatternParser(this.m_pattern).Parse();
        }

        public void AddConverter(ConverterInfo converterInfo)
        {
            if (converterInfo == null)
            {
                throw new ArgumentNullException("converterInfo");
            }
            if (!typeof(PatternConverter).IsAssignableFrom(converterInfo.Type))
            {
                throw new ArgumentException("The converter type specified [" + converterInfo.Type + "] must be a subclass of log4net.Util.PatternConverter", "converterInfo");
            }
            this.m_instanceRulesRegistry[converterInfo.Name] = converterInfo;
        }

        public void AddConverter(string name, Type type)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ConverterInfo converterInfo = new ConverterInfo {
                Name = name,
                Type = type
            };
            this.AddConverter(converterInfo);
        }

        private PatternParser CreatePatternParser(string pattern)
        {
            PatternParser parser = new PatternParser(pattern);
            IDictionaryEnumerator enumerator = s_globalRulesRegistry.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    ConverterInfo info = new ConverterInfo {
                        Name = (string) current.Key,
                        Type = (Type) current.Value
                    };
                    parser.PatternConverters.Add(current.Key, info);
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
            IDictionaryEnumerator enumerator2 = this.m_instanceRulesRegistry.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator2.Current;
                    parser.PatternConverters[current.Key] = current.Value;
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
            }
            return parser;
        }

        public string Format()
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            this.Format(writer);
            return writer.ToString();
        }

        public void Format(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            for (PatternConverter converter = this.m_head; converter != null; converter = converter.Next)
            {
                converter.Format(writer, null);
            }
        }

        public string ConversionPattern
        {
            get => 
                this.m_pattern;
            set => 
                this.m_pattern = value;
        }
    }
}

