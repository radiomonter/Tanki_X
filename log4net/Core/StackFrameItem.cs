namespace log4net.Core
{
    using log4net.Util;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    [Serializable]
    public class StackFrameItem
    {
        private readonly string m_lineNumber = "?";
        private readonly string m_fileName = "?";
        private readonly string m_className = "?";
        private readonly string m_fullInfo;
        private readonly MethodItem m_method = new MethodItem();
        private static readonly Type declaringType = typeof(StackFrameItem);
        private const string NA = "?";

        public StackFrameItem(StackFrame frame)
        {
            try
            {
                this.m_lineNumber = frame.GetFileLineNumber().ToString(NumberFormatInfo.InvariantInfo);
                this.m_fileName = frame.GetFileName();
                MethodBase method = frame.GetMethod();
                if (method != null)
                {
                    if (method.DeclaringType != null)
                    {
                        this.m_className = method.DeclaringType.FullName;
                    }
                    this.m_method = new MethodItem(method);
                }
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "An exception ocurred while retreiving stack frame information.", exception);
            }
            object[] objArray1 = new object[] { this.m_className, '.', this.m_method.Name, '(', this.m_fileName, ':', this.m_lineNumber, ')' };
            this.m_fullInfo = string.Concat(objArray1);
        }

        public string ClassName =>
            this.m_className;

        public string FileName =>
            this.m_fileName;

        public string LineNumber =>
            this.m_lineNumber;

        public MethodItem Method =>
            this.m_method;

        public string FullInfo =>
            this.m_fullInfo;
    }
}

