namespace Tanks.Battle.ClientCore.Impl
{
    using System;

    public class UnknownRegionTypeException : Exception
    {
        public UnknownRegionTypeException(BonusType bonusType) : base(bonusType.ToString())
        {
        }
    }
}

