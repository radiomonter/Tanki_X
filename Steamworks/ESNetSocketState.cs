namespace Steamworks
{
    using System;

    public enum ESNetSocketState
    {
        k_ESNetSocketStateInvalid = 0,
        k_ESNetSocketStateConnected = 1,
        k_ESNetSocketStateInitiated = 10,
        k_ESNetSocketStateLocalCandidatesFound = 11,
        k_ESNetSocketStateReceivedRemoteCandidates = 12,
        k_ESNetSocketStateChallengeHandshake = 15,
        k_ESNetSocketStateDisconnecting = 0x15,
        k_ESNetSocketStateLocalDisconnect = 0x16,
        k_ESNetSocketStateTimeoutDuringConnect = 0x17,
        k_ESNetSocketStateRemoteEndDisconnected = 0x18,
        k_ESNetSocketStateConnectionBroken = 0x19
    }
}

