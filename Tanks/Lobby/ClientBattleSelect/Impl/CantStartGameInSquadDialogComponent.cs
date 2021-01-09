namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;

    public class CantStartGameInSquadDialogComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private TextMeshProUGUI label;
        [SerializeField]
        private LocalizedField cantSearch;
        [SerializeField]
        private LocalizedField cantCreate;

        public bool CantSearch
        {
            set => 
                this.label.text = !value ? this.cantCreate.Value : this.cantSearch.Value;
        }
    }
}

