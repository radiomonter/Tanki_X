namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class EntranceStatisticsSystem : ECSSystem
    {
        [OnEventFire]
        public void InvalidLogin(NodeAddedEvent e, InvalidLoginFieldNode login, [JoinAll] SessionNode session)
        {
            base.ScheduleEvent(new IncrementRegistrationNicksEvent(login.inputField.Input), session);
        }

        [OnEventFire]
        public void InvalidPassword(NodeAddedEvent e, InvalidPasswordFieldNode password, [JoinAll] SessionNode session)
        {
            base.ScheduleEvent<InvalidRegistrationPasswordEvent>(session);
        }

        [OnEventFire]
        public void InvalidPasswordRepeat(NodeAddedEvent e, InvalidPasswordRepeatFieldNode password, [JoinAll] SessionNode session)
        {
            base.ScheduleEvent<InvalidRegistrationPasswordEvent>(session);
        }

        [OnEventFire]
        public void SendClientInfoStatistics(NodeAddedEvent e, UserOnlineNode userNode, [JoinAll] SessionNode session, Optional<SingleNode<SteamMarkerComponent>> steamNode)
        {
            ClientInfo info = new ClientInfo {
                deviceModel = SystemInfo.deviceModel,
                deviceName = SystemInfo.deviceName,
                deviceType = SystemInfo.deviceType.ToString(),
                deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier,
                graphicsDeviceName = SystemInfo.graphicsDeviceName,
                graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor,
                graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion,
                graphicsDeviceID = SystemInfo.graphicsDeviceID,
                graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString(),
                graphicsDeviceVendorID = SystemInfo.graphicsDeviceVendorID,
                graphicsMemorySize = SystemInfo.graphicsMemorySize,
                graphicsShaderLevel = SystemInfo.graphicsShaderLevel,
                operatingSystem = SystemInfo.operatingSystem,
                systemMemorySize = SystemInfo.systemMemorySize,
                processorType = SystemInfo.processorType,
                processorCount = SystemInfo.processorCount,
                processorFrequency = SystemInfo.processorFrequency,
                supportsLocationService = SystemInfo.supportsLocationService,
                qualityLevel = QualitySettings.GetQualityLevel(),
                resolution = Screen.currentResolution.ToString(),
                dpi = Screen.dpi,
                entranceSource = (!steamNode.IsPresent() ? 0 : 1).ToString()
            };
            base.ScheduleEvent(new ClientInfoSendEvent(JsonUtility.ToJson(info)), session);
        }

        [OnEventFire]
        public void ValidLogin(NodeAddedEvent e, ValidLoginFieldNode login, [JoinAll] SessionNode session)
        {
            base.ScheduleEvent(new IncrementRegistrationNicksEvent(login.inputField.Input), session);
        }

        public enum EntranceSource
        {
            CLIENT,
            STEAM
        }

        public class InvalidLoginFieldNode : Node
        {
            public RegistrationLoginInputComponent registrationLoginInput;
            public InputFieldComponent inputField;
            public InputFieldValidStateComponent inputFieldValidState;
        }

        public class InvalidPasswordFieldNode : Node
        {
            public RegistrationPasswordInputComponent registrationPasswordInput;
            public InputFieldInvalidStateComponent inputFieldInvalidState;
        }

        public class InvalidPasswordRepeatFieldNode : Node
        {
            public RepetitionPasswordInputComponent repetitionPasswordInput;
            public InputFieldInvalidStateComponent inputFieldInvalidState;
        }

        public class SessionNode : Node
        {
            public ClientSessionComponent clientSession;
        }

        public class UserOnlineNode : Node
        {
            public SelfUserComponent selfUser;
            public UserOnlineComponent userOnline;
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class ValidLoginFieldNode : Node
        {
            public RegistrationLoginInputComponent registrationLoginInput;
            public InputFieldComponent inputField;
            public InputFieldInvalidStateComponent inputFieldInvalidState;
        }
    }
}

