namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine.Profiling;

    public static class UnityProfiler
    {
        [CompilerGenerated]
        private static Action<string> <>f__mg$cache0;
        [CompilerGenerated]
        private static Action<string> <>f__mg$cache1;
        [CompilerGenerated]
        private static Action <>f__mg$cache2;
        [CompilerGenerated]
        private static Action <>f__mg$cache3;
        [CompilerGenerated]
        private static Func<string, bool> <>f__am$cache0;

        public static void Listen()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs != null)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = arg => "-profiler".Equals(arg);
                }
                if (commandLineArgs.Any<string>(<>f__am$cache0))
                {
                    Profiler.enabled = true;
                }
            }
            <>f__mg$cache0 ??= new Action<string>(UnityProfiler.OnBeginSample);
            Profiler.OnBeginSample -= <>f__mg$cache0;
            <>f__mg$cache1 ??= new Action<string>(UnityProfiler.OnBeginSample);
            Profiler.OnBeginSample += <>f__mg$cache1;
            <>f__mg$cache2 ??= new Action(UnityProfiler.OnEndSample);
            Profiler.OnEndSample -= <>f__mg$cache2;
            <>f__mg$cache3 ??= new Action(UnityProfiler.OnEndSample);
            Profiler.OnEndSample += <>f__mg$cache3;
        }

        public static void OnBeginSample(string name)
        {
        }

        public static void OnEndSample()
        {
        }
    }
}

