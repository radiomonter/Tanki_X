namespace log4net.Filter
{
    using log4net.Core;
    using System;

    public class PropertyFilter : StringMatchFilter
    {
        private string m_key;

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if (this.m_key == null)
            {
                return FilterDecision.Neutral;
            }
            object property = loggingEvent.LookupProperty(this.m_key);
            string input = loggingEvent.Repository.RendererMap.FindAndRender(property);
            return (((input == null) || ((base.m_stringToMatch == null) && (base.m_regexToMatch == null))) ? FilterDecision.Neutral : ((base.m_regexToMatch == null) ? ((base.m_stringToMatch == null) ? FilterDecision.Neutral : ((input.IndexOf(base.m_stringToMatch) != -1) ? (!base.m_acceptOnMatch ? FilterDecision.Deny : FilterDecision.Accept) : FilterDecision.Neutral)) : (base.m_regexToMatch.Match(input).Success ? (!base.m_acceptOnMatch ? FilterDecision.Deny : FilterDecision.Accept) : FilterDecision.Neutral)));
        }

        public string Key
        {
            get => 
                this.m_key;
            set => 
                this.m_key = value;
        }
    }
}

