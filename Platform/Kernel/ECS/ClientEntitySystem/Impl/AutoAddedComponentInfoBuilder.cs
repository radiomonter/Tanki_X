namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class AutoAddedComponentInfoBuilder : AnnotationComponentInfoBuilder<AutoAddedComponentInfo>
    {
        public AutoAddedComponentInfoBuilder() : base(typeof(AutoAdded), typeof(AutoAddedComponentInfo))
        {
        }
    }
}

