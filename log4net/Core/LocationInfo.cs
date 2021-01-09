namespace log4net.Core
{
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Security;

    [Serializable]
    public class LocationInfo
    {
        private readonly string m_className;
        private readonly string m_fileName;
        private readonly string m_lineNumber;
        private readonly string m_methodName;
        private readonly string m_fullInfo;
        private readonly StackFrameItem[] m_stackFrames;
        private static readonly Type declaringType = typeof(LocationInfo);
        private const string NA = "?";

        public LocationInfo(Type callerStackBoundaryDeclaringType)
        {
            this.m_className = "?";
            this.m_fileName = "?";
            this.m_lineNumber = "?";
            this.m_methodName = "?";
            this.m_fullInfo = "?";
            if (callerStackBoundaryDeclaringType != null)
            {
                try
                {
                    StackTrace trace = new StackTrace(true);
                    int index = 0;
                    while (true)
                    {
                        if (index < trace.FrameCount)
                        {
                            StackFrame frame = trace.GetFrame(index);
                            if ((frame == null) || !ReferenceEquals(frame.GetMethod().DeclaringType, callerStackBoundaryDeclaringType))
                            {
                                index++;
                                continue;
                            }
                        }
                        while (true)
                        {
                            if (index < trace.FrameCount)
                            {
                                StackFrame frame = trace.GetFrame(index);
                                if ((frame == null) || ReferenceEquals(frame.GetMethod().DeclaringType, callerStackBoundaryDeclaringType))
                                {
                                    index++;
                                    continue;
                                }
                            }
                            if (index < trace.FrameCount)
                            {
                                int capacity = trace.FrameCount - index;
                                ArrayList list = new ArrayList(capacity);
                                this.m_stackFrames = new StackFrameItem[capacity];
                                int num3 = index;
                                while (true)
                                {
                                    if (num3 >= trace.FrameCount)
                                    {
                                        list.CopyTo(this.m_stackFrames, 0);
                                        StackFrame frame = trace.GetFrame(index);
                                        if (frame != null)
                                        {
                                            MethodBase method = frame.GetMethod();
                                            if (method != null)
                                            {
                                                this.m_methodName = method.Name;
                                                if (method.DeclaringType != null)
                                                {
                                                    this.m_className = method.DeclaringType.FullName;
                                                }
                                            }
                                            this.m_fileName = frame.GetFileName();
                                            this.m_lineNumber = frame.GetFileLineNumber().ToString(NumberFormatInfo.InvariantInfo);
                                            object[] objArray1 = new object[] { this.m_className, '.', this.m_methodName, '(', this.m_fileName, ':', this.m_lineNumber, ')' };
                                            this.m_fullInfo = string.Concat(objArray1);
                                        }
                                        break;
                                    }
                                    list.Add(new StackFrameItem(trace.GetFrame(num3)));
                                    num3++;
                                }
                            }
                            break;
                        }
                        break;
                    }
                }
                catch (SecurityException)
                {
                    LogLog.Debug(declaringType, "Security exception while trying to get caller stack frame. Error Ignored. Location Information Not Available.");
                }
            }
        }

        public LocationInfo(string className, string methodName, string fileName, string lineNumber)
        {
            this.m_className = className;
            this.m_fileName = fileName;
            this.m_lineNumber = lineNumber;
            this.m_methodName = methodName;
            object[] objArray1 = new object[] { this.m_className, '.', this.m_methodName, '(', this.m_fileName, ':', this.m_lineNumber, ')' };
            this.m_fullInfo = string.Concat(objArray1);
        }

        public string ClassName =>
            this.m_className;

        public string FileName =>
            this.m_fileName;

        public string LineNumber =>
            this.m_lineNumber;

        public string MethodName =>
            this.m_methodName;

        public string FullInfo =>
            this.m_fullInfo;

        public StackFrameItem[] StackFrames =>
            this.m_stackFrames;
    }
}

