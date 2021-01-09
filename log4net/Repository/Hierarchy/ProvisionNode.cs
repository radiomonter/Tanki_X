namespace log4net.Repository.Hierarchy
{
    using System;
    using System.Collections;

    internal sealed class ProvisionNode : ArrayList
    {
        internal ProvisionNode(Logger log)
        {
            this.Add(log);
        }
    }
}

