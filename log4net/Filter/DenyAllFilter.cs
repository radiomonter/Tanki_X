namespace log4net.Filter
{
    using log4net.Core;

    public sealed class DenyAllFilter : FilterSkeleton
    {
        public override FilterDecision Decide(LoggingEvent loggingEvent) => 
            FilterDecision.Deny;
    }
}

