namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x154a8aaf7fcL)]
    public interface ServerShutdownTemplate : Template
    {
        ServerShutdownComponent serverShutdown();
    }
}

