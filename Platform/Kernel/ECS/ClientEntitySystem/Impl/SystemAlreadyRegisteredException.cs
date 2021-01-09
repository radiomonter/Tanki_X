﻿namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class SystemAlreadyRegisteredException : Exception
    {
        public SystemAlreadyRegisteredException(Type systemType) : base("system = " + systemType)
        {
        }
    }
}

