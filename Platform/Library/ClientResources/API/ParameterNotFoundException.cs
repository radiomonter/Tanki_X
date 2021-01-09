namespace Platform.Library.ClientResources.API
{
    using System;

    public class ParameterNotFoundException : Exception
    {
        public ParameterNotFoundException(string paramName) : base(paramName)
        {
        }
    }
}

