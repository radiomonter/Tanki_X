﻿namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;

    public class SelfUserToLoggerSystem : ECSSystem
    {
        [OnEventFire]
        public void RegisterUserUID(NodeAddedEvent e, SelfUserNode user)
        {
            ECStoLoggerGateway.UserUID = user.userUid.Uid;
        }

        [OnEventFire]
        public void UnRegisterUserUID(NodeRemoveEvent e, SelfUserNode user)
        {
            ECStoLoggerGateway.UserUID = string.Empty;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserUidComponent userUid;
        }
    }
}

