namespace Steamworks
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=8), CallbackIdentity(0xd3)]
    public struct ComputeNewPlayerCompatibilityResult_t
    {
        public const int k_iCallback = 0xd3;
        public EResult m_eResult;
        public int m_cPlayersThatDontLikeCandidate;
        public int m_cPlayersThatCandidateDoesntLike;
        public int m_cClanPlayersThatDontLikeCandidate;
        public CSteamID m_SteamIDCandidate;
    }
}

