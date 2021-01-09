namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class HandlerInvokeData
    {
        private int handlerVersion;
        private List<HandlerExecutor> handlerExecutors;
        private int illegalCombineIndex;

        public HandlerInvokeData()
        {
            this.handlerVersion = -1;
            this.handlerExecutors = new List<HandlerExecutor>();
        }

        public HandlerInvokeData(Platform.Kernel.ECS.ClientEntitySystem.Impl.Handler handler)
        {
            this.handlerVersion = -1;
            this.handlerExecutors = new List<HandlerExecutor>();
            this.Handler = handler;
            this.HandlerArguments = handler.HandlerArgumentsDescription.HandlerArguments;
        }

        private bool CheckMethodArgumentsIsActual(object[] args)
        {
            for (int i = 1; i < args.Length; i++)
            {
                object obj2 = args[i];
                Type objA = obj2.GetType();
                if (!ReferenceEquals(objA, typeof(Node)))
                {
                    if (obj2 is Node)
                    {
                        if (!this.CheckNodeIsActual((Node) obj2))
                        {
                            return false;
                        }
                    }
                    else if (obj2 is ICollection)
                    {
                        IEnumerator enumerator = (obj2 as ICollection).GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            if (!this.CheckNodeIsActual((Node) enumerator.Current))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool CheckNodeIsActual(Node node)
        {
            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(node.GetType(), null);
            return ((EntityImpl) node.Entity).NodeDescriptionStorage.Contains(orCreateNodeClassDescription.NodeDescription);
        }

        private bool CollectExecutors(HandlerInvokeGraph invokeGraph, EntityNode entityNode, int argumentIndex)
        {
            HandlerArgument argument = this.HandlerArguments[argumentIndex];
            if (argumentIndex == (this.HandlerArguments.Count - 1))
            {
                HandlerExecutor item = this.CreateExecutor();
                item.ArgumentForMethod[argumentIndex + 1] = entityNode.invokeArgument;
                this.handlerExecutors.Add(item);
                return true;
            }
            ArgumentNode argumentNode = invokeGraph.ArgumentNodes[argumentIndex + 1];
            List<EntityNode> source = (argument.Collection || argumentNode.linkBreak) ? argumentNode.entityNodes : entityNode.nextArgumentEntityNodes;
            int count = this.handlerExecutors.Count;
            bool flag = false;
            for (int i = 0; i < source.Count<EntityNode>(); i++)
            {
                if (this.CollectExecutors(invokeGraph, source[i], argumentIndex + 1))
                {
                    if (flag && !argumentNode.argument.Combine)
                    {
                        throw new IllegalCombineException(this.Handler, argumentNode);
                    }
                    flag = true;
                }
            }
            for (int j = count; j < this.handlerExecutors.Count; j++)
            {
                this.handlerExecutors[j].ArgumentForMethod[argumentIndex + 1] = entityNode.invokeArgument;
            }
            return flag;
        }

        protected virtual HandlerExecutor CreateExecutor() => 
            new HandlerExecutor(this.Handler, new object[this.HandlerArguments.Count + 1]);

        public HandlerInvokeData Init(Platform.Kernel.ECS.ClientEntitySystem.Impl.Handler handler)
        {
            this.handlerExecutors.Clear();
            this.handlerVersion = -1;
            this.Handler = handler;
            this.HandlerArguments = handler.HandlerArgumentsDescription.HandlerArguments;
            return this;
        }

        public virtual void Invoke(IList<HandlerInvokeData> otherInvokeArguments)
        {
            int count = this.handlerExecutors.Count;
            for (int i = 0; i < count; i++)
            {
                HandlerExecutor executor = this.handlerExecutors[i];
                if ((this.handlerVersion == this.Handler.Version) || this.CheckMethodArgumentsIsActual(executor.ArgumentForMethod))
                {
                    this.handlerExecutors[i].Execute();
                }
                else if (this.Handler.Mandatory)
                {
                    LoggerProvider.GetLogger<Flow>().Warn("Mandatory handler skiped by context change " + this.Handler);
                }
            }
        }

        public bool IsActual() => 
            (this.Handler != null) && (this.handlerVersion == this.Handler.Version);

        public bool Reuse(Event eventInstance)
        {
            if (!this.IsActual())
            {
                return false;
            }
            for (int i = 0; i < this.handlerExecutors.Count; i++)
            {
                this.handlerExecutors[i].SetEvent(eventInstance);
            }
            return true;
        }

        public virtual void Update(Event eventInstance, HandlerInvokeGraph invokeGraph)
        {
            this.handlerVersion = this.Handler.Version;
            this.handlerExecutors.Clear();
            ArgumentNode argumentNode = invokeGraph.ArgumentNodes[0];
            List<EntityNode> entityNodes = argumentNode.entityNodes;
            HandlerArgument argument = argumentNode.argument;
            bool flag = false;
            for (int i = 0; i < entityNodes.Count; i++)
            {
                if (this.CollectExecutors(invokeGraph, entityNodes[i], 0))
                {
                    if (!argument.Combine && flag)
                    {
                        throw new IllegalCombineException(this.Handler, argumentNode);
                    }
                    flag = true;
                }
            }
            if (!flag)
            {
                throw new InvalidInvokeGraphException(this.Handler);
            }
            for (int j = 0; j < this.handlerExecutors.Count; j++)
            {
                this.handlerExecutors[j].SetEvent(eventInstance);
            }
        }

        public virtual void UpdateForEmptyCall()
        {
            this.handlerVersion = this.Handler.Version;
            this.handlerExecutors.Clear();
        }

        public virtual void UpdateForEventOnlyArguments(Event eventInstance)
        {
            this.handlerVersion = this.Handler.Version;
            this.handlerExecutors.Clear();
            HandlerExecutor item = this.CreateExecutor();
            item.ArgumentForMethod[0] = eventInstance;
            this.handlerExecutors.Add(item);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.Handler Handler { get; private set; }

        public IList<HandlerArgument> HandlerArguments { get; private set; }
    }
}

