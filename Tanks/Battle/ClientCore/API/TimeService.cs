namespace Tanks.Battle.ClientCore.API
{
    using System;

    public interface TimeService
    {
        void InitServerTime(long serverTime);
        void SetDiffToServerWithSmoothing(long newDiffToServer);

        long DiffToServer { get; set; }
    }
}

