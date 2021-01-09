namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public static class SoundListenerStates
    {
        public class SoundListenerBattleFinishState : Node
        {
            public SoundListenerBattleStateComponent soundListenerBattleState;
            public SoundListenerBattleFinishStateComponent soundListenerBattleFinishState;
        }

        public class SoundListenerBattleState : Node
        {
            public SoundListenerReadyForHitFeedbackComponent soundListenerReadyForHitFeedback;
            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        public class SoundListenerLobbyState : Node
        {
            public SoundListenerLobbyStateComponent soundListenerLobbyState;
        }

        public class SoundListenerSelfRankRewardState : Node
        {
            public SoundListenerBattleStateComponent soundListenerBattleState;
            public SoundListenerSelfRankRewardStateComponent soundListenerSelfRankRewardState;
        }

        public class SoundListenerSpawnState : Node
        {
            public SoundListenerSpawnStateComponent soundListenerSpawnState;
        }
    }
}

