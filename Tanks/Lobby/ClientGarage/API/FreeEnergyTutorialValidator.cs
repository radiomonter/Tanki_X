namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class FreeEnergyTutorialValidator : MonoBehaviour, ITutorialShowStepValidator
    {
        public bool ShowAllowed(long stepId) => 
            (MainScreenComponent.Instance != null) && (MainScreenComponent.Instance.GetCurrentPanel() != MainScreenComponent.MainScreens.Shop);
    }
}

