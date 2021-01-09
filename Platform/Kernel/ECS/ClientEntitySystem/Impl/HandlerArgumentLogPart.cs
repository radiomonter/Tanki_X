namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HandlerArgumentLogPart : LogPart
    {
        private readonly ICollection<Entity> entities;
        private readonly HandlerArgument handlerArgument;
        private readonly IDictionary<Entity, ICollection<Type>> missingComponentsByEntity = new Dictionary<Entity, ICollection<Type>>();

        public HandlerArgumentLogPart(HandlerArgument handlerArgument, ICollection<Entity> entities)
        {
            this.handlerArgument = handlerArgument;
            this.entities = entities;
            if (!handlerArgument.Collection)
            {
                this.FindMostSuitableEntities(handlerArgument, entities);
            }
        }

        private void FindMostSuitableEntities(HandlerArgument handlerArgument, ICollection<Entity> entities)
        {
            int count = 0x7fffffff;
            foreach (Entity entity in entities)
            {
                ICollection<Type> missingComponents = GetMissingComponents(entity, handlerArgument.NodeDescription);
                if ((missingComponents.Count != 0) && (missingComponents.Count < count))
                {
                    count = missingComponents.Count;
                    continue;
                }
                if (missingComponents.Count == 0)
                {
                    count = 0;
                    break;
                }
            }
            if (count > 0)
            {
                foreach (Entity entity2 in entities)
                {
                    ICollection<Type> missingComponents = GetMissingComponents(entity2, handlerArgument.NodeDescription);
                    if (missingComponents.Count == count)
                    {
                        this.missingComponentsByEntity.Add(entity2, missingComponents);
                    }
                }
            }
        }

        private static ICollection<Type> GetMissingComponents(Entity entity, NodeDescription nodeDescription)
        {
            List<Type> list = new List<Type>();
            foreach (Type type in nodeDescription.Components)
            {
                if (!entity.HasComponent(type))
                {
                    list.Add(type);
                }
            }
            return list;
        }

        public virtual Optional<string> GetSkipReason() => 
            (this.entities.Count != 0) ? ((this.missingComponentsByEntity.Count != 0) ? Optional<string>.of(this.MessageForMissingNode) : Optional<string>.empty()) : Optional<string>.of(this.MessageForNoEntities);

        private string MessageForNoEntities =>
            "\tNo entity for node=" + this.NodeClassName;

        private string MessageForMissingNode
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append($"	Missing node={this.NodeClassName}
	");
                foreach (Entity entity in this.missingComponentsByEntity.Keys)
                {
                    ICollection<Type> components = this.missingComponentsByEntity[entity];
                    string str = $"	Entity={EcsToStringUtil.ToString(entity)}; Missing components={EcsToStringUtil.ToString(components)}, parameter=[{this.handlerArgument.NodeNumber + 1}]";
                    builder.Append(str);
                    builder.Append("\n\t");
                }
                return builder.ToString();
            }
        }

        private string NodeClassName =>
            this.handlerArgument.ClassInstanceDescription.NodeClass.Name;
    }
}

