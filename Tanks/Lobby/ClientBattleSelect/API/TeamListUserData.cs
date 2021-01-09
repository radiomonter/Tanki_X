namespace Tanks.Lobby.ClientBattleSelect.API
{
    using System;

    public class TeamListUserData
    {
        public long userEntityId;
        public string userUid;
        public string hullName;
        public string hullIconId;
        public string turretName;
        public string turretIconId;
        public bool ready;

        public TeamListUserData(string userId)
        {
            this.userUid = userId;
        }

        public TeamListUserData(long userEntityId, string userUid, bool ready)
        {
            this.userEntityId = userEntityId;
            this.userUid = userUid;
            this.ready = ready;
        }

        public TeamListUserData(long userEntityId, string userUid, string hullName, string hullIconId, string turretName, string turretIconId, bool ready)
        {
            this.userEntityId = userEntityId;
            this.userUid = userUid;
            this.hullName = hullName;
            this.hullIconId = hullIconId;
            this.turretName = turretName;
            this.turretIconId = turretIconId;
            this.ready = ready;
        }
    }
}

