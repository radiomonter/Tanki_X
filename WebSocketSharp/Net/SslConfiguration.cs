namespace WebSocketSharp.Net
{
    using System;
    using System.Net.Security;
    using System.Runtime.CompilerServices;
    using System.Security.Authentication;
    using System.Security.Cryptography.X509Certificates;

    public abstract class SslConfiguration
    {
        private LocalCertificateSelectionCallback _certSelectionCallback;
        private RemoteCertificateValidationCallback _certValidationCallback;
        private bool _checkCertRevocation;
        private SslProtocols _enabledProtocols;
        [CompilerGenerated]
        private static LocalCertificateSelectionCallback <>f__am$cache0;
        [CompilerGenerated]
        private static RemoteCertificateValidationCallback <>f__am$cache1;

        protected SslConfiguration(SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
        {
            this._enabledProtocols = enabledSslProtocols;
            this._checkCertRevocation = checkCertificateRevocation;
        }

        protected LocalCertificateSelectionCallback CertificateSelectionCallback
        {
            get
            {
                LocalCertificateSelectionCallback callback2 = this._certSelectionCallback;
                if (this._certSelectionCallback == null)
                {
                    LocalCertificateSelectionCallback local1 = this._certSelectionCallback;
                    <>f__am$cache0 ??= (sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => null;
                    callback2 = this._certSelectionCallback = <>f__am$cache0;
                }
                return callback2;
            }
            set => 
                this._certSelectionCallback = value;
        }

        protected RemoteCertificateValidationCallback CertificateValidationCallback
        {
            get
            {
                RemoteCertificateValidationCallback callback2 = this._certValidationCallback;
                if (this._certValidationCallback == null)
                {
                    RemoteCertificateValidationCallback local1 = this._certValidationCallback;
                    <>f__am$cache1 ??= (sender, certificate, chain, sslPolicyErrors) => true;
                    callback2 = this._certValidationCallback = <>f__am$cache1;
                }
                return callback2;
            }
            set => 
                this._certValidationCallback = value;
        }

        public bool CheckCertificateRevocation
        {
            get => 
                this._checkCertRevocation;
            set => 
                this._checkCertRevocation = value;
        }

        public SslProtocols EnabledSslProtocols
        {
            get => 
                this._enabledProtocols;
            set => 
                this._enabledProtocols = value;
        }
    }
}

