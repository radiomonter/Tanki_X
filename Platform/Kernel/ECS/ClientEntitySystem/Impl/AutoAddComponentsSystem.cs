namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AutoAddComponentsSystem : ECSSystem
    {
        private void AutoAddComponents(EntityInternal newEntity, TemplateDescription templateDescription)
        {
            foreach (Type type in templateDescription.GetAutoAddedComponentTypes())
            {
                Component component = !typeof(GroupComponent).IsAssignableFrom(type) ? newEntity.CreateNewComponentInstance(type) : GroupRegistry.FindOrCreateGroup(type, newEntity.Id);
                newEntity.AddComponent(component);
            }
        }

        private void AutoAddComponentsIfNeed(Node any)
        {
            EntityInternal entity = (EntityInternal) any.Entity;
            if (entity.TemplateAccessor.IsPresent())
            {
                this.AutoAddComponents(entity, entity.TemplateAccessor.Get().TemplateDescription);
            }
            base.ScheduleEvent<ComponentsAutoAddedEvent>(entity);
        }

        [OnEventFire]
        public void AutoAddComponentsIfNeedOnLoadedEntity(NodeAddedEvent e, SingleNode<SharedEntityComponent> any)
        {
            this.AutoAddComponentsIfNeed(any);
        }

        [OnEventFire]
        public void AutoAddComponentsIfNeedOnNewEntity(NodeAddedEvent e, SingleNode<NewEntityComponent> any)
        {
            this.AutoAddComponentsIfNeed(any);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.Impl.GroupRegistry GroupRegistry { get; set; }
    }
}

