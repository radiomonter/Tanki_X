namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;

    public class CantFindAssetGuidException : Exception
    {
        public CantFindAssetGuidException(long marketItemId) : base($"marketItemId {marketItemId}")
        {
        }
    }
}

