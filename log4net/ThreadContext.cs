namespace log4net
{
    using log4net.Util;
    using System;

    public sealed class ThreadContext
    {
        private static readonly ThreadContextProperties s_properties = new ThreadContextProperties();
        private static readonly ThreadContextStacks s_stacks = new ThreadContextStacks(s_properties);

        private ThreadContext()
        {
        }

        public static ThreadContextProperties Properties =>
            s_properties;

        public static ThreadContextStacks Stacks =>
            s_stacks;
    }
}

