namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class HandlerCallContextChangedException : Exception
    {
        public HandlerCallContextChangedException()
        {
        }

        public HandlerCallContextChangedException(Handler handler, HandlerArgument argument, Entity entity) : base(CreateMessage(handler, argument, entity))
        {
        }

        public HandlerCallContextChangedException(Handler handler, NodeClassInstanceDescription nodeDesc, Entity entity) : base(CreateMessage(handler, nodeDesc, entity))
        {
        }

        private static string CreateMessage(Handler handler, HandlerArgument argument, Entity entity)
        {
            object[] objArray1 = new object[] { "\nMethod: ", handler.GetHandlerName(), "\nNodeClass: ", argument.ClassInstanceDescription.NodeClass.Name, " Node: ", argument.NodeDescription };
            string str = string.Concat(objArray1);
            if (entity != null)
            {
                str = str + "\nEntity: " + (entity as EntityInternal).ToStringWithComponentsClasses();
            }
            return str;
        }

        private static string CreateMessage(Handler handler, NodeClassInstanceDescription nodeDesc, Entity entity)
        {
            object[] objArray1 = new object[6];
            objArray1[0] = "\nMethod: ";
            objArray1[1] = handler.GetHandlerName();
            objArray1[2] = "\nNodeClass: ";
            objArray1[3] = nodeDesc?.NodeClass.Name;
            object[] local1 = objArray1;
            local1[4] = " Node: ";
            local1[5] = nodeDesc?.NodeDescription;
            string str = string.Concat(local1);
            if (entity != null)
            {
                str = str + "\nEntity: " + (entity as EntityInternal).ToStringWithComponentsClasses();
            }
            return str;
        }
    }
}

