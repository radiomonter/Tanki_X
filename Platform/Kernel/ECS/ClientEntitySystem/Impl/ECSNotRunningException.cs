namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class ECSNotRunningException : Exception
    {
        public ECSNotRunningException()
        {
        }

        public ECSNotRunningException(string str) : base(str)
        {
        }
    }
}

