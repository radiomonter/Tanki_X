namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Battle.ClientCore.Impl;

    [Shared, SerialVersionUID(-7794607793680215703L)]
    public class BaseRailgunChargingShotEvent : TimeValidateEvent
    {
    }
}

