namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class ShaftAimingMapEffectSystem : ECSSystem
    {
        private const string HIDING_CENTER = "_HidingCenter";
        private const string MIN_HIDING_RADIUS = "_MinHidingRadius";
        private const string MAX_HIDING_RADIUS = "_MaxHidingRadius";
        private const string HIDING_SPEED = "_HidingSpeed";
        private const string HIDING_START_TIME = "_HidingStartTime";
        private const string ENABLE_HIDING_KEYWORD = "ENABLE_HIDING";
        private const int SHADER_DEFAULT_RENDER_QUEUE = -1;

        private void DisableMaterialHiding(Material material)
        {
            this.DisableMaterialHiding(material, -1);
        }

        private void DisableMaterialHiding(Material material, int renderQueue)
        {
            material.SetVector("_HidingCenter", Vector4.zero);
            material.SetFloat("_MaxHidingRadius", 0f);
            material.SetFloat("_MinHidingRadius", 0f);
            material.SetFloat("_HidingSpeed", 0f);
            material.SetFloat("_HidingStartTime", 0f);
            material.DisableKeyword("ENABLE_HIDING");
            material.renderQueue = renderQueue;
            Camera.main.GetComponent<NewGlobalFog>().ShaftDensity = 1f;
        }

        private void DisableMaterialHiding(Material material, Shader targetShader)
        {
            material.shader = targetShader;
            this.DisableMaterialHiding(material);
        }

        private void DisableMaterialHiding(Material material, Shader targetShader, int targetRenderQueue)
        {
            material.shader = targetShader;
            material.renderQueue = targetRenderQueue;
            this.DisableMaterialHiding(material);
        }

        private void EnableHidingItem(Material item, float startTime, ShaftAimingMapWorkingNode weapon)
        {
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetBarrelOriginWorld();
            ShaftAimingMapEffectComponent shaftAimingMapEffect = weapon.shaftAimingMapEffect;
            float initialEnergy = weapon.shaftAimingWorkingState.InitialEnergy;
            float maxRadius = shaftAimingMapEffect.ShrubsHidingRadiusMax * initialEnergy;
            Vector4 hidingCenter = new Vector4(barrelOriginWorld.x, barrelOriginWorld.y, barrelOriginWorld.z, 0f);
            this.EnableMaterialHiding(item, hidingCenter, weapon.shaftEnergy.UnloadAimingEnergyPerSec, maxRadius, Mathf.Lerp(shaftAimingMapEffect.ShrubsHidingRadiusMin, maxRadius, weapon.shaftAimingWorkingState.ExhaustedEnergy / initialEnergy), startTime);
        }

        private void EnableHidingItem(Material material, ShaftAimingMapWorkingNode weapon, Shader targetShader, float startTime)
        {
            material.shader = targetShader;
            this.EnableHidingItem(material, startTime, weapon);
        }

        private void EnableHidingItem(Material material, ShaftAimingMapWorkingNode weapon, Shader targetShader, int targetRenderQueue, float startTime)
        {
            material.shader = targetShader;
            material.renderQueue = targetRenderQueue;
            this.EnableHidingItem(material, startTime, weapon);
        }

        private void EnableMaterialHiding(Material material, Vector4 hidingCenter, float speed, float maxRadius, float minRadius, float startTime)
        {
            material.renderQueue = 0xdac;
            material.EnableKeyword("ENABLE_HIDING");
            material.SetVector("_HidingCenter", hidingCenter);
            material.SetFloat("_MaxHidingRadius", maxRadius);
            material.SetFloat("_MinHidingRadius", minRadius);
            material.SetFloat("_HidingSpeed", speed);
            material.SetFloat("_HidingStartTime", startTime);
            Camera.main.GetComponent<NewGlobalFog>().ShaftDensity = 0f;
        }

        [OnEventFire]
        public void StartHiding(NodeAddedEvent evt, ShaftAimingMapWorkingNode weapon, [JoinByBattle] ICollection<BonusBoxNode> bonuses, [JoinAll] ICollection<BonusRegionNode> regions, ShaftAimingMapWorkingNode weaponToJoinParachutes, [JoinByBattle] ICollection<BonusParachuteNode> parachutes, ShaftAimingMapWorkingNode weaponToJoinFlags, [JoinByBattle] ICollection<FlagNode> flags, [JoinAll] ICollection<SingleNode<MapHidingGeometryComponent>> hidingGeometryCollection)
        {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            foreach (BonusBoxNode node in bonuses)
            {
                this.EnableHidingItem(node.material.Material, timeSinceLevelLoad, weapon);
            }
            foreach (BonusRegionNode node2 in regions)
            {
                this.EnableHidingItem(node2.material.Material, timeSinceLevelLoad, weapon);
            }
            foreach (BonusParachuteNode node3 in parachutes)
            {
                this.EnableHidingItem(node3.parachuteMaterial.Material, timeSinceLevelLoad, weapon);
            }
            foreach (FlagNode node4 in flags)
            {
                this.EnableHidingItem(node4.flagInstance.FlagInstance.GetComponent<Sprite3D>().material, timeSinceLevelLoad, weapon);
            }
            ShaftAimingMapEffectComponent shaftAimingMapEffect = weapon.shaftAimingMapEffect;
            foreach (SingleNode<MapHidingGeometryComponent> node5 in hidingGeometryCollection)
            {
                Renderer[] hidingRenderers = node5.component.hidingRenderers;
                int index = 0;
                while (index < hidingRenderers.Length)
                {
                    Renderer renderer = hidingRenderers[index];
                    renderer.receiveShadows = false;
                    Material[] materials = renderer.materials;
                    int num3 = 0;
                    while (true)
                    {
                        if (num3 >= materials.Length)
                        {
                            index++;
                            break;
                        }
                        Material material = materials[num3];
                        if (material.shader == shaftAimingMapEffect.DefaultLeavesShader)
                        {
                            this.EnableHidingItem(material, weapon, shaftAimingMapEffect.HidingLeavesShader, 0xdac, timeSinceLevelLoad);
                        }
                        else if (material.shader == shaftAimingMapEffect.DefaultBillboardTreesShader)
                        {
                            this.EnableHidingItem(material, weapon, shaftAimingMapEffect.HidingBillboardTreesShader, timeSinceLevelLoad);
                        }
                        num3++;
                    }
                }
            }
        }

        [OnEventFire]
        public void StartHidingAnyNewBonus(NodeAddedEvent evt, BonusBoxNode bonus, [JoinByBattle] ShaftAimingMapWorkingNode weapon)
        {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            this.EnableHidingItem(bonus.material.Material, timeSinceLevelLoad, weapon);
        }

        [OnEventFire]
        public void StartHidingAnyNewFlag(NodeAddedEvent evt, FlagNode flag, [JoinByBattle] ShaftAimingMapWorkingNode weapon)
        {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            this.EnableHidingItem(flag.flagInstance.FlagInstance.GetComponent<Sprite3D>().material, timeSinceLevelLoad, weapon);
        }

        [OnEventFire]
        public void StartHidingAnyNewParachute(NodeAddedEvent evt, BonusParachuteNode parachute, [JoinByBattle] ShaftAimingMapWorkingNode weapon)
        {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            this.EnableHidingItem(parachute.parachuteMaterial.Material, timeSinceLevelLoad, weapon);
        }

        [OnEventFire]
        public void StopHiding(NodeAddedEvent evt, AimingMapIdleNode weapon, [JoinByBattle] ICollection<BonusBoxNode> bonuses, [JoinAll] ICollection<BonusRegionNode> regions, AimingMapIdleNode weaponToJoinParachutes, [JoinByBattle] ICollection<BonusParachuteNode> parachutes, AimingMapIdleNode weaponToJoinFlags, [JoinByBattle] ICollection<FlagNode> flags, [JoinAll] ICollection<SingleNode<MapHidingGeometryComponent>> hidingGeometryCollection)
        {
            foreach (BonusBoxNode node in bonuses)
            {
                this.DisableMaterialHiding(node.material.Material);
            }
            foreach (BonusRegionNode node2 in regions)
            {
                this.DisableMaterialHiding(node2.material.Material, 0xc1c);
            }
            foreach (BonusParachuteNode node3 in parachutes)
            {
                this.DisableMaterialHiding(node3.parachuteMaterial.Material);
            }
            foreach (FlagNode node4 in flags)
            {
                this.DisableMaterialHiding(node4.flagInstance.FlagInstance.GetComponent<Sprite3D>().material);
            }
            ShaftAimingMapEffectComponent shaftAimingMapEffect = weapon.shaftAimingMapEffect;
            foreach (SingleNode<MapHidingGeometryComponent> node5 in hidingGeometryCollection)
            {
                Renderer[] hidingRenderers = node5.component.hidingRenderers;
                int index = 0;
                while (index < hidingRenderers.Length)
                {
                    Renderer renderer = hidingRenderers[index];
                    renderer.receiveShadows = true;
                    Material[] materials = renderer.materials;
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 >= materials.Length)
                        {
                            index++;
                            break;
                        }
                        Material material = materials[num2];
                        if (material.shader == shaftAimingMapEffect.HidingLeavesShader)
                        {
                            this.DisableMaterialHiding(material, shaftAimingMapEffect.DefaultLeavesShader, -1);
                        }
                        else if (material.shader == shaftAimingMapEffect.HidingBillboardTreesShader)
                        {
                            this.DisableMaterialHiding(material, shaftAimingMapEffect.DefaultBillboardTreesShader);
                        }
                        num2++;
                    }
                }
            }
        }

        public class AimingMapIdleNode : Node
        {
            public ShaftAimingMapEffectComponent shaftAimingMapEffect;
            public ShaftIdleStateComponent shaftIdleState;
            public ShaftStateControllerComponent shaftStateController;
            public BattleGroupComponent battleGroup;
        }

        public class BonusBoxNode : Node
        {
            public MaterialComponent material;
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BattleGroupComponent battleGroup;
        }

        public class BonusParachuteNode : Node
        {
            public ParachuteMaterialComponent parachuteMaterial;
            public BonusParachuteInstanceComponent bonusParachuteInstance;
            public BattleGroupComponent battleGroup;
        }

        public class BonusRegionNode : Node
        {
            public MaterialComponent material;
            public BonusRegionInstanceComponent bonusRegionInstance;
        }

        public class FlagNode : Node
        {
            public FlagInstanceComponent flagInstance;
            public BattleGroupComponent battleGroup;
        }

        public class ShaftAimingMapWorkingNode : Node
        {
            public ShaftAimingMapEffectComponent shaftAimingMapEffect;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public MuzzlePointComponent muzzlePoint;
            public ShaftStateControllerComponent shaftStateController;
            public ShaftEnergyComponent shaftEnergy;
            public BattleGroupComponent battleGroup;
        }
    }
}

