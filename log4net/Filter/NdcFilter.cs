namespace log4net.Filter
{
    using System;

    public class NdcFilter : PropertyFilter
    {
        public NdcFilter()
        {
            base.Key = "NDC";
        }
    }
}

