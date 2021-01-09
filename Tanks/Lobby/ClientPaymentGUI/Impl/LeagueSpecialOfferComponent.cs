namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientBattleSelect.Impl;
    using Tanks.Lobby.ClientControls.API;

    public class LeagueSpecialOfferComponent : ItemContainerComponent
    {
        public LocalizedField worthItText;

        public void ShowOfferItems(List<SpecialOfferItem> items, int worthIt)
        {
            base.InstantiateItems(items);
            string text = string.Format(this.worthItText.Value, worthIt);
            base.GetComponent<SpecialOfferContent>().SetSaleText(text);
        }
    }
}

