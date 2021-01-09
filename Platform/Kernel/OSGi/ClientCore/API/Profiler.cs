namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Profiler
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Action<string> OnBeginSample;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Action OnEndSample;

        public static event Action<string> OnBeginSample
        {
            add
            {
                Action<string> onBeginSample = OnBeginSample;
                while (true)
                {
                    Action<string> objB = onBeginSample;
                    onBeginSample = Interlocked.CompareExchange<Action<string>>(ref OnBeginSample, objB + value, onBeginSample);
                    if (ReferenceEquals(onBeginSample, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action<string> onBeginSample = OnBeginSample;
                while (true)
                {
                    Action<string> objB = onBeginSample;
                    onBeginSample = Interlocked.CompareExchange<Action<string>>(ref OnBeginSample, objB - value, onBeginSample);
                    if (ReferenceEquals(onBeginSample, objB))
                    {
                        return;
                    }
                }
            }
        }

        public static event Action OnEndSample
        {
            add
            {
                Action onEndSample = OnEndSample;
                while (true)
                {
                    Action objB = onEndSample;
                    onEndSample = Interlocked.CompareExchange<Action>(ref OnEndSample, objB + value, onEndSample);
                    if (ReferenceEquals(onEndSample, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action onEndSample = OnEndSample;
                while (true)
                {
                    Action objB = onEndSample;
                    onEndSample = Interlocked.CompareExchange<Action>(ref OnEndSample, objB - value, onEndSample);
                    if (ReferenceEquals(onEndSample, objB))
                    {
                        return;
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        public static void BeginSample(string name)
        {
            if (OnBeginSample != null)
            {
                OnBeginSample(name);
            }
        }

        [Conditional("DEBUG")]
        public static void EndSample()
        {
            if (OnEndSample != null)
            {
                OnEndSample();
            }
        }
    }
}

