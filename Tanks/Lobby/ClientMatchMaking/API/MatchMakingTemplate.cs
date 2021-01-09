namespace Tanks.Lobby.ClientMatchMaking.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x15baa224618L)]
    public interface MatchMakingTemplate : Template
    {
        [AutoAdded]
        MatchMakingComponent matchMaking();
    }
}

