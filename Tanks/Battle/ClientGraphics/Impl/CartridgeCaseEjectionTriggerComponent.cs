namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class CartridgeCaseEjectionTriggerComponent : AnimationTriggerComponent
    {
        public void OnCaseEject()
        {
            base.ProvideEvent<CartridgeCaseEjectionEvent>();
        }
    }
}

