namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class UserMoneyIndicatorSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfUserMoneyNode userMoney, [Combine] UserMoneyIndicatorNode userMoneyIndicator)
        {
            userMoneyIndicator.userMoneyIndicator.SetMoneyImmediately(userMoney.userMoney.Money);
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, UserMoneyBufferNode userMoney, [Combine] UserMoneyIndicatorWithBufferNode userMoneyIndicator)
        {
            userMoneyIndicator.userMoneyIndicator.SetMoneyImmediately(userMoney.userMoney.Money - userMoney.userMoneyBuffer.CrystalBuffer);
        }

        [OnEventFire]
        public void UpdateIndicator(UserMoneyChangedEvent e, SelfUserMoneyNode userMoney, [Combine, JoinAll] UserMoneyIndicatorNode userMoneyIndicator)
        {
            userMoneyIndicator.userMoneyIndicator.SetMoneyAnimated(userMoney.userMoney.Money);
        }

        public class SelfUserMoneyNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
            public UserMoneyComponent userMoney;
        }

        public class UserMoneyBufferNode : UserMoneyIndicatorSystem.SelfUserMoneyNode
        {
            public UserMoneyBufferComponent userMoneyBuffer;
        }

        public class UserMoneyIndicatorNode : Node
        {
            public UserMoneyIndicatorComponent userMoneyIndicator;
        }

        public class UserMoneyIndicatorWithBufferNode : UserMoneyIndicatorSystem.UserMoneyIndicatorNode
        {
            public BufferComponent buffer;
        }
    }
}

