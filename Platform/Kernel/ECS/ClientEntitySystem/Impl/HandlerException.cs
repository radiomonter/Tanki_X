namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class HandlerException : Exception
    {
        public HandlerException(string message, Exception cause) : base(message, cause)
        {
        }
    }
}

