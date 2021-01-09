namespace Tanks.Lobby.ClientCommunicator.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Tanks.Lobby.ClientCommunicator.Impl;

    [SerialVersionUID(0x8d5321772cb0963L)]
    public interface PersonalChatTemplate : Template
    {
        ChatParticipantsComponent chatParticipants();
        [AutoAdded]
        PersonalChatComponent personalChat();
    }
}

