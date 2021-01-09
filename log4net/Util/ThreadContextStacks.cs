namespace log4net.Util
{
    using System;
    using System.Reflection;

    public sealed class ThreadContextStacks
    {
        private readonly ContextPropertiesBase m_properties;
        private static readonly Type declaringType = typeof(ThreadContextStacks);

        internal ThreadContextStacks(ContextPropertiesBase properties)
        {
            this.m_properties = properties;
        }

        public ThreadContextStack this[string key]
        {
            get
            {
                ThreadContextStack stack = null;
                object obj2 = this.m_properties[key];
                if (obj2 == null)
                {
                    stack = new ThreadContextStack();
                    this.m_properties[key] = stack;
                }
                else
                {
                    stack = obj2 as ThreadContextStack;
                    if (stack == null)
                    {
                        string nullText = SystemInfo.NullText;
                        try
                        {
                            nullText = obj2.ToString();
                        }
                        catch
                        {
                        }
                        string[] textArray1 = new string[] { "ThreadContextStacks: Request for stack named [", key, "] failed because a property with the same name exists which is a [", obj2.GetType().Name, "] with value [", nullText, "]" };
                        LogLog.Error(declaringType, string.Concat(textArray1));
                        stack = new ThreadContextStack();
                    }
                }
                return stack;
            }
        }
    }
}

