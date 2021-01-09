namespace log4net.Core
{
    using System;

    public class ExceptionEvaluator : ITriggeringEventEvaluator
    {
        private Type m_type;
        private bool m_triggerOnSubclass;

        public ExceptionEvaluator()
        {
        }

        public ExceptionEvaluator(Type exType, bool triggerOnSubClass)
        {
            if (exType == null)
            {
                throw new ArgumentNullException("exType");
            }
            this.m_type = exType;
            this.m_triggerOnSubclass = triggerOnSubClass;
        }

        public bool IsTriggeringEvent(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if (!this.m_triggerOnSubclass || (loggingEvent.ExceptionObject == null))
            {
                return (!this.m_triggerOnSubclass && ((loggingEvent.ExceptionObject != null) && ReferenceEquals(loggingEvent.ExceptionObject.GetType(), this.m_type)));
            }
            Type objA = loggingEvent.ExceptionObject.GetType();
            return (ReferenceEquals(objA, this.m_type) || objA.IsSubclassOf(this.m_type));
        }

        public Type ExceptionType
        {
            get => 
                this.m_type;
            set => 
                this.m_type = value;
        }

        public bool TriggerOnSubclass
        {
            get => 
                this.m_triggerOnSubclass;
            set => 
                this.m_triggerOnSubclass = value;
        }
    }
}

