namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;

    public abstract class AbstractTemplateComponentConstructor : ComponentConstructor
    {
        protected AbstractTemplateComponentConstructor()
        {
        }

        protected internal abstract Component GetComponentInstance(ComponentDescription componentDescription, EntityInternal entity);
        public Component GetComponentInstance(Type componentType, EntityInternal entity)
        {
            ComponentDescription componentDescription = entity.TemplateAccessor.Get().TemplateDescription.GetComponentDescription(componentType);
            return this.GetComponentInstance(componentDescription, entity);
        }

        protected abstract bool IsAcceptable(ComponentDescription componentDescription, EntityInternal entity);
        public bool IsAcceptable(Type componentType, EntityInternal entity)
        {
            Optional<TemplateAccessor> templateAccessor = entity.TemplateAccessor;
            if (!templateAccessor.IsPresent())
            {
                return false;
            }
            TemplateDescription templateDescription = templateAccessor.Get().TemplateDescription;
            if (!templateDescription.IsComponentDescriptionPresent(componentType))
            {
                return false;
            }
            ComponentDescription componentDescription = templateDescription.GetComponentDescription(componentType);
            return this.IsAcceptable(componentDescription, entity);
        }
    }
}

