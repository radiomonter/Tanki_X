﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class ModuleEffectGraphicsSystem : ECSSystem
    {
        private void ApplyTeamColor(EffectGraphicsNode effect, TeamColor color, bool useBlueMaterial)
        {
            Renderer renderer = effect.effectRendererGraphics.Renderer;
            EffectTeamGraphicsComponent effectTeamGraphics = effect.effectTeamGraphics;
            if (color == TeamColor.BLUE)
            {
                renderer.material = !useBlueMaterial ? new Material(effectTeamGraphics.SelfMaterial) : new Material(effectTeamGraphics.BlueTeamMaterial);
            }
            else if (color == TeamColor.RED)
            {
                renderer.material = new Material(effectTeamGraphics.RedTeamMaterial);
            }
            effect.Entity.AddComponent<EffectPaintedComponent>();
        }

        [OnEventFire]
        public void Disable(RemoveEffectEvent e, EffectRemovingNode effect)
        {
            InstantiateEffectEffect(effect.effectInstance, effect.removeEffectGraphics.EffectPrefab, effect.removeEffectGraphics.EffectLifeTime, effect.removeEffectGraphics.Origin);
        }

        [OnEventFire]
        public void InitEffectRenderer(NodeAddedEvent e, SingleNode<EffectInstanceComponent> effect)
        {
            Renderer renderer = effect.component.GameObject.GetComponentsInChildren<Renderer>()[0];
            effect.Entity.AddComponent(new EffectRendererGraphicsComponent(renderer));
        }

        [OnEventFire]
        public void InitEnemyDMMaterial(NodeAddedEvent e, EffectGraphicsNode effect, [JoinByUser] SingleNode<BattleUserComponent> battleUser, [JoinByBattle] SingleNode<DMComponent> dm)
        {
            this.ApplyTeamColor(effect, battleUser.Entity.GetComponent<ColorInBattleComponent>().TeamColor, true);
        }

        [OnEventFire]
        public void InitTeamMaterial(NodeAddedEvent e, [Combine] EffectTeamGraphicsNode effect, [JoinByTank, Context, Combine] RemoteTankNode remoteTank, [JoinByTeam] SingleNode<ColorInBattleComponent> teamUIColor)
        {
            this.ApplyTeamColor(effect, teamUIColor.component.TeamColor, true);
        }

        [OnEventFire]
        public void InitTeamMaterial(NodeAddedEvent e, [Combine] EffectTeamGraphicsNode effect, [JoinByTank, Context, Combine] SelfTankNode selfTank, [JoinByTeam] SingleNode<ColorInBattleComponent> teamUIColor)
        {
            this.ApplyTeamColor(effect, teamUIColor.component.TeamColor, false);
        }

        public static void InstantiateEffectEffect(EffectInstanceComponent effectInstanceComponent, GameObject prefab, float lifeTime, Vector3 yOrigin)
        {
            GameObject gameObject = effectInstanceComponent.GameObject;
            Object.DestroyObject(Object.Instantiate<GameObject>(prefab, gameObject.transform.position + yOrigin, Quaternion.identity), lifeTime);
        }

        public class EffectGraphicsNode : Node
        {
            public EffectRendererGraphicsComponent effectRendererGraphics;
            public EffectTeamGraphicsComponent effectTeamGraphics;
            public TankGroupComponent tankGroup;
        }

        public class EffectRemovingNode : Node
        {
            public RemoveEffectGraphicsComponent removeEffectGraphics;
            public EffectInstanceComponent effectInstance;
        }

        public class EffectTeamGraphicsNode : ModuleEffectGraphicsSystem.EffectGraphicsNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class RemoteTankNode : ModuleEffectGraphicsSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : ModuleEffectGraphicsSystem.TankNode
        {
            public SelfTankComponent selfTank;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public TeamGroupComponent teamGroup;
        }
    }
}

