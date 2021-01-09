namespace log4net.Util
{
    using log4net.Core;
    using System;

    public abstract class LevelMappingEntry : IOptionHandler
    {
        private log4net.Core.Level m_level;

        protected LevelMappingEntry()
        {
        }

        public virtual void ActivateOptions()
        {
        }

        public log4net.Core.Level Level
        {
            get => 
                this.m_level;
            set => 
                this.m_level = value;
        }
    }
}

