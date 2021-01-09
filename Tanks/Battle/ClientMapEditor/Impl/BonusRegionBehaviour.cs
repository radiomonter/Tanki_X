namespace Tanks.Battle.ClientMapEditor.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class BonusRegionBehaviour : EditorBehavior
    {
        public BattleMode battleMode;
        public BonusType bonusType;

        public void Initialize(BattleMode battleMode, BonusType bonusType)
        {
            this.battleMode = battleMode;
            this.bonusType = bonusType;
        }
    }
}

