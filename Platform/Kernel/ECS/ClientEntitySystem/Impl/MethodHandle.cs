namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Reflection;

    public class MethodHandle
    {
        private readonly MethodInfo method;
        private readonly ECSSystem system;
        private readonly bool throwInnerException;

        public MethodHandle(MethodInfo method, ECSSystem system)
        {
            this.method = method;
            this.system = system;
            this.throwInnerException = TestContext.IsTestMode;
        }

        public object Invoke(object[] args)
        {
            object obj2;
            if (!this.throwInnerException)
            {
                return this.method.Invoke(this.system, args);
            }
            try
            {
                obj2 = this.method.Invoke(this.system, args);
            }
            catch (TargetInvocationException exception)
            {
                MethodInfo method = typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(exception.InnerException, null);
                }
                throw exception.InnerException;
            }
            return obj2;
        }
    }
}

