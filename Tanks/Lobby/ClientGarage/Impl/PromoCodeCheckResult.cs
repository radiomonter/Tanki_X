namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;

    public enum PromoCodeCheckResult
    {
        VALID,
        NOT_FOUND,
        USED,
        EXPIRED,
        INVALID,
        OWNED
    }
}

