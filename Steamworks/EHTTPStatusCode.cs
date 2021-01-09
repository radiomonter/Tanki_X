namespace Steamworks
{
    using System;

    public enum EHTTPStatusCode
    {
        k_EHTTPStatusCodeInvalid = 0,
        k_EHTTPStatusCode100Continue = 100,
        k_EHTTPStatusCode101SwitchingProtocols = 0x65,
        k_EHTTPStatusCode200OK = 200,
        k_EHTTPStatusCode201Created = 0xc9,
        k_EHTTPStatusCode202Accepted = 0xca,
        k_EHTTPStatusCode203NonAuthoritative = 0xcb,
        k_EHTTPStatusCode204NoContent = 0xcc,
        k_EHTTPStatusCode205ResetContent = 0xcd,
        k_EHTTPStatusCode206PartialContent = 0xce,
        k_EHTTPStatusCode300MultipleChoices = 300,
        k_EHTTPStatusCode301MovedPermanently = 0x12d,
        k_EHTTPStatusCode302Found = 0x12e,
        k_EHTTPStatusCode303SeeOther = 0x12f,
        k_EHTTPStatusCode304NotModified = 0x130,
        k_EHTTPStatusCode305UseProxy = 0x131,
        k_EHTTPStatusCode307TemporaryRedirect = 0x133,
        k_EHTTPStatusCode400BadRequest = 400,
        k_EHTTPStatusCode401Unauthorized = 0x191,
        k_EHTTPStatusCode402PaymentRequired = 0x192,
        k_EHTTPStatusCode403Forbidden = 0x193,
        k_EHTTPStatusCode404NotFound = 0x194,
        k_EHTTPStatusCode405MethodNotAllowed = 0x195,
        k_EHTTPStatusCode406NotAcceptable = 0x196,
        k_EHTTPStatusCode407ProxyAuthRequired = 0x197,
        k_EHTTPStatusCode408RequestTimeout = 0x198,
        k_EHTTPStatusCode409Conflict = 0x199,
        k_EHTTPStatusCode410Gone = 410,
        k_EHTTPStatusCode411LengthRequired = 0x19b,
        k_EHTTPStatusCode412PreconditionFailed = 0x19c,
        k_EHTTPStatusCode413RequestEntityTooLarge = 0x19d,
        k_EHTTPStatusCode414RequestURITooLong = 0x19e,
        k_EHTTPStatusCode415UnsupportedMediaType = 0x19f,
        k_EHTTPStatusCode416RequestedRangeNotSatisfiable = 0x1a0,
        k_EHTTPStatusCode417ExpectationFailed = 0x1a1,
        k_EHTTPStatusCode4xxUnknown = 0x1a2,
        k_EHTTPStatusCode429TooManyRequests = 0x1ad,
        k_EHTTPStatusCode500InternalServerError = 500,
        k_EHTTPStatusCode501NotImplemented = 0x1f5,
        k_EHTTPStatusCode502BadGateway = 0x1f6,
        k_EHTTPStatusCode503ServiceUnavailable = 0x1f7,
        k_EHTTPStatusCode504GatewayTimeout = 0x1f8,
        k_EHTTPStatusCode505HTTPVersionNotSupported = 0x1f9,
        k_EHTTPStatusCode5xxUnknown = 0x257
    }
}

