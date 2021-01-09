namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;

    public interface Executor
    {
        void Execute(Action action);
    }
}

