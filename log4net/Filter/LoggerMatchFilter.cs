namespace log4net.Filter
{
    using log4net.Core;
    using System;

    public class LoggerMatchFilter : FilterSkeleton
    {
        private bool m_acceptOnMatch = true;
        private string m_loggerToMatch;

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            return (((this.m_loggerToMatch == null) || ((this.m_loggerToMatch.Length == 0) || !loggingEvent.LoggerName.StartsWith(this.m_loggerToMatch))) ? FilterDecision.Neutral : (!this.m_acceptOnMatch ? FilterDecision.Deny : FilterDecision.Accept));
        }

        public bool AcceptOnMatch
        {
            get => 
                this.m_acceptOnMatch;
            set => 
                this.m_acceptOnMatch = value;
        }

        public string LoggerToMatch
        {
            get => 
                this.m_loggerToMatch;
            set => 
                this.m_loggerToMatch = value;
        }
    }
}

