namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class HandlerArgumetCombinator
    {
        public virtual bool Combine(HandlerInvokeGraph handlerInvokeGraph, ICollection<Entity> contextEntities)
        {
            ArgumentNode[] argumentNodes = handlerInvokeGraph.ArgumentNodes;
            ArgumentNode fromArgumentNode = null;
            Handler handler = handlerInvokeGraph.Handler;
            for (int i = 0; i < argumentNodes.Length; i++)
            {
                ArgumentNode toArgumentNode = argumentNodes[i];
                if (!this.FillEntityNodes(contextEntities, fromArgumentNode, toArgumentNode))
                {
                    if (handler.Mandatory || toArgumentNode.argument.Mandatory)
                    {
                        throw new MandatoryException(SkipInfoBuilder.GetSkipReasonDetails(handler, fromArgumentNode, toArgumentNode, toArgumentNode.argument.JoinType));
                    }
                    return false;
                }
                fromArgumentNode = toArgumentNode;
            }
            this.Finalize(handlerInvokeGraph);
            return true;
        }

        private void FillArgumentNodesByJoin(JoinType join, ICollection<Entity> contextEntities, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode)
        {
            for (int i = 0; i < fromArgumentNode.entityNodes.Count; i++)
            {
                this.FillEntityNodesByJoin(join, contextEntities, fromArgumentNode.entityNodes[i], toArgumentNode);
            }
        }

        public void FillEntityNodes(ArgumentNode argumentNode, ICollection<Entity> entities)
        {
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(entities);
            while (enumerator.MoveNext())
            {
                EntityNode node;
                Entity current = enumerator.Current;
                if (argumentNode.TryGetEntityNode(current, out node))
                {
                    argumentNode.entityNodes.Add(node);
                }
            }
        }

        private bool FillEntityNodes(ICollection<Entity> contextEntities, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode)
        {
            HandlerArgument argument = toArgumentNode.argument;
            if (!toArgumentNode.filled)
            {
                Optional<JoinType> joinType = argument.JoinType;
                if (argument.Context && argument.SelectAll)
                {
                    if (contextEntities != null)
                    {
                        this.FillEntityNodes(toArgumentNode, contextEntities);
                    }
                    else
                    {
                        this.FillEntityNodesBySelectAll(toArgumentNode);
                    }
                }
                else if (joinType.IsPresent())
                {
                    if (joinType.Get() is JoinAllType)
                    {
                        this.FillEntityNodesBySelectAll(toArgumentNode);
                    }
                    else
                    {
                        this.FillArgumentNodesByJoin(joinType.Get(), contextEntities, fromArgumentNode, toArgumentNode);
                    }
                }
            }
            return ((argument.Collection || argument.Optional) || (toArgumentNode.entityNodes.Count > 0));
        }

        private void FillEntityNodesByJoin(JoinType join, ICollection<Entity> contextEntities, EntityNode fromEntityNode, ArgumentNode toArgumentNode)
        {
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(join.GetEntities(Flow.Current.NodeCollector, toArgumentNode.argument.NodeDescription, fromEntityNode.entity));
            while (enumerator.MoveNext())
            {
                EntityNode node;
                Entity current = enumerator.Current;
                if (!this.FilterByContext(toArgumentNode.argument, current, contextEntities) && toArgumentNode.TryGetEntityNode(current, out node))
                {
                    toArgumentNode.entityNodes.Add(node);
                    fromEntityNode.nextArgumentEntityNodes.Add(node);
                }
            }
        }

        private void FillEntityNodesBySelectAll(ArgumentNode argumentNode)
        {
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(Flow.Current.NodeCollector.GetEntities(argumentNode.argument.NodeDescription));
            while (enumerator.MoveNext())
            {
                EntityNode node;
                Entity current = enumerator.Current;
                if (argumentNode.TryGetEntityNode(current, out node))
                {
                    argumentNode.entityNodes.Add(node);
                }
            }
        }

        private bool FilterByContext(HandlerArgument argument, Entity entity, ICollection<Entity> contextEntities) => 
            (argument.Context && (contextEntities != null)) && !contextEntities.Contains(entity);

        private void Finalize(HandlerInvokeGraph handlerInvokeGraph)
        {
            foreach (ArgumentNode node in handlerInvokeGraph.ArgumentNodes)
            {
                node.PrepareInvokeArguments();
                if (node.argument.Collection)
                {
                    node.ConvertToCollection();
                }
                else if (node.argument.Optional)
                {
                    node.ConvertToOptional();
                }
            }
        }

        [Conditional("DEBUG")]
        private void ShowDebugSkipInfo(Handler handler, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode)
        {
            if (handler.SkipInfo)
            {
                Optional<JoinType> join = (fromArgumentNode == null) ? Optional<JoinType>.empty() : fromArgumentNode.argument.JoinType;
                string message = SkipInfoBuilder.GetSkipReasonDetails(handler, fromArgumentNode, toArgumentNode, join);
                LoggerProvider.GetLogger(this).Warn(message);
            }
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }
    }
}

