namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class EnergyInjectionEffectHUDSystem : ECSSystem
    {
        [OnEventFire]
        public void ExecuteEnergyInjection(ExecuteEnergyInjectionEvent e, SingleNode<WeaponEnergyComponent> weapon, SingleNode<EnergyInjectionEffectComponent> effect, [JoinAll] SingleNode<MainHUDComponent> hud)
        {
            weapon.component.Energy += effect.component.ReloadEnergyPercent;
            Mathf.Clamp(weapon.component.Energy, 0f, 1f);
            hud.component.EnergyInjectionBlink(true);
        }
    }
}

