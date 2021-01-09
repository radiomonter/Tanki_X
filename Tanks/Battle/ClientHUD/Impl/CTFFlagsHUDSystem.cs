namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class CTFFlagsHUDSystem : ECSSystem
    {
        [OnEventFire]
        public void OnDeliver(FlagDeliveryEvent e, FlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinAll] HUDNode hud, Optional<SingleNode<CTFHUDMessagesComponent>> messages)
        {
            <OnDeliver>c__AnonStorey0 storey = new <OnDeliver>c__AnonStorey0 {
                hud = hud
            };
            TeamColor teamColor = color.component.TeamColor;
            if (teamColor == TeamColor.BLUE)
            {
                storey.hud.flagsHUD.BlueFlag.TurnIn(new Action(storey.<>m__0));
            }
            else if (teamColor == TeamColor.RED)
            {
                storey.hud.flagsHUD.RedFlag.TurnIn(new Action(storey.<>m__1));
            }
            storey.hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(true);
            if (base.Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault<HUDNodes.SelfTankNode>() != null)
            {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key)
                {
                    storey.hud.mainHUD.RemoveMessageByPriority(100);
                }
                else
                {
                    storey.hud.mainHUD.RemoveMessageByPriority(10);
                }
                if (messages.IsPresent())
                {
                    storey.hud.mainHUD.ShowMessageWithPriority(messages.Get().component.CaptureFlagMessage, 0);
                }
            }
        }

        [OnEventFire]
        public void OnDeliver(FlagNotCountedDeliveryEvent e, FlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinAll] HUDNode hud, Optional<SingleNode<CTFHUDMessagesComponent>> messages)
        {
            <OnDeliver>c__AnonStorey1 storey = new <OnDeliver>c__AnonStorey1 {
                hud = hud
            };
            TeamColor teamColor = color.component.TeamColor;
            if (teamColor == TeamColor.BLUE)
            {
                storey.hud.flagsHUD.BlueFlag.TurnIn(new Action(storey.<>m__0));
            }
            else if (teamColor == TeamColor.RED)
            {
                storey.hud.flagsHUD.RedFlag.TurnIn(new Action(storey.<>m__1));
            }
            storey.hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(true);
            if (base.Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault<HUDNodes.SelfTankNode>() != null)
            {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key)
                {
                    storey.hud.mainHUD.RemoveMessageByPriority(100);
                }
                else
                {
                    storey.hud.mainHUD.RemoveMessageByPriority(10);
                }
                if (messages.IsPresent())
                {
                    storey.hud.mainHUD.ShowMessageWithPriority(messages.Get().component.CaptureFlagMessage, 0);
                }
            }
        }

        [OnEventFire]
        public void OnDrop(NodeRemoveEvent e, CarriedFlagNode flag, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinAll] SingleNode<FlagsHUDComponent> hud)
        {
            hud.component.RequestHide();
        }

        [OnEventFire]
        public void OnDrop(NodeAddedEvent e, HUDNodes.SelfBattleUserNode self, [JoinByBattle, Context, Combine] DroppedFlagNode flag, [JoinByTeam, Context] TeamNode color, HUDNode hud, Optional<SingleNode<CTFHUDMessagesComponent>> messages)
        {
            TeamColor teamColor = color.colorInBattle.TeamColor;
            if (teamColor == TeamColor.BLUE)
            {
                hud.flagsHUD.BlueFlag.Drop();
            }
            else if (teamColor == TeamColor.RED)
            {
                hud.flagsHUD.RedFlag.Drop();
            }
            hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(true);
            if ((base.Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault<HUDNodes.SelfTankNode>() != null) && messages.IsPresent())
            {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key)
                {
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.ReturnFlagMessage, 0);
                }
                else
                {
                    hud.mainHUD.RemoveMessageByPriority(10);
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.CaptureFlagMessage, 0);
                }
            }
        }

        [OnEventFire]
        public void OnDropFlag(NodeAddedEvent e, [Context, Combine] DroppedFlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle] ICollection<FlagPedestalNode> pedestals, SingleNode<FlagsHUDComponent> hud, [JoinAll] HUDNodes.SelfBattleUserNode self1, [JoinByBattle] ICollection<TeamNode> teams)
        {
            this.UpdateFlagPosition(flag, pedestals, teams, color.component, hud.component);
        }

        [OnEventFire]
        public void OnMoveFlag(TimeUpdateEvent e, CarriedFlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle, Combine] TeamNode team, [JoinByBattle] ICollection<FlagPedestalNode> pedestals, [JoinAll] SingleNode<FlagsHUDComponent> hud, [JoinAll] HUDNodes.SelfBattleUserNode self1, [JoinByBattle] ICollection<TeamNode> teams)
        {
            this.UpdateFlagPosition(flag, pedestals, teams, color.component, hud.component);
        }

        [OnEventFire]
        public void OnPickUp(NodeAddedEvent e, HUDNodes.SelfBattleUserNode self, [JoinByBattle, Context, Combine] CarriedFlagNode flag, [JoinByTeam, Context] TeamNode color, HUDNode hud, Optional<SingleNode<CTFHUDMessagesComponent>> messages)
        {
            TeamColor teamColor = color.colorInBattle.TeamColor;
            if (teamColor == TeamColor.BLUE)
            {
                hud.flagsHUD.BlueFlag.PickUp();
            }
            else if (teamColor == TeamColor.RED)
            {
                hud.flagsHUD.RedFlag.PickUp();
            }
            hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(false);
            HUDNodes.SelfTankNode node = base.Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault<HUDNodes.SelfTankNode>();
            if ((node != null) && messages.IsPresent())
            {
                if ((node.tankGroup.Key == flag.tankGroup.Key) && (node.Entity.GetComponent<TeamGroupComponent>().Key != flag.teamGroup.Key))
                {
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.DeliverFlagMessage, 10);
                }
                else
                {
                    FlagCarrierNode node2 = base.Select<FlagCarrierNode>(flag.Entity, typeof(TankGroupComponent)).SingleOrDefault<FlagCarrierNode>();
                    if ((node2 != null) && (node2.teamGroup.Key != flag.teamGroup.Key))
                    {
                        if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key)
                        {
                            hud.mainHUD.ShowMessageWithPriority(messages.Get().component.ReturnFlagMessage, 100);
                        }
                        else
                        {
                            hud.mainHUD.ShowMessageWithPriority(messages.Get().component.ProtectFlagCarrierMessage, 10);
                        }
                    }
                }
            }
        }

        [OnEventFire]
        public void OnReturn(NodeRemoveEvent e, DroppedFlagNode flag, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinAll] SingleNode<FlagsHUDComponent> hud)
        {
            hud.component.RequestHide();
        }

        [OnEventFire]
        public void OnReturn(FlagReturnEvent e, FlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinAll] HUDNode hud, [JoinAll] Optional<SingleNode<CTFHUDMessagesComponent>> messages)
        {
            <OnReturn>c__AnonStorey2 storey = new <OnReturn>c__AnonStorey2 {
                hud = hud
            };
            TeamColor teamColor = color.component.TeamColor;
            if (teamColor == TeamColor.BLUE)
            {
                storey.hud.flagsHUD.BlueFlag.Return(new Action(storey.<>m__0));
            }
            else if (teamColor == TeamColor.RED)
            {
                storey.hud.flagsHUD.RedFlag.Return(new Action(storey.<>m__1));
            }
            storey.hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(true);
            if (base.Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault<HUDNodes.SelfTankNode>() != null)
            {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key)
                {
                    storey.hud.mainHUD.RemoveMessageByPriority(100);
                }
                else
                {
                    storey.hud.mainHUD.RemoveMessageByPriority(10);
                }
                if (messages.IsPresent())
                {
                    storey.hud.mainHUD.ShowMessageWithPriority(messages.Get().component.CaptureFlagMessage, 0);
                }
            }
        }

        private float Project(Vector3 from, Vector3 to, Vector3 point)
        {
            Vector3 vector = to - from;
            Vector3 vector2 = point - from;
            return ((vector2.magnitude * Vector3.Dot(vector2.normalized, vector.normalized)) / (vector.magnitude * 0.95f));
        }

        private void UpdateFlagPosition(FlagNode flag, ICollection<FlagPedestalNode> pedestals, ICollection<TeamNode> teams, ColorInBattleComponent colorInBattle, FlagsHUDComponent hud)
        {
            BoxCollider boxCollider = flag.flagCollider.boxCollider;
            Vector3 point = boxCollider.transform.TransformPoint(boxCollider.center);
            FlagPedestalComponent flagPedestal = null;
            FlagPedestalComponent flagPedestal = null;
            IEnumerator<FlagPedestalNode> enumerator = pedestals.GetEnumerator();
            while (enumerator.MoveNext())
            {
                FlagPedestalNode current = enumerator.Current;
                TeamNode node2 = null;
                IEnumerator<TeamNode> enumerator2 = teams.GetEnumerator();
                while (true)
                {
                    if (enumerator2.MoveNext())
                    {
                        TeamNode node3 = enumerator2.Current;
                        if (node3.teamGroup.Key != current.teamGroup.Key)
                        {
                            continue;
                        }
                        node2 = node3;
                    }
                    TeamColor color = node2.colorInBattle.TeamColor;
                    if (color == TeamColor.BLUE)
                    {
                        flagPedestal = current.flagPedestal;
                    }
                    else if (color == TeamColor.RED)
                    {
                        flagPedestal = current.flagPedestal;
                    }
                    break;
                }
            }
            TeamColor teamColor = colorInBattle.TeamColor;
            if (teamColor == TeamColor.BLUE)
            {
                hud.BlueFlagNormalizedPosition = this.Project(flagPedestal.Position, flagPedestal.Position, point);
            }
            else if (teamColor == TeamColor.RED)
            {
                hud.RedFlagNormalizedPosition = this.Project(flagPedestal.Position, flagPedestal.Position, point);
            }
        }

        [CompilerGenerated]
        private sealed class <OnDeliver>c__AnonStorey0
        {
            internal CTFFlagsHUDSystem.HUDNode hud;

            internal void <>m__0()
            {
                this.hud.flagsHUD.BlueFlagNormalizedPosition = 0f;
                this.hud.flagsHUD.RequestHide();
            }

            internal void <>m__1()
            {
                this.hud.flagsHUD.RedFlagNormalizedPosition = 0f;
                this.hud.flagsHUD.RequestHide();
            }
        }

        [CompilerGenerated]
        private sealed class <OnDeliver>c__AnonStorey1
        {
            internal CTFFlagsHUDSystem.HUDNode hud;

            internal void <>m__0()
            {
                this.hud.flagsHUD.BlueFlagNormalizedPosition = 0f;
                this.hud.flagsHUD.RequestHide();
            }

            internal void <>m__1()
            {
                this.hud.flagsHUD.RedFlagNormalizedPosition = 0f;
                this.hud.flagsHUD.RequestHide();
            }
        }

        [CompilerGenerated]
        private sealed class <OnReturn>c__AnonStorey2
        {
            internal CTFFlagsHUDSystem.HUDNode hud;

            internal void <>m__0()
            {
                this.hud.flagsHUD.BlueFlagNormalizedPosition = 0f;
                this.hud.flagsHUD.RequestHide();
            }

            internal void <>m__1()
            {
                this.hud.flagsHUD.RedFlagNormalizedPosition = 0f;
                this.hud.flagsHUD.RequestHide();
            }
        }

        public class CarriedFlagNode : CTFFlagsHUDSystem.FlagNode
        {
            public TankGroupComponent tankGroup;
        }

        public class DroppedFlagNode : CTFFlagsHUDSystem.FlagNode
        {
            public FlagGroundedStateComponent flagGroundedState;
        }

        public class FlagCarrierNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public TeamGroupComponent teamGroup;
        }

        public class FlagNode : Node
        {
            public TeamGroupComponent teamGroup;
            public BattleGroupComponent battleGroup;
            public FlagInstanceComponent flagInstance;
            public FlagColliderComponent flagCollider;
        }

        public class FlagPedestalNode : Node
        {
            public FlagPedestalComponent flagPedestal;
            public TeamGroupComponent teamGroup;
        }

        public class HUDNode : Node
        {
            public MainHUDComponent mainHUD;
            public FlagsHUDComponent flagsHUD;
        }

        public class TeamNode : Node
        {
            public TeamGroupComponent teamGroup;
            public ColorInBattleComponent colorInBattle;
        }
    }
}

