namespace Tanks.Lobby.ClientControls.API
{
    using TMPro;
    using UnityEngine;

    public class CombatEventLogUser : BaseCombatLogMessageElement
    {
        [SerializeField]
        private ImageListSkin rankIcon;
        [SerializeField]
        private TextMeshProUGUI userName;

        public ImageListSkin RankIcon =>
            this.rankIcon;

        public TextMeshProUGUI UserName =>
            this.userName;
    }
}

