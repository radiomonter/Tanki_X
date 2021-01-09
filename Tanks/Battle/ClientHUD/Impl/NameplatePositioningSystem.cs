namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class NameplatePositioningSystem : ECSSystem
    {
        private const float REPOSITION_THRESHOLD = 1.2f;

        private void AlignToCamera(NameplateNode nameplate, Transform nameplateTransform, Camera camera)
        {
            Vector3 inCamPos = camera.WorldToScreenPoint(nameplateTransform.position);
            Vector3 previousPosition = nameplate.nameplatePosition.previousPosition;
            float x = Mathf.Round(inCamPos.x);
            float y = Mathf.Round(inCamPos.y);
            float z = inCamPos.z;
            if (this.NearlyEqual(inCamPos, previousPosition))
            {
                inCamPos.x = Mathf.Round(previousPosition.x);
                inCamPos.y = Mathf.Round(previousPosition.y);
            }
            else
            {
                inCamPos.x = Mathf.Round(inCamPos.x);
                inCamPos.y = Mathf.Round(inCamPos.y);
            }
            nameplate.nameplatePosition.previousPosition = inCamPos;
            Vector3 position = new Vector3(x, y, z);
            nameplateTransform.position = camera.ScreenToWorldPoint(position);
            nameplateTransform.rotation = camera.transform.rotation;
        }

        private bool NearlyEqual(Vector3 inCamPos, Vector3 previousPos) => 
            (Mathf.Abs((float) (inCamPos.x - previousPos.x)) <= 1.2f) && (Mathf.Abs((float) (inCamPos.y - previousPos.y)) <= 1.2f);

        private void PositionAboveTank(Vector3 position, Transform nameplateTransform, NameplateComponent nameplateComponent)
        {
            float x = position.x;
            nameplateTransform.position = new Vector3(x, position.y + nameplateComponent.yOffset, position.z);
        }

        [OnEventFire]
        public void UpdateNameplateTransform(UpdateEvent e, NameplateNode nameplate, [JoinByTank] WeaponRendererNode weapon, [JoinByTank] TankNode remoteTank, [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD)
        {
            NameplateComponent nameplateComponent = nameplate.nameplate;
            Transform nameplateTransform = nameplateComponent.transform;
            Camera cachedCamera = nameplateComponent.CachedCamera;
            Vector3 position = weapon.weaponVisualRoot.transform.position;
            this.PositionAboveTank(position, nameplateTransform, nameplateComponent);
            this.AlignToCamera(nameplate, nameplateTransform, cachedCamera);
            WorldSpaceHUDUtil.ScaleToRealSize(worldSpaceHUD.component.canvas.transform, nameplateTransform, nameplateComponent.CachedCamera);
            nameplate.nameplatePosition.sqrDistance = (cachedCamera.transform.position - nameplateTransform.position).sqrMagnitude;
        }

        public class NameplateNode : Node
        {
            public NameplateComponent nameplate;
            public TankGroupComponent tankGroup;
            public NameplatePositionComponent nameplatePosition;
        }

        public class TankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankVisualRootComponent tankVisualRoot;
            public RemoteTankComponent remoteTank;
        }

        public class WeaponRendererNode : Node
        {
            public WeaponComponent weapon;
            public TankGroupComponent tankGroup;
            public WeaponVisualRootComponent weaponVisualRoot;
        }
    }
}

