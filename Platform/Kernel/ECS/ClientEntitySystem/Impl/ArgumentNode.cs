namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ArgumentNode
    {
        public bool linkBreak;
        public bool filled;
        public HandlerArgument argument;
        public List<EntityNode> entityNodes;

        public ArgumentNode(HandlerArgument argument)
        {
            this.argument = argument;
            this.entityNodes = new List<EntityNode>();
        }

        public void Clear()
        {
            this.entityNodes.Clear();
            this.filled = false;
            this.linkBreak = this.argument.SelectAll || this.argument.Collection;
        }

        public void ConvertToCollection()
        {
            this.entityNodes.Clear();
            EntityNode item = Cache.entityNode.GetInstance().Init(this, null);
            item.invokeArgument = this.GetCollection();
            this.entityNodes.Add(item);
        }

        public void ConvertToOptional()
        {
            if (this.IsEmpty())
            {
                this.linkBreak = true;
                EntityNode item = Cache.entityNode.GetInstance().Init(this, null);
                item.ConvertToOptional();
                this.entityNodes.Add(item);
            }
            else
            {
                for (int i = 0; i < this.entityNodes.Count; i++)
                {
                    this.entityNodes[i].ConvertToOptional();
                }
            }
        }

        private IList GetCollection()
        {
            IList genericListInstance = Cache.GetGenericListInstance(this.argument.ClassInstanceDescription.NodeClass, this.entityNodes.Count);
            for (int i = 0; i < this.entityNodes.Count; i++)
            {
                genericListInstance.Add(this.entityNodes[i].invokeArgument);
            }
            return genericListInstance;
        }

        public ArgumentNode Init()
        {
            this.Clear();
            return this;
        }

        public bool IsEmpty() => 
            this.entityNodes.Count == 0;

        public void PrepareInvokeArguments()
        {
            for (int i = 0; i < this.entityNodes.Count; i++)
            {
                this.entityNodes[i].PrepareInvokeArgument();
            }
        }

        public bool TryGetEntityNode(Entity entity, out EntityNode entityNode)
        {
            NodeClassInstanceDescription classInstanceDescription = this.argument.ClassInstanceDescription;
            entityNode = null;
            if (!((EntityInternal) entity).CanCast(classInstanceDescription.NodeDescription))
            {
                return false;
            }
            entityNode = Cache.entityNode.GetInstance().Init(this, entity);
            return true;
        }

        [Inject]
        public static FlowInstancesCache Cache { get; set; }
    }
}

