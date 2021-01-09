namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class EntityNode
    {
        public ArgumentNode argumentNode;
        public Entity entity;
        public object invokeArgument;
        public List<EntityNode> nextArgumentEntityNodes = new List<EntityNode>();

        public void Clear()
        {
            this.nextArgumentEntityNodes.Clear();
        }

        public void ConvertToOptional()
        {
            MethodInfo method = this.argumentNode.argument.ArgumentType.GetMethod("nullableOf");
            object[] parameters = new object[] { this.invokeArgument };
            this.invokeArgument = method.Invoke(null, parameters);
        }

        public EntityNode Init(ArgumentNode argumentNode, Entity entity)
        {
            this.entity = entity;
            this.argumentNode = argumentNode;
            this.nextArgumentEntityNodes.Clear();
            this.invokeArgument = null;
            return this;
        }

        public void PrepareInvokeArgument()
        {
            NodeClassInstanceDescription classInstanceDescription = this.argumentNode.argument.ClassInstanceDescription;
            this.invokeArgument = ((EntityInternal) this.entity).GetNode(classInstanceDescription);
        }
    }
}

