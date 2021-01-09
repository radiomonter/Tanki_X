namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class HandlerArgumentsDescription
    {
        public HandlerArgumentsDescription(IList<HandlerArgument> handlerArguments, ICollection<Type> additionalEventClasses, ICollection<Type> additionalComponentClasses)
        {
            this.HandlerArguments = handlerArguments;
            this.EventClasses = additionalEventClasses;
            this.ComponentClasses = additionalComponentClasses;
        }

        public IList<HandlerArgument> HandlerArguments { get; internal set; }

        public ICollection<Type> EventClasses { get; internal set; }

        public ICollection<Type> ComponentClasses { get; internal set; }
    }
}

