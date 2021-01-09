namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientHUD.API;
    using Tanks.Lobby.ClientControls.API;

    public class SelfDestructionHUDSystem : ECSSystem
    {
        [OnEventFire]
        public void HideOnDeactivate(NodeRemoveEvent e, SingleNode<TankActiveStateComponent> activeTank, [JoinByUser] SelfBattleUserNode selfBattleUser, [JoinAll] SelfDestructionServiceMessageNode serviceMessage)
        {
            serviceMessage.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageHiddenState>();
        }

        [OnEventFire]
        public void LocalizeSelfDestructionMessage(NodeAddedEvent e, SelfDestructionLocalizationNode node)
        {
            node.serviceMessage.MessageText.text = node.selfDestructionMessage.Message;
        }

        [OnEventFire]
        public void OnUpdate(TimeUpdateEvent evt, SelfDestructionServiceMessageNode selfDestruction, [JoinAll] SelfBattleUserNode selfBattleUser, [JoinAll] SingleNode<BattleScreenComponent> battleScreen)
        {
            if ((selfDestruction.timer.Timer != null) && ((selfDestruction.timer.Timer.SecondsLeft - evt.DeltaTime) >= 1f))
            {
                TimerUIComponent timer = selfDestruction.timer.Timer;
                timer.SecondsLeft -= evt.DeltaTime;
            }
        }

        [OnEventFire]
        public void ShowOnActivate(NodeAddedEvent evt, SelfDestructionNode selfDestruction, [JoinByUser] SingleNode<SelfDestructionConfigComponent> config, [JoinAll] SelfDestructionServiceMessageNode serviceMessage)
        {
            serviceMessage.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageVisibleState>();
            serviceMessage.timer.Timer.SecondsLeft = config.component.SuicideDurationTime + 1;
        }

        [Not(typeof(UserInBattleAsSpectatorComponent))]
        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public BattleUserComponent battleUser;
        }

        public class SelfDestructionLocalizationNode : Node
        {
            public SelfDestructionMessageComponent selfDestructionMessage;
            public ServiceMessageComponent serviceMessage;
        }

        public class SelfDestructionNode : Node
        {
            public SelfTankComponent selfTank;
            public SelfDestructionComponent selfDestruction;
        }

        public class SelfDestructionServiceMessageNode : Node
        {
            public SelfDestructionServiceMessageComponent selfDestructionServiceMessage;
            public ServiceMessageESMComponent serviceMessageESM;
            public ServiceMessageComponent serviceMessage;
            public TimerComponent timer;
        }

        public class ServiceMessageNode : Node
        {
            public ServiceMessageComponent serviceMessage;
        }
    }
}

