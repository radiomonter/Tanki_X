namespace Tanks.Lobby.ClientControls.API
{
    using TMPro;
    using UnityEngine;

    public class CombatEventLogText : BaseCombatLogMessageElement
    {
        [SerializeField]
        private TMP_Text text;

        public TMP_Text Text =>
            this.text;
    }
}

