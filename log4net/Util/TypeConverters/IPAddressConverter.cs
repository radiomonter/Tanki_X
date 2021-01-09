namespace log4net.Util.TypeConverters
{
    using System;
    using System.Net;

    internal class IPAddressConverter : IConvertFrom
    {
        private static readonly char[] validIpAddressChars = new char[] { 
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f',
            'A', 'B', 'C', 'D', 'E', 'F', 'x', 'X', '.', ':', '%'
        };

        public bool CanConvertFrom(Type sourceType) => 
            ReferenceEquals(sourceType, typeof(string));

        public object ConvertFrom(object source)
        {
            string ipString = source as string;
            if ((ipString != null) && (ipString.Length > 0))
            {
                try
                {
                    IPAddress address;
                    if (!IPAddress.TryParse(ipString, out address))
                    {
                        IPHostEntry hostEntry = Dns.GetHostEntry(ipString);
                        if (((hostEntry != null) && ((hostEntry.AddressList != null) && (hostEntry.AddressList.Length > 0))) && (hostEntry.AddressList[0] != null))
                        {
                            return hostEntry.AddressList[0];
                        }
                    }
                    else
                    {
                        return address;
                    }
                }
                catch (Exception exception)
                {
                    throw ConversionNotSupportedException.Create(typeof(IPAddress), source, exception);
                }
            }
            throw ConversionNotSupportedException.Create(typeof(IPAddress), source);
        }
    }
}

