namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    [JoinBy(typeof(PreviewGroupComponent)), AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, Inherited=false)]
    public class JoinByPreviewAttribute : Attribute
    {
    }
}

