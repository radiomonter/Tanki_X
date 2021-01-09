namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Collections.Generic;

    public class MandatoryException : Exception
    {
        public MandatoryException(string reason) : base(reason)
        {
        }

        public MandatoryException(ICollection<Entity> contexEntities, Handler handler) : base(new SkipLog(contexEntities, handler).ToString())
        {
        }

        public MandatoryException(ICollection<Entity> contexEntities, Handler handler, HandlerArgument handlerArgument) : base(new SkipLog(contexEntities, handler).ToString())
        {
        }
    }
}

