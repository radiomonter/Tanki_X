namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class EntityNotExistsException : Exception
    {
        public EntityNotExistsException(long id) : base("id=" + id)
        {
        }
    }
}

