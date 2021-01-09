﻿namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientHUD.API;

    public class ServiceMessagesInteroperabilitySystem : ECSSystem
    {
        [OnEventFire]
        public void HideAutokickOnPause(NodeAddedEvent e, AutokickServiceMessageNode autokick, PauseServiceMessageNode pause)
        {
            autokick.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageHiddenState>();
        }

        [OnEventFire]
        public void HideAutokickWarningOnSuicide(NodeAddedEvent e, AutokickServiceMessageNode autokick, SelfDestructionServiceMessageNode selfDestruction)
        {
            autokick.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageHiddenState>();
        }

        [OnEventFire]
        public void HidePauseOnSuicide(NodeAddedEvent e, SelfDestructionServiceMessageNode selfDestruction, PauseServiceMessageNode pause)
        {
            base.NewEvent<HideServiceMessageEvent>().Attach(pause).Schedule();
        }

        [OnEventFire]
        public void HideUpsideDownMessage(NodeAddedEvent e, UpsideDownServiceMessageVisibleNode upsideDown, AutokickServiceMessageNode autokick)
        {
            upsideDown.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageHiddenState>();
        }

        [OnEventFire]
        public void HideUpsideDownMessage(NodeAddedEvent e, UpsideDownServiceMessageVisibleNode upsideDown, PauseServiceMessageNode pause)
        {
            upsideDown.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageHiddenState>();
        }

        [OnEventFire]
        public void HideUpsideDownMessage(NodeAddedEvent e, UpsideDownServiceMessageVisibleNode upsideDown, SelfDestructionServiceMessageNode selfDestruction)
        {
            upsideDown.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageHiddenState>();
        }

        [OnEventFire]
        public void RestoreUpsideDownAfterAutokickWarning(NodeAddedEvent e, AutokickServiceHiddenMessageNode autokick, [JoinAll] UpsideDownSelfTankNode tank, [JoinAll] UpsideDownHiddenServiceMessageNode upsideDown)
        {
            upsideDown.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageVisibleState>();
        }

        [OnEventFire]
        public void RestoreUpsideDownMessage(NodeRemoveEvent e, SingleNode<PauseComponent> pause, [JoinAll] UpsideDownSelfTankNode tank, [JoinAll] UpsideDownHiddenServiceMessageNode serviceMessage)
        {
            serviceMessage.serviceMessageESM.Esm.ChangeState<ServiceMessageStates.ServiceMessageVisibleState>();
        }

        public class AutokickServiceHiddenMessageNode : Node
        {
            public AutokickServiceMessageComponent autokickServiceMessage;
            public ServiceMessageESMComponent serviceMessageESM;
            public ServiceMessageHiddenStateComponent serviceMessageHiddenState;
        }

        public class AutokickServiceMessageNode : Node
        {
            public AutokickServiceMessageComponent autokickServiceMessage;
            public ServiceMessageESMComponent serviceMessageESM;
            public ServiceMessageVisibleStateComponent serviceMessageVisibleState;
        }

        public class PauseServiceMessageNode : Node
        {
            public PauseServiceMessageComponent pauseServiceMessage;
            public ServiceMessageESMComponent serviceMessageESM;
            public ServiceMessageVisibleStateComponent serviceMessageVisibleState;
        }

        public class SelfDestructionServiceMessageNode : Node
        {
            public SelfDestructionServiceMessageComponent selfDestructionServiceMessage;
            public ServiceMessageESMComponent serviceMessageESM;
            public ServiceMessageVisibleStateComponent serviceMessageVisibleState;
        }

        public class UpsideDownHiddenServiceMessageNode : Node
        {
            public UpsideDownServiceMessageComponent upsideDownServiceMessage;
            public ServiceMessageESMComponent serviceMessageESM;
            public ServiceMessageHiddenStateComponent serviceMessageHiddenState;
        }

        public class UpsideDownSelfTankNode : Node
        {
            public SelfTankComponent selfTank;
            public UpsideDownTankComponent upsideDownTank;
        }

        public class UpsideDownServiceMessageVisibleNode : Node
        {
            public UpsideDownServiceMessageComponent upsideDownServiceMessage;
            public ServiceMessageESMComponent serviceMessageESM;
            public ServiceMessageVisibleStateComponent serviceMessageVisibleState;
        }
    }
}

