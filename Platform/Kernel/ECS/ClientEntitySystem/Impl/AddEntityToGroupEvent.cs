namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class AddEntityToGroupEvent : Event
    {
        private GroupComponent groupComponent;

        public AddEntityToGroupEvent(GroupComponent group)
        {
            this.groupComponent = group;
        }

        public GroupComponent Group =>
            this.groupComponent;
    }
}

