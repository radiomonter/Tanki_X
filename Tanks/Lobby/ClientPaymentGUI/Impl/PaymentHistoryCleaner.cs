namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientNavigation.API;

    public class PaymentHistoryCleaner : ScreenHistoryCleaner
    {
        public override void ClearHistory(Stack<ShowScreenData> history)
        {
            foreach (ShowScreenData data in history)
            {
                if (ReferenceEquals(data.ScreenType, typeof(GoodsSelectionScreenComponent)))
                {
                    this.ClearTo(data, history);
                    break;
                }
            }
        }

        private void ClearTo(ShowScreenData entry, Stack<ShowScreenData> history)
        {
            while (true)
            {
                if (history.Count > 0)
                {
                    if (history.Peek() != entry)
                    {
                        history.Pop().FreeContext();
                        continue;
                    }
                    if (base.GetComponent<GoodsSelectionScreenComponent>() != null)
                    {
                        history.Pop().FreeContext();
                    }
                }
                return;
            }
        }
    }
}

