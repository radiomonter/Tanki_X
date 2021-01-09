namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class HandlerStateListener : EntityListener
    {
        private HandlerCollector handlerCollector;

        public HandlerStateListener(HandlerCollector handlerCollector)
        {
            this.handlerCollector = handlerCollector;
        }

        public void OnEntityDeleted(Entity entity)
        {
        }

        public void OnNodeAdded(Entity entity, NodeDescription nodeDescription)
        {
            IEnumerator<Handler> enumerator = this.handlerCollector.GetHandlersWithoutContext(nodeDescription).GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.ChangeVersion();
            }
        }

        public void OnNodeRemoved(Entity entity, NodeDescription nodeDescription)
        {
            IEnumerator<Handler> enumerator = this.handlerCollector.GetHandlersWithoutContext(nodeDescription).GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.ChangeVersion();
            }
        }
    }
}

