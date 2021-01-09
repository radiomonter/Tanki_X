namespace log4net.Core
{
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class MethodItem
    {
        private readonly string m_name;
        private readonly string[] m_parameters;
        private static readonly Type declaringType = typeof(MethodItem);
        private const string NA = "?";

        public MethodItem()
        {
            this.m_name = "?";
            this.m_parameters = new string[0];
        }

        public MethodItem(MethodBase methodBase) : this(methodBase.Name, GetMethodParameterNames(methodBase))
        {
        }

        public MethodItem(string name) : this()
        {
            this.m_name = name;
        }

        public MethodItem(string name, string[] parameters) : this(name)
        {
            this.m_parameters = parameters;
        }

        private static string[] GetMethodParameterNames(MethodBase methodBase)
        {
            ArrayList list = new ArrayList();
            try
            {
                ParameterInfo[] parameters = methodBase.GetParameters();
                int upperBound = parameters.GetUpperBound(0);
                for (int i = 0; i <= upperBound; i++)
                {
                    list.Add(parameters[i].ParameterType + " " + parameters[i].Name);
                }
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "An exception ocurred while retreiving method parameters.", exception);
            }
            return (string[]) list.ToArray(typeof(string));
        }

        public string Name =>
            this.m_name;

        public string[] Parameters =>
            this.m_parameters;
    }
}

