namespace log4net.Util
{
    using log4net.Core;
    using System;
    using System.Collections;
    using System.Globalization;

    public sealed class PatternParser
    {
        private const char ESCAPE_CHAR = '%';
        private PatternConverter m_head;
        private PatternConverter m_tail;
        private string m_pattern;
        private Hashtable m_patternConverters = new Hashtable();
        private static readonly Type declaringType = typeof(PatternParser);

        public PatternParser(string pattern)
        {
            this.m_pattern = pattern;
        }

        private void AddConverter(PatternConverter pc)
        {
            if (this.m_head != null)
            {
                this.m_tail = this.m_tail.SetNext(pc);
            }
            else
            {
                this.m_head = this.m_tail = pc;
            }
        }

        private string[] BuildCache()
        {
            string[] array = new string[this.m_patternConverters.Keys.Count];
            this.m_patternConverters.Keys.CopyTo(array, 0);
            Array.Sort(array, 0, array.Length, StringLengthComparer.Instance);
            return array;
        }

        public PatternConverter Parse()
        {
            string[] matches = this.BuildCache();
            this.ParseInternal(this.m_pattern, matches);
            return this.m_head;
        }

        private void ParseInternal(string pattern, string[] matches)
        {
            int startIndex = 0;
            while (startIndex < pattern.Length)
            {
                int index = pattern.IndexOf('%', startIndex);
                if ((index < 0) || (index == (pattern.Length - 1)))
                {
                    this.ProcessLiteral(pattern.Substring(startIndex));
                    startIndex = pattern.Length;
                    continue;
                }
                if (pattern[index + 1] == '%')
                {
                    this.ProcessLiteral(pattern.Substring(startIndex, (index - startIndex) + 1));
                    startIndex = index + 2;
                    continue;
                }
                this.ProcessLiteral(pattern.Substring(startIndex, index - startIndex));
                startIndex = index + 1;
                FormattingInfo formattingInfo = new FormattingInfo();
                if ((startIndex < pattern.Length) && (pattern[startIndex] == '-'))
                {
                    formattingInfo.LeftAlign = true;
                    startIndex++;
                }
                while (true)
                {
                    if ((startIndex >= pattern.Length) || !char.IsDigit(pattern[startIndex]))
                    {
                        if ((startIndex < pattern.Length) && (pattern[startIndex] == '.'))
                        {
                            startIndex++;
                        }
                        while (true)
                        {
                            if ((startIndex >= pattern.Length) || !char.IsDigit(pattern[startIndex]))
                            {
                                int num3 = pattern.Length - startIndex;
                                for (int i = 0; i < matches.Length; i++)
                                {
                                    if ((matches[i].Length <= num3) && (string.Compare(pattern, startIndex, matches[i], 0, matches[i].Length, false, CultureInfo.InvariantCulture) == 0))
                                    {
                                        startIndex += matches[i].Length;
                                        string option = null;
                                        if ((startIndex < pattern.Length) && (pattern[startIndex] == '{'))
                                        {
                                            startIndex++;
                                            int num5 = pattern.IndexOf('}', startIndex);
                                            if (num5 >= 0)
                                            {
                                                option = pattern.Substring(startIndex, num5 - startIndex);
                                                startIndex = num5 + 1;
                                            }
                                        }
                                        this.ProcessConverter(matches[i], option, formattingInfo);
                                        break;
                                    }
                                }
                                break;
                            }
                            if (formattingInfo.Max == 0x7fffffff)
                            {
                                formattingInfo.Max = 0;
                            }
                            formattingInfo.Max = (formattingInfo.Max * 10) + int.Parse(pattern[startIndex].ToString(CultureInfo.InvariantCulture), NumberFormatInfo.InvariantInfo);
                            startIndex++;
                        }
                        break;
                    }
                    if (formattingInfo.Min < 0)
                    {
                        formattingInfo.Min = 0;
                    }
                    formattingInfo.Min = (formattingInfo.Min * 10) + int.Parse(pattern[startIndex].ToString(CultureInfo.InvariantCulture), NumberFormatInfo.InvariantInfo);
                    startIndex++;
                }
            }
        }

        private void ProcessConverter(string converterName, string option, FormattingInfo formattingInfo)
        {
            object[] objArray1 = new object[11];
            objArray1[0] = "Converter [";
            objArray1[1] = converterName;
            objArray1[2] = "] Option [";
            objArray1[3] = option;
            objArray1[4] = "] Format [min=";
            objArray1[5] = formattingInfo.Min;
            objArray1[6] = ",max=";
            objArray1[7] = formattingInfo.Max;
            objArray1[8] = ",leftAlign=";
            objArray1[9] = formattingInfo.LeftAlign;
            objArray1[10] = "]";
            LogLog.Debug(declaringType, string.Concat(objArray1));
            ConverterInfo info = (ConverterInfo) this.m_patternConverters[converterName];
            if (info == null)
            {
                LogLog.Error(declaringType, "Unknown converter name [" + converterName + "] in conversion pattern.");
            }
            else
            {
                PatternConverter pc = null;
                try
                {
                    pc = (PatternConverter) Activator.CreateInstance(info.Type);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to create instance of Type [" + info.Type.FullName + "] using default constructor. Exception: " + exception.ToString());
                }
                pc.FormattingInfo = formattingInfo;
                pc.Option = option;
                pc.Properties = info.Properties;
                IOptionHandler handler = pc as IOptionHandler;
                if (handler != null)
                {
                    handler.ActivateOptions();
                }
                this.AddConverter(pc);
            }
        }

        private void ProcessLiteral(string text)
        {
            if (text.Length > 0)
            {
                this.ProcessConverter("literal", text, new FormattingInfo());
            }
        }

        public Hashtable PatternConverters =>
            this.m_patternConverters;

        private sealed class StringLengthComparer : IComparer
        {
            public static readonly PatternParser.StringLengthComparer Instance = new PatternParser.StringLengthComparer();

            private StringLengthComparer()
            {
            }

            public int Compare(object x, object y)
            {
                string str = x as string;
                string str2 = y as string;
                return (((str != null) || (str2 != null)) ? ((str != null) ? ((str2 != null) ? str2.Length.CompareTo(str.Length) : -1) : 1) : 0);
            }
        }
    }
}

