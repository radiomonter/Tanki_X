namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class ShaftShotAnimationTriggerComponent : AnimationTriggerComponent
    {
        private void OnCooldownClosing()
        {
            base.ProvideEvent<ShaftShotAnimationCooldownClosingEvent>();
        }

        private void OnCooldownEnd()
        {
            base.ProvideEvent<ShaftShotAnimationCooldownEndEvent>();
        }

        private void OnCooldownStart()
        {
            base.ProvideEvent<ShaftShotAnimationCooldownStartEvent>();
        }
    }
}

