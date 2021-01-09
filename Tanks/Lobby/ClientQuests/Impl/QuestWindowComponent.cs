namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class QuestWindowComponent : UIBehaviour, Component
    {
        [SerializeField]
        private GameObject questPrefab;
        [SerializeField]
        private GameObject questCellPrefab;
        [SerializeField]
        private GameObject questsContainer;
        [SerializeField]
        private GameObject background;
        private List<Animator> animators;
        public Action HideDelegate;

        private void Hide()
        {
            if (this.HideDelegate != null)
            {
                this.HideDelegate();
                this.HideDelegate = null;
            }
            else if (this.ShowOnMainScreen)
            {
                MainScreenComponent.Instance.ClearOnBackOverride();
                this.ShowHiddenScreenParts();
            }
            base.gameObject.SetActive(false);
        }

        public void HideWindow()
        {
            this.Hide();
        }

        private void OnDisable()
        {
            this.ShowHiddenScreenParts();
        }

        public void Show(List<Animator> animators)
        {
            base.gameObject.SetActive(true);
            this.background.SetActive(true);
            if (this.ShowOnMainScreen)
            {
                MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
                this.animators = animators;
                foreach (Animator animator in animators)
                {
                    animator.SetBool("Visible", false);
                }
            }
        }

        private void ShowHiddenScreenParts()
        {
            if (this.animators != null)
            {
                foreach (Animator animator in this.animators)
                {
                    animator.SetBool("Visible", true);
                }
                this.animators = null;
            }
        }

        private void Update()
        {
            if (InputMapping.Cancel && this.ShowOnMainScreen)
            {
                this.Hide();
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public GameObject QuestPrefab =>
            this.questPrefab;

        public GameObject QuestCellPrefab =>
            this.questCellPrefab;

        public GameObject QuestsContainer =>
            this.questsContainer;

        public bool ShowOnMainScreen { get; set; }

        public bool ShowProgress { get; set; }
    }
}

