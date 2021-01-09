namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class CreateChatSystem : ECSSystem
    {
        [OnEventComplete]
        public void CloseChannel(CloseChannelEvent e, ActivePersonalChatNode channel, [JoinAll] SingleNode<GeneralChatComponent> general)
        {
            base.ScheduleEvent(new SelectChannelEvent(), general);
        }

        [OnEventComplete]
        public void CloseChannel(CloseChannelEvent e, VisiblePersonalChatNode channel, [JoinAll] SingleNode<GeneralChatComponent> general)
        {
            GameObject tab = channel.chatChannelUI.Tab;
            if (tab != null)
            {
                Object.DestroyImmediate(tab);
            }
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, SingleNode<CustomChatComponent> chat, SingleNode<ChatDialogComponent> dialog)
        {
            this.CreateChat(chat.Entity, chat.component.ChatType, dialog, true);
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, SingleNode<GeneralChatComponent> chat, SingleNode<ChatDialogComponent> dialog)
        {
            this.CreateChat(chat.Entity, chat.component.ChatType, dialog, true);
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, SingleNode<OverallChannelComponent> chat, SingleNode<ChatDialogComponent> dialog)
        {
            this.CreateChat(chat.Entity, chat.component.ChatType, dialog, false);
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, SingleNode<SquadChatComponent> chat, SingleNode<ChatDialogComponent> dialog)
        {
            this.CreateChat(chat.Entity, chat.component.ChatType, dialog, true);
        }

        public EntityBehaviour CreateChat(Entity entity, ChatType chatType, SingleNode<ChatDialogComponent> dialog, bool select = true)
        {
            EntityBehaviour behaviour = dialog.component.CreateChatChannel(chatType);
            behaviour.BuildEntity(entity);
            if (!behaviour.Entity.HasComponent<ChatChannelComponent>())
            {
                behaviour.Entity.AddComponent(new ChatChannelComponent(chatType));
            }
            if (select)
            {
                base.ScheduleEvent(new SelectChannelEvent(), entity);
            }
            return behaviour;
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, [Combine] PersonalChatNode chat, SingleNode<ChatDialogComponent> dialog, [JoinAll] SelfUser selfUser)
        {
            if (selfUser.personalChatOwner.ChatsIs.Contains(chat.Entity.Id))
            {
                this.CreatePersonalChat(chat, selfUser, dialog, true);
            }
            else if (!chat.Entity.HasComponent<ChatChannelComponent>())
            {
                chat.Entity.AddComponent(new ChatChannelComponent(chat.personalChat.ChatType));
            }
        }

        [OnEventFire]
        public void CreatePersonalChannelOnRecievedMessage(RecievedLobbyChatMessageEvent e, HiddenPersonalChatNode channel, [JoinAll] SingleNode<ChatDialogComponent> dialog, [JoinAll] SelfUser selfUser)
        {
            if (!e.Message.System)
            {
                this.CreatePersonalChat(channel, selfUser, dialog, false);
            }
        }

        private void CreatePersonalChat(PersonalChatNode chat, SelfUser selfUser, SingleNode<ChatDialogComponent> dialog, bool select = true)
        {
            <CreatePersonalChat>c__AnonStorey1 storey = new <CreatePersonalChat>c__AnonStorey1 {
                selfUser = selfUser
            };
            EntityBehaviour behaviour = this.CreateChat(chat.Entity, chat.personalChat.ChatType, dialog, false);
            Entity entity = chat.chatParticipants.Users.FirstOrDefault<Entity>(new Func<Entity, bool>(storey.<>m__0));
            if (entity == null)
            {
                object[] args = new object[] { storey.selfUser };
                Debug.LogWarningFormat("CreatePersonalChat self {0}, other = null", args);
            }
            else if (!entity.HasComponent<UserAvatarComponent>())
            {
                object[] args = new object[] { storey.selfUser, entity };
                Debug.LogWarningFormat("CreatePersonalChat self {0}, other {1} = avatar not found", args);
            }
            else
            {
                string id = entity.GetComponent<UserAvatarComponent>().Id;
                behaviour.GetComponent<ChatChannelUIComponent>().SetIcon(id);
                if (select)
                {
                    base.ScheduleEvent(new SelectChannelEvent(), behaviour.GetComponent<EntityBehaviour>().Entity);
                }
                object[] args = new object[] { chat.chatParticipants.Users.First<Entity>(new Func<Entity, bool>(storey.<>m__1)).Id };
                Debug.LogFormat("chat user id: {0}", args);
            }
        }

        [OnEventComplete]
        public void onRemoveChat(NodeRemoveEvent e, VisibleNotGeneralChatNode chat, [JoinAll] SingleNode<ChatDialogComponent> dialog, [JoinAll] SingleNode<GeneralChatComponent> general)
        {
            GameObject tab = chat.chatChannelUI.Tab;
            if (tab != null)
            {
                Object.Destroy(tab);
            }
        }

        [OnEventFire]
        public void OpenPersonalChannel(OpenPersonalChannelEvent e, SelfUser selfUser, Optional<SingleNode<ChatComponent>> contextChat, [JoinAll] ICollection<PersonalChatNode> chats, [JoinAll] SingleNode<ChatDialogComponent> dialog)
        {
            <OpenPersonalChannel>c__AnonStorey0 storey = new <OpenPersonalChannel>c__AnonStorey0 {
                e = e
            };
            PersonalChatNode node = chats.FirstOrDefault<PersonalChatNode>(new Func<PersonalChatNode, bool>(storey.<>m__0));
            if (node != null)
            {
                if (node.Entity.HasComponent<ChatChannelUIComponent>())
                {
                    base.ScheduleEvent(new SelectChannelEvent(), node);
                }
                else
                {
                    this.CreatePersonalChat(node, selfUser, dialog, true);
                }
            }
            else
            {
                CreatePrivateChatEvent eventInstance = new CreatePrivateChatEvent {
                    UserUid = storey.e.UserUid
                };
                EventBuilder builder = base.NewEvent(eventInstance);
                builder.Attach(selfUser);
                if (contextChat.IsPresent())
                {
                    builder.Attach(contextChat.Get());
                }
                builder.Schedule();
            }
        }

        [OnEventComplete]
        public void RemovePersonal(NodeRemoveEvent e, NotGeneralChatNode chat, [JoinAll] SingleNode<ChatDialogComponent> dialog, [JoinAll] GeneralChatUINode general)
        {
            if (!general.Entity.HasComponent<ActiveChannelComponent>() && chat.Entity.HasComponent<ActiveChannelComponent>())
            {
                base.ScheduleEvent(new SelectChannelEvent(), general);
            }
        }

        [OnEventComplete]
        public void SetPersonalChatName(NodeAddedEvent e, [Combine] UserNode user, [Combine] PersonalChatUINode chat, [JoinAll] SingleNode<ChatDialogComponent> dialog, [JoinAll] SelfUser selfUser)
        {
            if (!selfUser.Entity.Id.Equals(user.Entity.Id) && chat.chatParticipants.Users.Contains(user.Entity))
            {
                dialog.component.ChangeName(chat.chatChannelUI.gameObject, chat.personalChat.ChatType, user.userUid.Uid);
                if (chat.Entity.HasComponent<ActiveChannelComponent>())
                {
                    dialog.component.SetHeader(user.userAvatar.Id, chat.chatChannelUI.Name, true);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <CreatePersonalChat>c__AnonStorey1
        {
            internal CreateChatSystem.SelfUser selfUser;

            internal bool <>m__0(Entity x) => 
                !ReferenceEquals(x, this.selfUser.Entity);

            internal bool <>m__1(Entity x) => 
                !x.Id.Equals(this.selfUser.Entity.Id);
        }

        [CompilerGenerated]
        private sealed class <OpenPersonalChannel>c__AnonStorey0
        {
            internal OpenPersonalChannelEvent e;

            internal bool <>m__0(CreateChatSystem.PersonalChatNode chat) => 
                chat.chatParticipants.Users.Any<Entity>(user => user.HasComponent<UserComponent>() && user.GetComponent<UserUidComponent>().Uid.Equals(this.e.UserUid));

            internal bool <>m__1(Entity user) => 
                user.HasComponent<UserComponent>() && user.GetComponent<UserUidComponent>().Uid.Equals(this.e.UserUid);
        }

        public class ActiveNotGeneralChatNode : CreateChatSystem.NotGeneralChatNode
        {
            public ActiveChannelComponent activeChannel;
        }

        public class ActivePersonalChatNode : CreateChatSystem.PersonalChatNode
        {
            public ActiveChannelComponent activeChannel;
        }

        public class GeneralChatUINode : Node
        {
            public GeneralChatComponent generalChat;
            public ChatChannelUIComponent chatChannelUI;
        }

        [Not(typeof(ChatChannelUIComponent))]
        public class HiddenPersonalChatNode : CreateChatSystem.PersonalChatNode
        {
            public ChatChannelComponent chatChannel;
        }

        public class NotGeneralChatNode : Node
        {
            public ChatParticipantsComponent chatParticipants;
        }

        public class PersonalChatNode : Node
        {
            public PersonalChatComponent personalChat;
            public ChatParticipantsComponent chatParticipants;
        }

        public class PersonalChatUINode : CreateChatSystem.PersonalChatNode
        {
            public ChatChannelUIComponent chatChannelUI;
        }

        public class SelfUser : Node
        {
            public SelfUserComponent selfUser;
            public UserUidComponent userUid;
            public PersonalChatOwnerComponent personalChatOwner;
        }

        public class UserNode : Node
        {
            public UserUidComponent userUid;
            public UserAvatarComponent userAvatar;
        }

        public class VisibleNotGeneralChatNode : CreateChatSystem.NotGeneralChatNode
        {
            public ChatChannelUIComponent chatChannelUI;
        }

        public class VisiblePersonalChatNode : CreateChatSystem.PersonalChatNode
        {
            public ChatChannelUIComponent chatChannelUI;
        }
    }
}

