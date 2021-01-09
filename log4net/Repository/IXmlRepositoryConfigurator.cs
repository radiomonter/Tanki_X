namespace log4net.Repository
{
    using System;
    using System.Xml;

    public interface IXmlRepositoryConfigurator
    {
        void Configure(XmlElement element);
    }
}

