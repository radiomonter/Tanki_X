namespace YamlDotNet
{
    using System;
    using System.Globalization;

    internal sealed class CultureInfoAdapter : CultureInfo
    {
        private readonly IFormatProvider _provider;

        public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider) : base(baseCulture.LCID)
        {
            this._provider = provider;
        }

        public override object GetFormat(Type formatType) => 
            this._provider.GetFormat(formatType);
    }
}

