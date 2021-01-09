using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientCommunicator.API;
using Tanks.Lobby.ClientCommunicator.Impl;

[SerialVersionUID(0x8d521801dfe59ddL)]
public interface GeneralChatTemplate : ChatTemplate, Template
{
    GeneralChatComponent generalChat();
}

