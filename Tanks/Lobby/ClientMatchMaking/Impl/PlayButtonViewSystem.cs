namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class PlayButtonViewSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayButtonTimerExpired(PlayButtonTimerExpiredEvent e, MatchBeginTimerStateNode node)
        {
            node.Entity.GetComponent<ESMComponent>().Esm.ChangeState<PlayButtonStates.MatchBeginningState>();
        }

        [OnEventFire]
        public void ToCustomBattleState(NodeAddedEvent e, CustomBattleStateNode button)
        {
            button.playButton.SetAnimatorTrigger("CustomBattle");
        }

        [OnEventFire]
        public void ToEnteringLobbyState(NodeAddedEvent e, EnteringLobbyStateNode node)
        {
            node.playButton.StopTheStopwatch();
            node.playButton.SetAnimatorTrigger("EnteringLobby");
        }

        [OnEventFire]
        public void ToMatchBeginnigState(NodeAddedEvent e, MatchBeginningStateNode button)
        {
            button.playButton.StopTheTimer();
            button.playButton.SetAnimatorTrigger("MatchBegining");
        }

        [OnEventFire]
        public void ToMatchBeginTimerState(NodeAddedEvent e, MatchBeginTimerStateNode node)
        {
            node.playButton.StopTheStopwatch();
            node.playButton.SetAnimatorTrigger("MatchBeginTimer");
        }

        [OnEventFire]
        public void ToNormalState(NodeAddedEvent e, NormalStateNode node)
        {
            node.playButton.StopTheStopwatch();
            node.playButton.SetAnimatorTrigger("Normal");
        }

        [OnEventFire]
        public void ToNotEnoughPlayersTimerState(NodeAddedEvent e, NotEnoughtPlayersStateNode node)
        {
            node.playButton.StopTheStopwatch();
            node.playButton.SetAnimatorTrigger("NotEnoughtPlayersTimer");
        }

        [OnEventFire]
        public void ToReturnToBattleState(NodeAddedEvent e, ReturnToBattleStateNode button)
        {
            button.playButton.SetAnimatorTrigger("ReturnToBattle");
        }

        [OnEventFire]
        public void ToReturnToBattleState(NodeAddedEvent e, ShareEnergyStateNode button)
        {
            button.playButton.SetAnimatorTrigger("EnergyShareState");
        }

        [OnEventFire]
        public void ToSearchingState(NodeAddedEvent e, SearchingStateNode node)
        {
            node.playButton.RunTheStopwatch();
            node.playButton.SetAnimatorTrigger("Searching");
        }

        [OnEventFire]
        public void ToStartCustomBattleState(NodeAddedEvent e, StartCustomBattleStateNode button)
        {
            button.playButton.SetAnimatorTrigger("StartCustomBattle");
        }

        public class CustomBattleStateNode : Node
        {
            public PlayButtonCustomBattleStateComponent playButtonCustomBattleState;
            public PlayButtonComponent playButton;
        }

        public class EnteringLobbyStateNode : Node
        {
            public PlayerButtonEnteringLobbyStateComponent playerButtonEnteringLobbyState;
            public PlayButtonComponent playButton;
        }

        public class MatchBeginningStateNode : Node
        {
            public PlayButtonMatchBeginningStateComponent playButtonMatchBeginningState;
            public PlayButtonComponent playButton;
        }

        public class MatchBeginTimerStateNode : Node
        {
            public PlayerButtonMatchBeginTimerStateComponent playerButtonMatchBeginTimerState;
            public PlayButtonComponent playButton;
            public ESMComponent esm;
        }

        public class NormalStateNode : Node
        {
            public PlayButtonNormalStateComponent playButtonNormalState;
            public PlayButtonComponent playButton;
        }

        public class NotEnoughtPlayersStateNode : Node
        {
            public PlayButtonNotEnoughtPlayersTimerStateComponent playButtonNotEnoughtPlayersTimerState;
            public PlayButtonComponent playButton;
        }

        public class ReturnToBattleStateNode : Node
        {
            public PlayButtonReturnToBattleStateComponent playButtonReturnToBattleState;
            public PlayButtonComponent playButton;
        }

        public class SearchingStateNode : Node
        {
            public PlayerButtonSearchingStateComponent playerButtonSearchingState;
            public PlayButtonComponent playButton;
        }

        public class ShareEnergyStateNode : Node
        {
            public PlayButtonEnergyShareScreenStateComponent playButtonEnergyShareScreenState;
            public PlayButtonComponent playButton;
        }

        public class StartCustomBattleStateNode : Node
        {
            public PlayButtonStartCustomBattleStateComponent playButtonStartCustomBattleState;
            public PlayButtonComponent playButton;
        }
    }
}

