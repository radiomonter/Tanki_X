namespace log4net.Filter
{
    using log4net.Core;
    using System;

    public abstract class FilterSkeleton : IFilter, IOptionHandler
    {
        private IFilter m_next;

        protected FilterSkeleton()
        {
        }

        public virtual void ActivateOptions()
        {
        }

        public abstract FilterDecision Decide(LoggingEvent loggingEvent);

        public IFilter Next
        {
            get => 
                this.m_next;
            set => 
                this.m_next = value;
        }
    }
}

