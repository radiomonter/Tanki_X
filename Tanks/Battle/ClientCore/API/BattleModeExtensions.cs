namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Runtime.CompilerServices;

    public static class BattleModeExtensions
    {
        public static Type GetBattleModeComponent(this BattleMode battleMode)
        {
            if (battleMode == BattleMode.DM)
            {
                return typeof(DMComponent);
            }
            if (battleMode == BattleMode.TDM)
            {
                return typeof(TDMComponent);
            }
            if (battleMode == BattleMode.CTF)
            {
                return typeof(CTFComponent);
            }
            if (battleMode != BattleMode.CP)
            {
                throw new Exception();
            }
            return typeof(CPComponent);
        }
    }
}

