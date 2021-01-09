namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class EntityByIdNotFoundException : Exception
    {
        public EntityByIdNotFoundException(long id) : base("id=" + id)
        {
        }
    }
}

