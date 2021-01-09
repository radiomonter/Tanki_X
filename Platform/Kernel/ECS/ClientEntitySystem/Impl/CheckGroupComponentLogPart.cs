namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CheckGroupComponentLogPart : LogPart
    {
        private readonly Type groupComponent;
        private readonly IList<Entity> entitiesWithMissingGroupComponentByEntity = Collections.EmptyList<Entity>();

        public CheckGroupComponentLogPart(HandlerArgument handlerArgument, ICollection<Entity> entities)
        {
            Optional<Type> contextComponent = handlerArgument.JoinType.Get().ContextComponent;
            if (contextComponent.IsPresent())
            {
                this.groupComponent = contextComponent.Get();
                this.entitiesWithMissingGroupComponentByEntity = new List<Entity>();
                foreach (Entity entity in entities)
                {
                    if (entity.HasComponent(this.groupComponent))
                    {
                        break;
                    }
                    this.entitiesWithMissingGroupComponentByEntity.Add(entity);
                }
            }
        }

        private string GetMessageForGroupComponent()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"	Missing group component={this.groupComponent.Name}
	");
            foreach (Entity entity in this.entitiesWithMissingGroupComponentByEntity)
            {
                builder.Append($"	Entity={EcsToStringUtil.ToString(entity)}");
                builder.Append("\n\t");
            }
            return builder.ToString();
        }

        public Optional<string> GetSkipReason() => 
            (this.entitiesWithMissingGroupComponentByEntity.Count != 0) ? Optional<string>.of(this.GetMessageForGroupComponent()) : Optional<string>.empty();
    }
}

