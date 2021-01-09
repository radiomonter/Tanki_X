namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class ForceFieldEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void CreateVisualEffect(NodeAddedEvent e, SingleNode<PreloadedModuleEffectsComponent> mapEffect, [Combine] ForceFieldEffectTransformNode effect)
        {
            GameObject original = mapEffect.component.PreloadedEffects["forcefield"];
            if (original)
            {
                GameObject gameObject = Object.Instantiate<GameObject>(original, null);
                gameObject.SetActive(true);
                gameObject.transform.SetPositionSafe(effect.forceFieldTranform.Movement.Position);
                gameObject.transform.SetRotationSafe(effect.forceFieldTranform.Movement.Orientation);
                gameObject.GetComponent<EntityBehaviour>().BuildEntity(effect.Entity);
                gameObject.SetActive(true);
                ForceFieldEffect component = gameObject.GetComponent<ForceFieldEffect>();
                component.Show();
                component.SetLayer(Layers.VISUAL_STATIC);
                effect.Entity.AddComponent(new EffectInstanceComponent(gameObject));
                gameObject.AddComponent<Rigidbody>().isKinematic = true;
                ForcefieldTargetBehaviour behaviour = gameObject.AddComponent<ForcefieldTargetBehaviour>();
                behaviour.OwnerTeamCanShootThrough = effect.forceFieldEffect.OwnerTeamCanShootThrough;
                behaviour.Init(effect.Entity, null);
            }
        }

        [OnEventComplete]
        public void DrawWavesOnHit(UpdateEvent evt, SingleNode<StreamHitComponent> weaponNode, [JoinAll] ICollection<ForceFieldEffectInstanceNode> effects)
        {
            if (weaponNode.component.StaticHit != null)
            {
                this.DrawWavesOnHit(effects, weaponNode.component.StaticHit.Position, false);
            }
        }

        private void DrawWavesOnHit(ICollection<ForceFieldEffectInstanceNode> effects, Vector3 hitPosition, bool playSound)
        {
            foreach (ForceFieldEffectInstanceNode node in effects)
            {
                ForceFieldEffect component = node.effectInstance.GameObject.GetComponent<ForceFieldEffect>();
                MeshCollider outerMeshCollider = component.outerMeshCollider;
                if (Vector3.Distance(hitPosition, outerMeshCollider.ClosestPointOnBounds(hitPosition)) < 0.1f)
                {
                    component.DrawWave(hitPosition, playSound);
                }
            }
        }

        [OnEventFire]
        public void DrawWavesOnHit(BulletStaticHitEvent e, Node node, [JoinAll] ICollection<ForceFieldEffectInstanceNode> effects)
        {
            this.DrawWavesOnHit(effects, e.Position, true);
        }

        [OnEventFire]
        public void DrawWavesOnHit(HitEvent e, NotStreamWeaponNode node, [JoinAll] ICollection<ForceFieldEffectInstanceNode> effects)
        {
            if (e.StaticHit != null)
            {
                this.DrawWavesOnHit(effects, e.StaticHit.Position, true);
            }
        }

        [OnEventComplete]
        public void DrawWavesOnHitFromRicochet(RicochetBulletBounceEvent e, RicochetBulletNode ricochetBulletNode, [JoinAll] ICollection<ForceFieldEffectInstanceNode> effects)
        {
            Vector3 worldSpaceBouncePosition = e.WorldSpaceBouncePosition;
            this.DrawWavesOnHit(effects, worldSpaceBouncePosition, true);
        }

        [OnEventFire]
        public void FindLocation(NodeAddedEvent e, ForceFieldEffectNode effect, [Context, JoinByTank] SelfWeaponNode weaponNode)
        {
            ForceFieldTranformComponent transformComponent = ForceFieldTransformUtil.GetTransformComponent(weaponNode.weaponInstance.WeaponInstance.transform);
            effect.Entity.AddComponent(transformComponent);
        }

        [OnEventFire]
        public void HideVisualEffect(NodeRemoveEvent e, ForceFieldEffectInstanceNode fieldEffectNode)
        {
            fieldEffectNode.effectInstance.GameObject.GetComponent<ForceFieldEffect>().Hide();
        }

        [OnEventFire]
        public void InitForceFieldColor(NodeAddedEvent e, SelfBattleUserNode selfUser, [Combine] TeamForceFieldInstanceNode forceField)
        {
            if (!selfUser.Entity.IsSameGroup<TeamGroupComponent>(forceField.Entity))
            {
                forceField.effectInstance.GameObject.GetComponent<ForceFieldEffect>().SwitchToEnemyView();
            }
        }

        [OnEventFire]
        public void InitForceFieldColorForDm(NodeAddedEvent e, SelfDmBattleUserNode selfUser, [JoinByUser] SingleNode<TankComponent> tank, [Combine] ForceFieldEffectInstanceNode forceField)
        {
            if (!tank.Entity.IsSameGroup<TankGroupComponent>(forceField.Entity))
            {
                forceField.effectInstance.GameObject.GetComponent<ForceFieldEffect>().SwitchToEnemyView();
            }
        }

        [OnEventFire]
        public void InitForceFieldForSpectator(NodeAddedEvent e, SpectatorBattleUserNode spectator, [Context, Combine] TeamForceFieldInstanceNode forceField, [Context, JoinByTeam] TeamColorNode teamColor)
        {
            if (teamColor.teamColor.TeamColor == TeamColor.RED)
            {
                forceField.effectInstance.GameObject.GetComponent<ForceFieldEffect>().SwitchToEnemyView();
            }
        }

        public class ForceFieldEffectInstanceNode : ForceFieldEffectSystem.ForceFieldEffectTransformNode
        {
            public EffectInstanceComponent effectInstance;
        }

        public class ForceFieldEffectNode : Node
        {
            public ForceFieldEffectComponent forceFieldEffect;
            public TankGroupComponent tankGroup;
        }

        public class ForceFieldEffectTransformNode : ForceFieldEffectSystem.ForceFieldEffectNode
        {
            public ForceFieldTranformComponent forceFieldTranform;
        }

        [Not(typeof(StreamWeaponComponent)), Not(typeof(StreamHitConfigComponent))]
        public class NotStreamWeaponNode : Node
        {
            public WeaponComponent weapon;
        }

        public class RicochetBulletNode : Node
        {
            public BulletComponent bullet;
            public RicochetBulletComponent ricochetBullet;
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public TeamGroupComponent teamGroup;
        }

        [Not(typeof(TeamGroupComponent)), Not(typeof(UserInBattleAsSpectatorComponent))]
        public class SelfDmBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
        }

        public class SelfWeaponNode : ForceFieldEffectSystem.WeaponNode
        {
            public SelfComponent self;
        }

        public class SpectatorBattleUserNode : Node
        {
            public UserInBattleAsSpectatorComponent userInBattleAsSpectator;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class TeamColorNode : Node
        {
            public TeamGroupComponent teamGroup;
            public TeamColorComponent teamColor;
        }

        public class TeamForceFieldInstanceNode : ForceFieldEffectSystem.ForceFieldEffectInstanceNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

