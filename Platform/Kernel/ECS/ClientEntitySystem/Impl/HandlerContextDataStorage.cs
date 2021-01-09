namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class HandlerContextDataStorage : EntityListener
    {
        private Dictionary<HandlerContextDescription, HandlerInvokeData> invokeDataByDescription = new Dictionary<HandlerContextDescription, HandlerInvokeData>(100);
        private MultiMap<long, HandlerContextDescription> contextDescriptionsByEntity = new MultiMap<long, HandlerContextDescription>();

        public HandlerInvokeData GetInvokeData(Handler handler, Type eventType, ICollection<Entity> entities)
        {
            HandlerInvokeData data;
            if ((entities.Count == 0) || (entities.Count > 2))
            {
                return Cache.flowInvokeData.GetInstance().Init(handler);
            }
            long id = 0L;
            long id = 0L;
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(entities);
            while (enumerator.MoveNext())
            {
                if (id == 0L)
                {
                    id = enumerator.Current.Id;
                    continue;
                }
                id = enumerator.Current.Id;
            }
            HandlerContextDescription key = new HandlerContextDescription(handler, eventType, id, id);
            if (!this.invokeDataByDescription.TryGetValue(key, out data))
            {
                data = new HandlerInvokeData(handler);
                this.invokeDataByDescription.Add(key, data);
                enumerator = Collections.GetEnumerator<Entity>(entities);
                while (enumerator.MoveNext())
                {
                    this.contextDescriptionsByEntity.Add(enumerator.Current.Id, key);
                }
            }
            return data;
        }

        public void OnEntityDeleted(Entity entity)
        {
            HashSet<HandlerContextDescription> set;
            if (this.contextDescriptionsByEntity.TryGetValue(entity.Id, out set))
            {
                HashSet<HandlerContextDescription>.Enumerator enumerator = set.GetEnumerator();
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        this.contextDescriptionsByEntity.Remove(entity.Id);
                        break;
                    }
                    this.invokeDataByDescription.Remove(enumerator.Current);
                }
            }
        }

        public void OnNodeAdded(Entity entity, NodeDescription nodeDescription)
        {
        }

        public void OnNodeRemoved(Entity entity, NodeDescription nodeDescription)
        {
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        public struct HandlerContextDescription : IEquatable<HandlerContextDataStorage.HandlerContextDescription>
        {
            public Handler handler;
            public long e1;
            public long e2;
            public Type eventType;
            public HandlerContextDescription(Handler handler, Type eventType, long e1, long e2)
            {
                this.handler = handler;
                this.e1 = e1;
                this.e2 = e2;
                this.eventType = eventType;
            }

            public bool Equals(HandlerContextDataStorage.HandlerContextDescription other) => 
                (ReferenceEquals(this.handler, other.handler) && (ReferenceEquals(this.eventType, other.eventType) && ((this.e1 == other.e1) || (this.e1 == other.e2)))) && ((this.e2 == other.e1) || (this.e2 == other.e2));

            public override int GetHashCode() => 
                0x1f * ((((int) (this.e1 + this.e2)) + this.handler.GetHashCode()) ^ this.eventType.GetHashCode());
        }
    }
}

