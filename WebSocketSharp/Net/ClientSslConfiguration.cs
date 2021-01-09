namespace WebSocketSharp.Net
{
    using System;
    using System.Net.Security;
    using System.Security.Authentication;
    using System.Security.Cryptography.X509Certificates;

    public class ClientSslConfiguration : SslConfiguration
    {
        private X509CertificateCollection _certs;
        private string _host;

        public ClientSslConfiguration(string targetHost) : this(targetHost, null, SslProtocols.Default, false)
        {
        }

        public ClientSslConfiguration(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation) : base(enabledSslProtocols, checkCertificateRevocation)
        {
            this._host = targetHost;
            this._certs = clientCertificates;
        }

        public X509CertificateCollection ClientCertificates
        {
            get => 
                this._certs;
            set => 
                this._certs = value;
        }

        public LocalCertificateSelectionCallback ClientCertificateSelectionCallback
        {
            get => 
                base.CertificateSelectionCallback;
            set => 
                base.CertificateSelectionCallback = value;
        }

        public RemoteCertificateValidationCallback ServerCertificateValidationCallback
        {
            get => 
                base.CertificateValidationCallback;
            set => 
                base.CertificateValidationCallback = value;
        }

        public string TargetHost
        {
            get => 
                this._host;
            set => 
                this._host = value;
        }
    }
}

