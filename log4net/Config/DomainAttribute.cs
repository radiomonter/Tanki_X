namespace log4net.Config
{
    using System;

    [Serializable, AttributeUsage(AttributeTargets.Assembly), Obsolete("Use RepositoryAttribute instead of DomainAttribute")]
    public sealed class DomainAttribute : RepositoryAttribute
    {
        public DomainAttribute()
        {
        }

        public DomainAttribute(string name) : base(name)
        {
        }
    }
}

