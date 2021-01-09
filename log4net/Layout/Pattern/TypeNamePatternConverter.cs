namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using System;

    internal sealed class TypeNamePatternConverter : NamedPatternConverter
    {
        protected override string GetFullyQualifiedName(LoggingEvent loggingEvent) => 
            loggingEvent.LocationInformation.ClassName;
    }
}

