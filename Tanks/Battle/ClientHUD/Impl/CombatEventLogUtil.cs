namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public static class CombatEventLogUtil
    {
        public static string ApplyPlaceholder(string message, string placeholder, int rank, string uid, Color color)
        {
            object[] objArray1 = new object[] { "{", rank, ":", ColorUtility.ToHtmlStringRGB(color), ":", uid, "}" };
            return message.Replace(placeholder, string.Concat(objArray1));
        }

        public static Color GetTeamColor(TeamColor teamColor, CombatEventLogComponent combatEventLog) => 
            (teamColor == TeamColor.BLUE) ? combatEventLog.BlueTeamColor : ((teamColor == TeamColor.RED) ? combatEventLog.RedTeamColor : combatEventLog.NeutralColor);

        public static UILog GetUILog(CombatEventLogComponent combatEventLog) => 
            combatEventLog.gameObject.GetComponent<UILog>();
    }
}

