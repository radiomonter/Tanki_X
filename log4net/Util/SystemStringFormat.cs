namespace log4net.Util
{
    using System;
    using System.Text;

    public sealed class SystemStringFormat
    {
        private readonly IFormatProvider m_provider;
        private readonly string m_format;
        private readonly object[] m_args;
        private static readonly Type declaringType = typeof(SystemStringFormat);

        public SystemStringFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.m_provider = provider;
            this.m_format = format;
            this.m_args = args;
        }

        private static void RenderArray(Array array, StringBuilder buffer)
        {
            if (array == null)
            {
                buffer.Append(SystemInfo.NullText);
            }
            else if (array.Rank != 1)
            {
                buffer.Append(array.ToString());
            }
            else
            {
                buffer.Append("{");
                int length = array.Length;
                if (length > 0)
                {
                    RenderObject(array.GetValue(0), buffer);
                    for (int i = 1; i < length; i++)
                    {
                        buffer.Append(", ");
                        RenderObject(array.GetValue(i), buffer);
                    }
                }
                buffer.Append("}");
            }
        }

        private static void RenderObject(object obj, StringBuilder buffer)
        {
            if (obj == null)
            {
                buffer.Append(SystemInfo.NullText);
            }
            else
            {
                try
                {
                    buffer.Append(obj);
                }
                catch (Exception exception)
                {
                    buffer.Append("<Exception: ").Append(exception.Message).Append(">");
                }
            }
        }

        private static string StringFormat(IFormatProvider provider, string format, params object[] args)
        {
            try
            {
                return ((format != null) ? ((args != null) ? string.Format(provider, format, args) : format) : null);
            }
            catch (Exception exception)
            {
                LogLog.Warn(declaringType, "Exception while rendering format [" + format + "]", exception);
                return StringFormatError(exception, format, args);
            }
        }

        private static string StringFormatError(Exception formatException, string format, object[] args)
        {
            try
            {
                StringBuilder buffer = new StringBuilder("<log4net.Error>");
                if (formatException != null)
                {
                    buffer.Append("Exception during StringFormat: ").Append(formatException.Message);
                }
                else
                {
                    buffer.Append("Exception during StringFormat");
                }
                buffer.Append(" <format>").Append(format).Append("</format>");
                buffer.Append("<args>");
                RenderArray(args, buffer);
                buffer.Append("</args>");
                buffer.Append("</log4net.Error>");
                return buffer.ToString();
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "INTERNAL ERROR during StringFormat error handling", exception);
                return "<log4net.Error>Exception during StringFormat. See Internal Log.</log4net.Error>";
            }
        }

        public override string ToString() => 
            StringFormat(this.m_provider, this.m_format, this.m_args);
    }
}

