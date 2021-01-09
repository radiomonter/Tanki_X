namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class NoServerConnectionEvent : Event
    {
        public string ErrorMessage { get; set; }
    }
}

