namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class NodeChangedEventMaker
    {
        private readonly Event eventInstance;
        private readonly Type _fireHandlerType;
        private readonly Type _completeHandlerType;
        private readonly HandlerCollector handlerCollector;

        public NodeChangedEventMaker(Event eventInstance, Type fireHandlerType, Type completeHandlerType, HandlerCollector handlerCollector)
        {
            this.eventInstance = eventInstance;
            this._fireHandlerType = fireHandlerType;
            this._completeHandlerType = completeHandlerType;
            this.handlerCollector = handlerCollector;
        }

        public static ICollection<Handler> CollectHandlers(HandlerCollector handlerCollector, Type handlerType, ICollection<NodeDescription> changedNodes)
        {
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator<NodeDescription>(changedNodes);
            enumerator.MoveNext();
            ICollection<Handler> handlers = handlerCollector.GetHandlers(handlerType, enumerator.Current);
            if (!enumerator.MoveNext())
            {
                return handlers;
            }
            List<Handler> list = new List<Handler>(handlers);
            while (true)
            {
                list.AddRange(handlerCollector.GetHandlers(handlerType, enumerator.Current));
                if (!enumerator.MoveNext())
                {
                    return list;
                }
            }
        }

        private void Make(Entity entity, ICollection<NodeDescription> changedNodes)
        {
            if (changedNodes.Count != 0)
            {
                ICollection<Handler> fireHandlers = CollectHandlers(this.handlerCollector, this._fireHandlerType, changedNodes);
                Flow.Current.TryInvoke(fireHandlers, CollectHandlers(this.handlerCollector, this._completeHandlerType, changedNodes), this.eventInstance, entity, changedNodes);
            }
        }

        public void MakeIfNeed(Entity entity, Type componentType)
        {
            ICollection<NodeDescription> nodeDescriptions = ((EntityInternal) entity).NodeDescriptionStorage.GetNodeDescriptions(componentType);
            this.Make(entity, nodeDescriptions);
        }
    }
}

