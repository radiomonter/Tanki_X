namespace Platform.Library.ClientUnityIntegration.API
{
    using System;

    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string name) : base($"Entity already exists, name = {name}")
        {
        }
    }
}

