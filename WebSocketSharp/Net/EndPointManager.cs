namespace WebSocketSharp.Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    internal sealed class EndPointManager
    {
        private static readonly Dictionary<IPAddress, Dictionary<int, EndPointListener>> _addressToEndpoints = new Dictionary<IPAddress, Dictionary<int, EndPointListener>>();

        private EndPointManager()
        {
        }

        public static void AddListener(HttpListener listener)
        {
            List<string> list = new List<string>();
            Monitor.Enter(((ICollection) _addressToEndpoints).SyncRoot);
            try
            {
                foreach (string str in listener.Prefixes)
                {
                    addPrefix(str, listener);
                    list.Add(str);
                }
            }
            catch
            {
                foreach (string str2 in list)
                {
                    removePrefix(str2, listener);
                }
                throw;
            }
            finally
            {
                object obj2;
                Monitor.Exit(obj2);
            }
        }

        private static void addPrefix(string uriPrefix, HttpListener listener)
        {
            HttpListenerPrefix prefix = new HttpListenerPrefix(uriPrefix);
            string path = prefix.Path;
            if (path.IndexOf('%') != -1)
            {
                throw new HttpListenerException(0x57, "Includes an invalid path.");
            }
            if (path.IndexOf("//", StringComparison.Ordinal) != -1)
            {
                throw new HttpListenerException(0x57, "Includes an invalid path.");
            }
            getEndPointListener(prefix, listener).AddPrefix(prefix, listener);
        }

        public static void AddPrefix(string uriPrefix, HttpListener listener)
        {
            lock (((ICollection) _addressToEndpoints).SyncRoot)
            {
                addPrefix(uriPrefix, listener);
            }
        }

        private static IPAddress convertToIPAddress(string hostname)
        {
            IPAddress address;
            if ((hostname == "*") || (hostname == "+"))
            {
                return IPAddress.Any;
            }
            if (IPAddress.TryParse(hostname, out address))
            {
                return address;
            }
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                return ((hostEntry == null) ? IPAddress.Any : hostEntry.AddressList[0]);
            }
            catch
            {
                return IPAddress.Any;
            }
        }

        private static EndPointListener getEndPointListener(HttpListenerPrefix prefix, HttpListener listener)
        {
            IPAddress key = convertToIPAddress(prefix.Host);
            Dictionary<int, EndPointListener> dictionary = null;
            if (_addressToEndpoints.ContainsKey(key))
            {
                dictionary = _addressToEndpoints[key];
            }
            else
            {
                dictionary = new Dictionary<int, EndPointListener>();
                _addressToEndpoints[key] = dictionary;
            }
            int port = prefix.Port;
            EndPointListener listener2 = null;
            if (dictionary.ContainsKey(port))
            {
                listener2 = dictionary[port];
            }
            else
            {
                listener2 = new EndPointListener(key, port, listener.ReuseAddress, prefix.IsSecure, listener.CertificateFolderPath, listener.SslConfiguration);
                dictionary[port] = listener2;
            }
            return listener2;
        }

        internal static void RemoveEndPoint(EndPointListener listener)
        {
            lock (((ICollection) _addressToEndpoints).SyncRoot)
            {
                IPAddress key = listener.Address;
                Dictionary<int, EndPointListener> dictionary = _addressToEndpoints[key];
                dictionary.Remove(listener.Port);
                if (dictionary.Count == 0)
                {
                    _addressToEndpoints.Remove(key);
                }
                listener.Close();
            }
        }

        public static void RemoveListener(HttpListener listener)
        {
            lock (((ICollection) _addressToEndpoints).SyncRoot)
            {
                foreach (string str in listener.Prefixes)
                {
                    removePrefix(str, listener);
                }
            }
        }

        private static void removePrefix(string uriPrefix, HttpListener listener)
        {
            HttpListenerPrefix prefix = new HttpListenerPrefix(uriPrefix);
            string path = prefix.Path;
            if ((path.IndexOf('%') == -1) && (path.IndexOf("//", StringComparison.Ordinal) == -1))
            {
                getEndPointListener(prefix, listener).RemovePrefix(prefix, listener);
            }
        }

        public static void RemovePrefix(string uriPrefix, HttpListener listener)
        {
            lock (((ICollection) _addressToEndpoints).SyncRoot)
            {
                removePrefix(uriPrefix, listener);
            }
        }
    }
}

