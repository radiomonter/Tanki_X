namespace Platform.Tool.ClientUnityLogger.API
{
    using log4net.Appender;
    using log4net.Core;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UnityAppender : AppenderSkeleton
    {
        [CompilerGenerated]
        private static Func<Assembly, bool> <>f__am$cache0;

        protected override void Append(LoggingEvent loggingEvent)
        {
            string message = base.RenderLoggingEvent(loggingEvent);
            Level actualLogLevel = GetActualLogLevel(loggingEvent);
            if (Level.Compare(actualLogLevel, Level.Fatal) >= 0)
            {
                Debug.LogException(new FatalException(message));
            }
            else if (Level.Compare(actualLogLevel, Level.Error) >= 0)
            {
                Debug.LogError(message);
            }
            else if (Level.Compare(actualLogLevel, Level.Warn) >= 0)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.Log(message);
            }
        }

        private static Level GetActualLogLevel(LoggingEvent loggingEvent) => 
            loggingEvent.Level;

        private static bool IsRunningFromTest()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = a => a.FullName.StartsWith("NUnit");
            }
            return AppDomain.CurrentDomain.GetAssemblies().Any<Assembly>(<>f__am$cache0);
        }
    }
}

