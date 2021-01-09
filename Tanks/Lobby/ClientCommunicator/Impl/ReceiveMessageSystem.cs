namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class ReceiveMessageSystem : ECSSystem
    {
        private string systemMessageAuthorKey = "ee469a5a-5894-4a5e-8c59-414e614cfb22";

        [OnEventFire]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, Node any, [JoinAll] Overall overallChannel)
        {
            overallChannel.chatChannel.AddMessage(e.Message);
        }

        [OnEventFire]
        public void ShowReceivedMessage(ChatMessageReceivedEvent e, ChatNode chatNode, [JoinAll] SelfUserNode selfNode)
        {
            if (e.SystemMessage || !selfNode.blackList.BlockedUsers.Contains(e.UserId))
            {
                ChatMessage message = new ChatMessage {
                    Author = !e.SystemMessage ? e.UserUid : LocalizationUtils.Localize(this.systemMessageAuthorKey),
                    AvatarId = e.UserAvatarId,
                    Message = e.Message,
                    Time = DateTime.Now.ToString("HH:mm"),
                    System = e.SystemMessage,
                    Self = (e.UserId == selfNode.Entity.Id) && !e.SystemMessage,
                    ChatType = chatNode.chatChannel.ChatType,
                    ChatId = chatNode.Entity.Id
                };
                chatNode.chatChannel.AddMessage(message);
                base.ScheduleEvent(new RecievedLobbyChatMessageEvent(message), chatNode);
            }
        }

        public class ChatNode : Node
        {
            public ChatChannelComponent chatChannel;
        }

        public class Overall : ReceiveMessageSystem.ChatNode
        {
            public OverallChannelComponent overallChannel;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserComponent user;
            public BlackListComponent blackList;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserUidComponent userUid;
        }
    }
}

