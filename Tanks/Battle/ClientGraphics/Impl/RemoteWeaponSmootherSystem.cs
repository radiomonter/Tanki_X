namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class RemoteWeaponSmootherSystem : ECSSystem
    {
        private const float SMOOTHING_COEFF = 5f;

        private float CalculateDistance(float logicValue, Transform weaponInstance)
        {
            float f = weaponInstance.localEulerAngles.y - logicValue;
            float num2 = 360f - Mathf.Abs(f);
            if (Mathf.Abs(f) > num2)
            {
                f = (f <= 0f) ? num2 : -num2;
            }
            return f;
        }

        private void InterpolateVisualRotation(Transform weaponInstance, Transform visualInstance, float deltaTime)
        {
            Vector3 localEulerAngles = visualInstance.localEulerAngles;
            localEulerAngles = new Vector3(0f, localEulerAngles.y, 0f);
            float num = this.CalculateDistance(localEulerAngles.y, weaponInstance);
            float num2 = 5f * deltaTime;
            if (num2 > 1f)
            {
                num2 = 1f;
            }
            localEulerAngles.y += num * num2;
            visualInstance.SetLocalEulerAnglesSafe(localEulerAngles);
            visualInstance.localPosition = Vector3.zero;
        }

        [OnEventComplete]
        public void OnUpdate(TimeUpdateEvent e, WeaponNode node, [JoinByTank] RemoteTankNode tank)
        {
            Transform weaponInstance = node.weaponInstance.WeaponInstance.transform;
            this.InterpolateVisualRotation(weaponInstance, node.weaponVisualRoot.transform, e.DeltaTime);
        }

        private void Snap(WeaponNode weapon)
        {
            Transform transform = weapon.weaponVisualRoot.transform;
            transform.SetLocalRotationSafe(weapon.weaponInstance.WeaponInstance.transform.localRotation);
            transform.localPosition = Vector3.zero;
        }

        [OnEventFire]
        public void SnapOnAdd(NodeAddedEvent e, WeaponNode weapon, [Context, JoinByTank] RemoteTankNode tank)
        {
            this.Snap(weapon);
        }

        [OnEventFire]
        public void SnapOnInit(TankMovementInitEvent e, WeaponNode weapon)
        {
            this.Snap(weapon);
        }

        public class RemoteTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public RemoteTankComponent remoteTank;
        }

        [Not(typeof(DetachedWeaponComponent))]
        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public WeaponVisualRootComponent weaponVisualRoot;
        }
    }
}

