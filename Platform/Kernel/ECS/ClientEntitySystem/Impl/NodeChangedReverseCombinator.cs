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

    public class NodeChangedReverseCombinator
    {
        public bool Combine(HandlerInvokeGraph handlerInvokeGraph, Entity contextEntity, ICollection<NodeDescription> changedNodes)
        {
            ArgumentNode[] argumentNodes = handlerInvokeGraph.ArgumentNodes;
            ArgumentNode fromArgumentNode = null;
            for (int i = argumentNodes.Length - 1; i >= 0; i--)
            {
                ArgumentNode toArgumentNode = argumentNodes[i];
                if (!this.FillEntityNodes(fromArgumentNode, toArgumentNode, contextEntity, changedNodes))
                {
                    Handler handler = handlerInvokeGraph.Handler;
                    return false;
                }
                fromArgumentNode = toArgumentNode;
            }
            return true;
        }

        private void FillArgumentNodesByJoin(JoinType join, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode, Entity contextEntity)
        {
            for (int i = 0; i < fromArgumentNode.entityNodes.Count; i++)
            {
                this.FillEntityNodesByJoin(join, fromArgumentNode.entityNodes[i], toArgumentNode, contextEntity);
            }
        }

        public void FillEntityNodes(ArgumentNode argumentNode, Entity entity)
        {
            EntityNode node;
            argumentNode.filled = true;
            if (argumentNode.TryGetEntityNode(entity, out node))
            {
                argumentNode.entityNodes.Add(node);
            }
        }

        protected bool FillEntityNodes(ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode, Entity contextEntity, ICollection<NodeDescription> changedNodes)
        {
            Optional<JoinType> optional;
            bool flag = toArgumentNode.argument.Context && changedNodes.Contains(toArgumentNode.argument.NodeDescription);
            if (((fromArgumentNode != null) && fromArgumentNode.filled) && ((optional = fromArgumentNode.argument.JoinType).IsPresent() && !(optional.Get() is JoinAllType)))
            {
                this.FillArgumentNodesByJoin(optional.Get(), fromArgumentNode, toArgumentNode, !flag ? null : contextEntity);
                HandlerArgument argument = toArgumentNode.argument;
                return (argument.Collection || (argument.Optional || (toArgumentNode.entityNodes.Count > 0)));
            }
            if (flag)
            {
                this.FillEntityNodes(toArgumentNode, contextEntity);
            }
            return true;
        }

        protected void FillEntityNodesByJoin(JoinType join, EntityNode fromEntityNode, ArgumentNode toArgumentNode, Entity contextEntity)
        {
            toArgumentNode.filled = true;
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(join.GetEntities(Flow.Current.NodeCollector, toArgumentNode.argument.NodeDescription, fromEntityNode.entity));
            while (enumerator.MoveNext())
            {
                EntityNode node;
                Entity current = enumerator.Current;
                if (((contextEntity == null) || contextEntity.Equals(current)) && toArgumentNode.TryGetEntityNode(current, out node))
                {
                    node.nextArgumentEntityNodes.Add(fromEntityNode);
                    toArgumentNode.entityNodes.Add(node);
                }
            }
        }

        [Conditional("DEBUG")]
        private void ShowDebugSkipInfo(Handler handler, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode)
        {
            if (handler.SkipInfo)
            {
                Optional<JoinType> joinType = toArgumentNode.argument.JoinType;
                string message = SkipInfoBuilder.GetSkipReasonDetails(handler, fromArgumentNode, toArgumentNode, joinType);
                LoggerProvider.GetLogger(this).Warn(message);
            }
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }
    }
}

