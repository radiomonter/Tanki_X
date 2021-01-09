﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class FlagVisualPickupSystem : ECSSystem
    {
        private static readonly Vector3 FLAG_MOUNT = new Vector3(0.5f, 0f, -1f);

        [OnEventFire]
        public void PickupFlag(NodeAddedEvent e, CarriedFlagNode flag, [Context, JoinByTank] TankNode tank, [JoinByTank] WeaponNode weapon, [JoinByBattle] SingleNode<CTFConfigComponent> ctfConfig)
        {
            if (flag.teamGroup.Key != tank.teamGroup.Key)
            {
                Transform transform = flag.flagInstance.FlagInstance.transform;
                transform.parent = weapon.weaponVisualRoot.transform;
                transform.localPosition = FLAG_MOUNT;
                Transform child = transform.GetChild(0);
                if ((transform.parent != null) && (child != null))
                {
                    transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    child.transform.localScale = new Vector3(ctfConfig.component.flagScaleOnTank, ctfConfig.component.flagScaleOnTank, ctfConfig.component.flagScaleOnTank);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    child.transform.localPosition = new Vector3(0f, ctfConfig.component.flagOriginPositionOnTank, 0f);
                    transform.GetComponent<Sprite3D>().scale = 0f;
                    FlagVisualRotate component = transform.GetComponent<FlagVisualRotate>();
                    component.timerUpsideDown = ctfConfig.component.upsideDownMarkTimer;
                    component.scale = ctfConfig.component.upsideDownMarkScale;
                    component.origin = ctfConfig.component.upsideDownMarkOrigin;
                    component.distanceForRotateFlag = ctfConfig.component.distanceForRotateFlag;
                    component.tank = tank.tankVisualRoot.transform;
                }
            }
        }

        public class CarriedFlagNode : Node
        {
            public FlagComponent flag;
            public FlagInstanceComponent flagInstance;
            public TeamGroupComponent teamGroup;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankVisualRootComponent tankVisualRoot;
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
            public TeamGroupComponent teamGroup;
            public AssembledTankComponent assembledTank;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public WeaponVisualRootComponent weaponVisualRoot;
            public TankGroupComponent tankGroup;
        }
    }
}

