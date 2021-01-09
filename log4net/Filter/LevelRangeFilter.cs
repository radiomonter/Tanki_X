namespace log4net.Filter
{
    using log4net.Core;
    using System;

    public class LevelRangeFilter : FilterSkeleton
    {
        private bool m_acceptOnMatch = true;
        private Level m_levelMin;
        private Level m_levelMax;

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            return (((this.m_levelMin == null) || (loggingEvent.Level >= this.m_levelMin)) ? (((this.m_levelMax == null) || (loggingEvent.Level <= this.m_levelMax)) ? (!this.m_acceptOnMatch ? FilterDecision.Neutral : FilterDecision.Accept) : FilterDecision.Deny) : FilterDecision.Deny);
        }

        public bool AcceptOnMatch
        {
            get => 
                this.m_acceptOnMatch;
            set => 
                this.m_acceptOnMatch = value;
        }

        public Level LevelMin
        {
            get => 
                this.m_levelMin;
            set => 
                this.m_levelMin = value;
        }

        public Level LevelMax
        {
            get => 
                this.m_levelMax;
            set => 
                this.m_levelMax = value;
        }
    }
}

