﻿namespace Steamworks
{
    using System;

    public enum ECheckFileSignature
    {
        k_ECheckFileSignatureInvalidSignature,
        k_ECheckFileSignatureValidSignature,
        k_ECheckFileSignatureFileNotFound,
        k_ECheckFileSignatureNoSignaturesFoundForThisApp,
        k_ECheckFileSignatureNoSignaturesFoundForThisFile
    }
}

