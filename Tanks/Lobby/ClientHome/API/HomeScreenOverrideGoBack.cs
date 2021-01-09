namespace Tanks.Lobby.ClientHome.API
{
    using Tanks.Lobby.ClientNavigation.API;

    public class HomeScreenOverrideGoBack : OverrideGoBack
    {
        private ShowScreenData data = new ShowScreenData(typeof(HomeScreenComponent), AnimationDirection.DOWN);

        public override ShowScreenData ScreenData =>
            this.data;
    }
}

