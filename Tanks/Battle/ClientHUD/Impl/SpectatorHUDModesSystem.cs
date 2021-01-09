namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class SpectatorHUDModesSystem : ECSSystem
    {
        [OnEventFire]
        public void ActuateHUDMode(ChangeHUDModeEvent e, ActiveSpectatorNode spectator, [JoinAll] SingleNode<SpectatorBattleScreenComponent> battleSpectatorScreen, [JoinAll] SingleNode<BattleScreenComponent> battleScreen, [JoinAll] Optional<SingleNode<HUDWorldSpaceCanvas>> hudWorldspaceCanvas)
        {
            spectator.spectatorHUDMode.HUDMode = e.Mode;
            if (hudWorldspaceCanvas.IsPresent())
            {
                this.SetGameObjectVisibleByAlpha(hudWorldspaceCanvas.Get().component.gameObject, e.Mode == SpectatorHUDMode.Full);
            }
            this.SetGameObjectVisibleByAlpha(battleScreen.component.topPanel, ((e.Mode == SpectatorHUDMode.Full) || (e.Mode == SpectatorHUDMode.WithoutNameplates)) || (e.Mode == SpectatorHUDMode.WithoutMessagesAndChat));
            this.SetGameObjectVisible(battleScreen.component.combatEventLog, (e.Mode == SpectatorHUDMode.Full) || (e.Mode == SpectatorHUDMode.WithoutNameplates));
            this.SetGameObjectVisible(battleSpectatorScreen.component.spectatorChat, (e.Mode == SpectatorHUDMode.Full) || (e.Mode == SpectatorHUDMode.WithoutNameplates));
            this.SetGameObjectVisible(battleSpectatorScreen.component.scoreTable, ((e.Mode == SpectatorHUDMode.Full) || (e.Mode == SpectatorHUDMode.WithoutNameplates)) || (e.Mode == SpectatorHUDMode.OnlyScoreTable));
            this.SetGameObjectVisible(battleSpectatorScreen.component.scoreTableShadow, ((e.Mode == SpectatorHUDMode.Full) || (e.Mode == SpectatorHUDMode.WithoutNameplates)) || (e.Mode == SpectatorHUDMode.OnlyScoreTable));
        }

        [OnEventFire]
        public void ChangeMode(UpdateEvent evt, ActiveSpectatorNode spectator)
        {
            if (InputManager.GetKeyDown(KeyCode.Backslash))
            {
                SpectatorHUDMode hUDMode = spectator.spectatorHUDMode.HUDMode;
                ChangeHUDModeEvent eventInstance = new ChangeHUDModeEvent {
                    Mode = this.GetNextMode(hUDMode)
                };
                base.ScheduleEvent(eventInstance, spectator);
            }
        }

        private SpectatorHUDMode GetNextMode(SpectatorHUDMode current) => 
            (current != SpectatorHUDMode.NoHUD) ? (current + 1) : SpectatorHUDMode.Full;

        [OnEventFire]
        public void InitSpectator(NodeAddedEvent e, SpectatorNode spectator, SingleNode<BattleScreenComponent> screen, SingleNode<SpectatorBattleScreenComponent> specScreen)
        {
            spectator.Entity.AddComponent(new SpectatorHUDModeComponent(SpectatorHUDMode.Full));
            base.ScheduleEvent<ChangeHUDModeEvent>(spectator);
        }

        private void SetGameObjectVisible(GameObject go, bool visible)
        {
            go.SetActive(visible);
        }

        private void SetGameObjectVisibleByAlpha(GameObject go, bool visible)
        {
            go.GetComponent<CanvasGroup>().alpha = !visible ? ((float) 0) : ((float) 1);
        }

        private void SetVisible(CanvasGroup cg, bool visible)
        {
            cg.alpha = !visible ? ((float) 0) : ((float) 1);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class ActiveSpectatorNode : Node
        {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
            public SpectatorHUDModeComponent spectatorHUDMode;
        }

        public class SpectatorNode : Node
        {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }
    }
}

