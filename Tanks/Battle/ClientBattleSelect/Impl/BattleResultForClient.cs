namespace Tanks.Battle.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class BattleResultForClient
    {
        public UserResult FindUserResultByUserId(long id)
        {
            <FindUserResultByUserId>c__AnonStorey0 storey = new <FindUserResultByUserId>c__AnonStorey0 {
                id = id
            };
            Predicate<UserResult> match = new Predicate<UserResult>(storey.<>m__0);
            UserResult result = this.RedUsers.Find(match);
            if (result == null)
            {
                result = this.BlueUsers.Find(match);
                if (result != null)
                {
                    return result;
                }
                result = this.DmUsers.Find(match);
                if (result == null)
                {
                    throw new Exception("User result not found: " + storey.id);
                }
            }
            return result;
        }

        public override string ToString() => 
            EcsToStringUtil.ToStringWithProperties(this, "\n");

        public long BattleId { get; set; }

        public long MapId { get; set; }

        public Tanks.Battle.ClientCore.API.BattleMode BattleMode { get; set; }

        public BattleType MatchMakingModeType { get; set; }

        public bool Custom { get; set; }

        public int RedTeamScore { get; set; }

        public int BlueTeamScore { get; set; }

        public int DmScore { get; set; }

        public List<UserResult> RedUsers { get; set; }

        public List<UserResult> BlueUsers { get; set; }

        public List<UserResult> DmUsers { get; set; }

        public bool Spectator { get; set; }

        [ProtocolOptional]
        public PersonalBattleResultForClient PersonalResult { get; set; }

        [CompilerGenerated]
        private sealed class <FindUserResultByUserId>c__AnonStorey0
        {
            internal long id;

            internal bool <>m__0(UserResult r) => 
                r.UserId == this.id;
        }
    }
}

