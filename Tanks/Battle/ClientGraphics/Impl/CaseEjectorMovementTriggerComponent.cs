namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class CaseEjectorMovementTriggerComponent : AnimationTriggerComponent
    {
        private void OnCaseEjectorClose()
        {
            base.ProvideEvent<CaseEjectorCloseEvent>();
        }

        private void OnCaseEjectorOpen()
        {
            base.ProvideEvent<CaseEjectorOpenEvent>();
        }
    }
}

