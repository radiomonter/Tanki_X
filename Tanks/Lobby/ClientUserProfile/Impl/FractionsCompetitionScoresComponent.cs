namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class FractionsCompetitionScoresComponent : Component
    {
        public long TotalCryFund;
        public Dictionary<long, long> Scores;
    }
}

