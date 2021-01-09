namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;

    public class GoBackSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayGoBackSound(GoBackEvent evt, SingleNode<GoBackSoundEffectComponent> effect)
        {
            effect.component.PlaySoundEffect();
        }
    }
}

