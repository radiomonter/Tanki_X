namespace Tanks.Lobby.ClientProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;

    public class CrystalsBufferSystem : ECSSystem
    {
        [OnEventFire]
        public void AddMoneyBuffer(NodeAddedEvent e, SelfUserNode user)
        {
            user.Entity.AddComponent<UserMoneyBufferComponent>();
        }

        [OnEventFire]
        public void ChangeBufferBy(ChangeUserMoneyBufferEvent e, UserMoneyBufferNode buffer)
        {
            buffer.userMoneyBuffer.ChangeCrystalBufferBy(e.Crystals);
            buffer.userMoneyBuffer.ChangeXCrystalBufferBy(e.XCrystals);
            base.ScheduleEvent<UserMoneyChangedEvent>(buffer);
            base.ScheduleEvent<UserXCrystalsChangedEvent>(buffer);
        }

        [OnEventFire]
        public void ResetBuffer(ResetUserMoneyBufferEvent e, UserMoneyBufferNode buffer)
        {
            buffer.userMoneyBuffer.CrystalBuffer = 0;
            buffer.userMoneyBuffer.XCrystalBuffer = 0;
        }

        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfComponent self;
        }

        public class UserMoneyBufferNode : CrystalsBufferSystem.SelfUserNode
        {
            public UserMoneyBufferComponent userMoneyBuffer;
        }
    }
}

