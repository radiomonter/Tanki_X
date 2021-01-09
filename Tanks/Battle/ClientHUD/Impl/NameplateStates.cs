﻿namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class NameplateStates
    {
        public class NameplateAppearanceState : Node
        {
            public NameplateAppearanceStateComponent nameplateAppearanceState;
        }

        public class NameplateConcealmentState : Node
        {
            public NameplateConcealmentStateComponent nameplateConcealmentState;
        }

        public class NameplateDeletionState : Node
        {
            public NameplateDeletionStateComponent nameplateDeletionState;
        }

        public class NameplateInvisibilityEffectState : Node
        {
            public NameplateConcealmentStateComponent nameplateConcealmentState;
            public NameplateInvisibilityEffectStateComponent nameplateInvisibilityEffectState;
        }
    }
}

