namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class UpgradeTeleportButtonComponent : BehaviourComponent
    {
        private void OnEnable()
        {
            base.GetComponent<Button>().interactable = true;
        }
    }
}

