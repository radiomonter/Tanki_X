namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class BattleHUDStates
    {
        public class ActionsState : Node
        {
            public BattleActionsStateComponent battleActionsState;
        }

        public class ChatState : Node
        {
            public BattleChatStateComponent battleChatState;
        }

        public class ShaftAimingState : Node
        {
            public BattleShaftAimingStateComponent battleShaftAimingState;
        }
    }
}

