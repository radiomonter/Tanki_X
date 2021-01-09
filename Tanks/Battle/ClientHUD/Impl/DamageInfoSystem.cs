namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using TMPro;
    using UnityEngine;

    public class DamageInfoSystem : ECSSystem
    {
        [OnEventFire]
        public unsafe void ShowDamage(DamageInfoEvent e, SingleNode<TankVisualRootComponent> tank, [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD, [JoinAll] SingleNode<GameTankSettingsComponent> gameSettings)
        {
            if (gameSettings.component.DamageInfoEnabled)
            {
                Vector3 hitPoint = e.HitPoint;
                if (hitPoint == Vector3.zero)
                {
                    Vector3* vectorPtr1 = &hitPoint;
                    vectorPtr1->y++;
                }
                Vector3 vector2 = tank.component.transform.TransformPoint(hitPoint);
                GameObject obj2 = Object.Instantiate<GameObject>(worldSpaceHUD.component.DamageInfoPrefab, worldSpaceHUD.component.gameObject.transform);
                obj2.transform.position = vector2;
                obj2.transform.rotation = Camera.main.transform.rotation;
                TextMeshProUGUI componentInChildren = obj2.GetComponentInChildren<TextMeshProUGUI>();
                componentInChildren.text = ((int) e.Damage).ToString();
                if (e.BackHit)
                {
                    componentInChildren.fontStyle = FontStyles.Bold;
                    obj2.GetComponent<Animator>().SetTrigger("Critical");
                }
                if (e.HealHit)
                {
                    obj2.GetComponent<Animator>().SetTrigger("Healing");
                }
            }
        }

        [OnEventFire]
        public void UpdateDamageTransform(UpdateEvent e, SingleNode<DamageInfoComponent> damageInfo, [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD)
        {
            DamageInfoComponent component = damageInfo.component;
            Transform elementTransform = component.transform;
            elementTransform.rotation = component.CachedCamera.transform.rotation;
            WorldSpaceHUDUtil.ScaleToRealSize(worldSpaceHUD.component.canvas.transform, elementTransform, component.CachedCamera);
        }
    }
}

