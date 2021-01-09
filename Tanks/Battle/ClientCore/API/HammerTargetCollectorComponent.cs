namespace Tanks.Battle.ClientCore.API
{
    using System;
    using Tanks.Battle.ClientCore.Impl;

    public class HammerTargetCollectorComponent : TargetCollectorComponent
    {
        public HammerTargetCollectorComponent(TargetCollector targetCollector, TargetValidator targetValidator) : base(targetCollector, targetValidator)
        {
        }
    }
}

