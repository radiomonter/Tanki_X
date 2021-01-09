namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Text;

    internal class StackTraceDetailPatternConverter : StackTracePatternConverter
    {
        private static readonly Type declaringType = typeof(StackTracePatternConverter);

        internal override string GetMethodInformation(MethodItem method)
        {
            string str = string.Empty;
            try
            {
                string str2 = string.Empty;
                string[] parameters = method.Parameters;
                StringBuilder builder = new StringBuilder();
                if ((parameters != null) && (parameters.GetUpperBound(0) > 0))
                {
                    for (int i = 0; i <= parameters.GetUpperBound(0); i++)
                    {
                        builder.AppendFormat("{0}, ", parameters[i]);
                    }
                }
                if (builder.Length > 0)
                {
                    builder.Remove(builder.Length - 2, 2);
                    str2 = builder.ToString();
                }
                str = base.GetMethodInformation(method) + "(" + str2 + ")";
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "An exception ocurred while retreiving method information.", exception);
            }
            return str;
        }
    }
}

