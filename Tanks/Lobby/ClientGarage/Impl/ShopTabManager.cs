namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class ShopTabManager : TabManager
    {
        private static ShopTabManager _instance;
        private static int _shopTabIndex;

        private void Awake()
        {
            _instance = this;
        }

        protected override void OnEnable()
        {
            this.Show(_shopTabIndex);
        }

        public override void Show(int newIndex)
        {
            LogScreen shopBlueprints;
            _shopTabIndex = newIndex;
            base.Show(newIndex);
            switch (newIndex)
            {
                case 1:
                    shopBlueprints = LogScreen.ShopBlueprints;
                    break;

                case 2:
                    shopBlueprints = LogScreen.ShopContainers;
                    break;

                case 3:
                    shopBlueprints = LogScreen.ShopXCry;
                    break;

                case 4:
                    shopBlueprints = LogScreen.ShopCry;
                    break;

                case 5:
                    shopBlueprints = LogScreen.ShopPrem;
                    break;

                case 6:
                    shopBlueprints = LogScreen.GoldBoxes;
                    break;

                default:
                    shopBlueprints = LogScreen.ShopDeals;
                    break;
            }
            MainScreenComponent.Instance.SendShowScreenStat(shopBlueprints);
        }

        public static ShopTabManager Instance =>
            _instance;

        public static int shopTabIndex
        {
            get => 
                shopTabIndex;
            set
            {
                if (_instance == null)
                {
                    _shopTabIndex = value;
                }
                else
                {
                    _instance.Show(value);
                }
            }
        }
    }
}

