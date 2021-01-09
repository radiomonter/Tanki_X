﻿namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class EventAbsentInArgumentHandlerFactoryException : HandlerFactoryException
    {
        public EventAbsentInArgumentHandlerFactoryException(MethodInfo method) : base(method)
        {
        }

        public EventAbsentInArgumentHandlerFactoryException(MethodInfo method, Type type) : base(method, type)
        {
        }
    }
}

