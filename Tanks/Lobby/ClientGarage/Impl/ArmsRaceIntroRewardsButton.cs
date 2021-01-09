namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class ArmsRaceIntroRewardsButton : MonoBehaviour
    {
        public Animator currentScreenAnimator;
        public GameObject rewardsScreen;

        public void ShowRewardsButtonClick()
        {
            this.currentScreenAnimator.SetTrigger("Hide");
            this.rewardsScreen.SetActive(true);
        }
    }
}

