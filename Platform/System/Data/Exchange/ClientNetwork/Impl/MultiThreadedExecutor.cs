namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using System;
    using System.Threading;

    public class MultiThreadedExecutor : Executor
    {
        public void Execute(Action action)
        {
            new Thread(new ThreadStart(action.Invoke)).Start();
        }
    }
}

