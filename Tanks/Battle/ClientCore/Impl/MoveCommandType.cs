namespace Tanks.Battle.ClientCore.Impl
{
    using System;

    [Flags]
    public enum MoveCommandType
    {
        NONE,
        TANK,
        WEAPON,
        FULL
    }
}

