namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Library.ClientDataStructures.API;

    public interface LogPart
    {
        Optional<string> GetSkipReason();
    }
}

