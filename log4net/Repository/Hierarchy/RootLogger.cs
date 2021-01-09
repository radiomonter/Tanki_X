namespace log4net.Repository.Hierarchy
{
    using log4net.Core;
    using log4net.Util;
    using System;

    public class RootLogger : Logger
    {
        private static readonly Type declaringType = typeof(RootLogger);

        public RootLogger(log4net.Core.Level level) : base("root")
        {
            this.Level = level;
        }

        public override log4net.Core.Level EffectiveLevel =>
            base.Level;

        public override log4net.Core.Level Level
        {
            get => 
                base.Level;
            set
            {
                if (value == null)
                {
                    LogLog.Error(declaringType, "You have tried to set a null level to root.", new LogException());
                }
                else
                {
                    base.Level = value;
                }
            }
        }
    }
}

