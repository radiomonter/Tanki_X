namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class IsisTargetEvaluatorComponent : Component
    {
        public int? LastDirectionIndex { get; set; }
    }
}

