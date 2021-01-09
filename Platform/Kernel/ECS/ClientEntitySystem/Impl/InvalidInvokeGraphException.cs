namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class InvalidInvokeGraphException : Exception
    {
        public InvalidInvokeGraphException(Handler handler) : base($"Supposed handler call, but skipped {handler}")
        {
        }
    }
}

