namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class BroadcastInvokeDataStorage
    {
        private List<HandlerBroadcastInvokeData> datas = new List<HandlerBroadcastInvokeData>(200);

        public void Add(Entity entity, ICollection<Handler> handlers)
        {
            if (handlers.Count != 0)
            {
                Collections.Enumerator<Handler> enumerator = Collections.GetEnumerator<Handler>(handlers);
                while (enumerator.MoveNext())
                {
                    Handler current = enumerator.Current;
                    HandlerBroadcastInvokeData item = new HandlerBroadcastInvokeData(current, entity);
                    this.datas.Add(item);
                }
            }
        }

        public void Remove(Entity entity)
        {
            for (int i = this.datas.Count - 1; i >= 0; i--)
            {
                HandlerBroadcastInvokeData data = this.datas[i];
                if (data.Entity.Equals(entity))
                {
                    this.datas.RemoveAt(i);
                }
            }
        }

        public void Remove(Entity entity, ICollection<Handler> handlers)
        {
            if (handlers.Count != 0)
            {
                Collections.Enumerator<Handler> enumerator = Collections.GetEnumerator<Handler>(handlers);
                while (enumerator.MoveNext())
                {
                    for (int i = this.datas.Count - 1; i >= 0; i--)
                    {
                        HandlerBroadcastInvokeData data = this.datas[i];
                        Handler current = enumerator.Current;
                        if (ReferenceEquals(data.Handler, current) && data.Entity.Equals(entity))
                        {
                            this.datas.RemoveAt(i);
                        }
                    }
                }
            }
        }

        public IList<HandlerBroadcastInvokeData> ContextInvokeDatas =>
            this.datas;
    }
}

