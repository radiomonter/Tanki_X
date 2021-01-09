namespace log4net.Filter
{
    using log4net.Core;
    using System;
    using System.Text.RegularExpressions;

    public class StringMatchFilter : FilterSkeleton
    {
        protected bool m_acceptOnMatch = true;
        protected string m_stringToMatch;
        protected string m_stringRegexToMatch;
        protected Regex m_regexToMatch;

        public override void ActivateOptions()
        {
            if (this.m_stringRegexToMatch != null)
            {
                this.m_regexToMatch = new Regex(this.m_stringRegexToMatch, RegexOptions.None);
            }
        }

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            string renderedMessage = loggingEvent.RenderedMessage;
            return (((renderedMessage == null) || ((this.m_stringToMatch == null) && (this.m_regexToMatch == null))) ? FilterDecision.Neutral : ((this.m_regexToMatch == null) ? ((this.m_stringToMatch == null) ? FilterDecision.Neutral : ((renderedMessage.IndexOf(this.m_stringToMatch) != -1) ? (!this.m_acceptOnMatch ? FilterDecision.Deny : FilterDecision.Accept) : FilterDecision.Neutral)) : (this.m_regexToMatch.Match(renderedMessage).Success ? (!this.m_acceptOnMatch ? FilterDecision.Deny : FilterDecision.Accept) : FilterDecision.Neutral)));
        }

        public bool AcceptOnMatch
        {
            get => 
                this.m_acceptOnMatch;
            set => 
                this.m_acceptOnMatch = value;
        }

        public string StringToMatch
        {
            get => 
                this.m_stringToMatch;
            set => 
                this.m_stringToMatch = value;
        }

        public string RegexToMatch
        {
            get => 
                this.m_stringRegexToMatch;
            set => 
                this.m_stringRegexToMatch = value;
        }
    }
}

