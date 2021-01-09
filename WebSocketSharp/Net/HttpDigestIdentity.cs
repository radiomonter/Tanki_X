namespace WebSocketSharp.Net
{
    using System;
    using System.Collections.Specialized;
    using System.Security.Principal;

    public class HttpDigestIdentity : GenericIdentity
    {
        private NameValueCollection _parameters;

        internal HttpDigestIdentity(NameValueCollection parameters) : base(parameters["username"], "Digest")
        {
            this._parameters = parameters;
        }

        internal bool IsValid(string password, string realm, string method, string entity)
        {
            NameValueCollection parameters = new NameValueCollection(this._parameters) {
                ["password"] = password,
                ["realm"] = realm,
                ["method"] = method,
                ["entity"] = entity
            };
            return (this._parameters["response"] == AuthenticationResponse.CreateRequestDigest(parameters));
        }

        public string Algorithm =>
            this._parameters["algorithm"];

        public string Cnonce =>
            this._parameters["cnonce"];

        public string Nc =>
            this._parameters["nc"];

        public string Nonce =>
            this._parameters["nonce"];

        public string Opaque =>
            this._parameters["opaque"];

        public string Qop =>
            this._parameters["qop"];

        public string Realm =>
            this._parameters["realm"];

        public string Response =>
            this._parameters["response"];

        public string Uri =>
            this._parameters["uri"];
    }
}

