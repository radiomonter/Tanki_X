namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class GoldBoxesLearnMoreSystem : ECSSystem
    {
        [OnEventFire]
        public void ShowInfoDialog(ButtonClickEvent e, SingleNode<GoldBoxesLearnMoreButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs60)
        {
            dialogs60.component.Get<GoldBoxesLearnMoreComponent>().Show(null);
        }
    }
}

