namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x14d4c4e05c7L)]
    public class AddChildGroupToGroupEvent : Event
    {
        private readonly GroupComponent groupComponent;
        private readonly Type childGroupClass;

        public AddChildGroupToGroupEvent(GroupComponent groupComponent, Type childGroupClass)
        {
            this.groupComponent = groupComponent;
            this.childGroupClass = childGroupClass;
        }

        public GroupComponent Group =>
            this.groupComponent;

        public Type ChildGroupClass =>
            this.childGroupClass;
    }
}

