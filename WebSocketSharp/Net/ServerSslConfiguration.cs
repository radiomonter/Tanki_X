namespace WebSocketSharp.Net
{
    using System;
    using System.Net.Security;
    using System.Security.Authentication;
    using System.Security.Cryptography.X509Certificates;

    public class ServerSslConfiguration : SslConfiguration
    {
        private X509Certificate2 _cert;
        private bool _clientCertRequired;

        public ServerSslConfiguration(X509Certificate2 serverCertificate) : this(serverCertificate, false, SslProtocols.Default, false)
        {
        }

        public ServerSslConfiguration(X509Certificate2 serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation) : base(enabledSslProtocols, checkCertificateRevocation)
        {
            this._cert = serverCertificate;
            this._clientCertRequired = clientCertificateRequired;
        }

        public bool ClientCertificateRequired
        {
            get => 
                this._clientCertRequired;
            set => 
                this._clientCertRequired = value;
        }

        public RemoteCertificateValidationCallback ClientCertificateValidationCallback
        {
            get => 
                base.CertificateValidationCallback;
            set => 
                base.CertificateValidationCallback = value;
        }

        public X509Certificate2 ServerCertificate
        {
            get => 
                this._cert;
            set => 
                this._cert = value;
        }
    }
}

