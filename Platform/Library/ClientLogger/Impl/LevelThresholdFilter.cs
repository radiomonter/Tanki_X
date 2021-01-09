namespace Platform.Library.ClientLogger.Impl
{
    using log4net.Core;
    using log4net.Filter;
    using System;
    using System.Runtime.CompilerServices;

    public class LevelThresholdFilter : FilterSkeleton
    {
        public LevelThresholdFilter()
        {
            this.ThresholdLevel = Level.All;
        }

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException(loggingEvent.GetType().FullName);
            }
            return ((loggingEvent.Level < this.ThresholdLevel) ? FilterDecision.Neutral : FilterDecision.Accept);
        }

        public Level ThresholdLevel { get; set; }
    }
}

