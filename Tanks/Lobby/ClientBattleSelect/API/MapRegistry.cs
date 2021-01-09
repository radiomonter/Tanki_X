namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;

    public interface MapRegistry
    {
        Map GetMap(Entity mapEntity);
    }
}

