namespace Platform.System.Data.Exchange.ClientNetwork.API
{
    using System;

    public interface ServerTimeService
    {
        event Action<long> OnInitServerTime;

        long InitialServerTime { get; }
    }
}

