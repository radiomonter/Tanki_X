namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [Shared, SerialVersionUID(0x15f0bfefcebL)]
    public class LeaveBattleBeforeItEndEvent : Event
    {
    }
}

