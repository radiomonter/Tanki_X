namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class Handler : IComparable<Handler>
    {
        private static int activeHandlersCount;
        private readonly MethodHandle _methodHandle;
        private string _fullMethodName;
        public Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerInvokeGraph HandlerInvokeGraph;
        private readonly int _profilerIndex;

        public Handler(Platform.Kernel.ECS.ClientEntitySystem.Impl.EventPhase eventPhase, Type eventType, MethodInfo method, MethodHandle methodHandle, Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerArgumentsDescription handlerArgumentsDescription)
        {
            this.EventPhase = eventPhase;
            this.EventType = eventType;
            this.Method = method;
            this._methodHandle = methodHandle;
            this.HandlerArgumentsDescription = handlerArgumentsDescription;
            this.Mandatory = IsMandatory(method);
            this.SkipInfo = IsSkipInfo(method);
            this.ProjectName = this.GetProjectName();
            this.Name = this.GetHandlerName();
            bool flag = true;
            this.ContextArguments = new List<HandlerArgument>();
            foreach (HandlerArgument argument in this.HandlerArgumentsDescription.HandlerArguments)
            {
                if (argument.Context)
                {
                    this.ContextArguments.Add(argument);
                }
                if (argument.JoinType.IsPresent())
                {
                    flag = false;
                }
            }
            this.IsContextOnlyArguments = flag;
            this.IsEventOnlyArguments = this.HandlerArgumentsDescription.HandlerArguments.Count == 0;
            this.HandlerInvokeGraph = new Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerInvokeGraph(this);
        }

        public void ChangeVersion()
        {
            this.Version++;
        }

        public int CompareTo(Handler other) => 
            this.getKey().CompareTo(other.getKey());

        private string GetFullMethodName()
        {
            ParameterInfo[] parameters = this.Method.GetParameters();
            string str = $"{this.Name}(";
            foreach (ParameterInfo info in parameters)
            {
                str = !info.ParameterType.IsGenericType ? (str + info.ParameterType.Name + ", ") : (str + info.ParameterType + ", ");
            }
            if (parameters.Length > 0)
            {
                str = str.Remove(str.Length - 2);
            }
            return (str + ")");
        }

        public string GetHandlerName()
        {
            Type declaringType = this.Method.DeclaringType;
            return ((declaringType == null) ? this.Method.Name : (declaringType.Name + "." + this.Method.Name));
        }

        private string getKey() => 
            this.Method.ToString();

        private string GetProjectName()
        {
            Type declaringType = this.Method.DeclaringType;
            if (declaringType != null)
            {
                string str = declaringType.Namespace;
                if (!string.IsNullOrEmpty(str))
                {
                    int index = str.IndexOf('.');
                    return ((index < 0) ? str : str.Substring(0, index));
                }
            }
            return string.Empty;
        }

        public object Invoke(object[] args)
        {
            try
            {
                return this._methodHandle.Invoke(args);
            }
            finally
            {
            }
        }

        private static bool IsMandatory(MethodInfo method) => 
            (!TestContext.IsTestMode || !TestContext.Current.IsDataExists(typeof(MandatoryDisabled))) ? (method.GetCustomAttributes(typeof(Platform.Kernel.ECS.ClientEntitySystem.API.Mandatory), true).Length == 1) : false;

        private static bool IsSkipInfo(MethodInfo method) => 
            method.GetCustomAttributes(typeof(Platform.Kernel.ECS.ClientEntitySystem.API.SkipInfo), true).Length == 1;

        public override string ToString() => 
            $"[{base.GetType().Name} {this.ProjectName}.{this.Name}]";

        public int Version { get; private set; }

        public IList<HandlerArgument> ContextArguments { get; private set; }

        public bool IsContextOnlyArguments { get; private set; }

        public bool IsEventOnlyArguments { get; private set; }

        public MethodInfo Method { get; internal set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerArgumentsDescription HandlerArgumentsDescription { get; internal set; }

        public bool Mandatory { get; internal set; }

        public bool SkipInfo { get; internal set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.EventPhase EventPhase { get; internal set; }

        public Type EventType { get; internal set; }

        public string ProjectName { get; internal set; }

        public string Name { get; internal set; }

        public string FullMethodName
        {
            get
            {
                if (string.IsNullOrEmpty(this._fullMethodName))
                {
                    this._fullMethodName = this.GetFullMethodName();
                }
                return this._fullMethodName;
            }
        }

        private class NodeDescriptionState
        {
            public int count;
        }
    }
}

