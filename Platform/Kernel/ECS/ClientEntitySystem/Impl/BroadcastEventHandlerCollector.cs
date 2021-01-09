namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class BroadcastEventHandlerCollector : EntityListener
    {
        private HandlerCollector handlerCollector;
        private Dictionary<Type, BroadcastInvokeDataStorage> handlersByType = new Dictionary<Type, BroadcastInvokeDataStorage>();

        public BroadcastEventHandlerCollector(HandlerCollector handlerCollector)
        {
            this.handlerCollector = handlerCollector;
        }

        public IList<HandlerBroadcastInvokeData> GetHandlers(Type handlerType) => 
            this.handlersByType[handlerType].ContextInvokeDatas;

        public void OnEntityDeleted(Entity entity)
        {
            Collections.Enumerator<BroadcastInvokeDataStorage> enumerator = Collections.GetEnumerator<BroadcastInvokeDataStorage>(this.handlersByType.Values);
            while (enumerator.MoveNext())
            {
                enumerator.Current.Remove(entity);
            }
        }

        public void OnNodeAdded(Entity entity, NodeDescription node)
        {
            Collections.Enumerator<KeyValuePair<Type, BroadcastInvokeDataStorage>> enumerator = Collections.GetEnumerator<KeyValuePair<Type, BroadcastInvokeDataStorage>>(this.handlersByType);
            while (enumerator.MoveNext())
            {
                KeyValuePair<Type, BroadcastInvokeDataStorage> current = enumerator.Current;
                ICollection<Handler> handlers = this.handlerCollector.GetHandlers(current.Key, node);
                KeyValuePair<Type, BroadcastInvokeDataStorage> pair2 = enumerator.Current;
                pair2.Value.Add(entity, handlers);
            }
        }

        public void OnNodeRemoved(Entity entity, NodeDescription node)
        {
            Collections.Enumerator<KeyValuePair<Type, BroadcastInvokeDataStorage>> enumerator = Collections.GetEnumerator<KeyValuePair<Type, BroadcastInvokeDataStorage>>(this.handlersByType);
            while (enumerator.MoveNext())
            {
                KeyValuePair<Type, BroadcastInvokeDataStorage> current = enumerator.Current;
                ICollection<Handler> handlers = this.handlerCollector.GetHandlers(current.Key, node);
                KeyValuePair<Type, BroadcastInvokeDataStorage> pair2 = enumerator.Current;
                pair2.Value.Remove(entity, handlers);
            }
        }

        public void Register(Type handlerType)
        {
            this.handlersByType[handlerType] = new BroadcastInvokeDataStorage();
        }
    }
}

