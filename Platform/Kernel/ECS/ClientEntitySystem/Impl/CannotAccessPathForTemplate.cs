namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class CannotAccessPathForTemplate : Exception
    {
        public CannotAccessPathForTemplate(Type templateClass) : base(templateClass.FullName)
        {
        }
    }
}

