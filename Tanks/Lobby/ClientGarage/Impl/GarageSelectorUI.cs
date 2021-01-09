namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    public class GarageSelectorUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject hullButton;
        [SerializeField]
        private GameObject turretButton;
        [SerializeField]
        private GameObject modulesButton;
        [SerializeField]
        private GameObject visualButton;
        [SerializeField]
        private Animator hullAnimator;
        [SerializeField]
        private Animator turretAnimator;
        public Action onTurretSelected;
        public Action onHullSelected;
        private Action onEnable;
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated]
        private static Action <>f__am$cache1;

        private void Awake()
        {
            this.hullButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnHullSelected));
            this.turretButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnSelectTurret));
        }

        private void OnEnable()
        {
            if (this.onEnable != null)
            {
                this.onEnable();
            }
        }

        private void OnHullSelected()
        {
            this.SelectHull();
            this.onHullSelected();
        }

        private void OnSelectTurret()
        {
            this.SelectTurret();
            this.onTurretSelected();
        }

        public void SelectHull()
        {
            this.SetSelectionButton(this.hullButton);
            if (!base.gameObject.activeInHierarchy)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate {
                    };
                }
                this.onEnable = <>f__am$cache0;
            }
        }

        public void SelectModules()
        {
            this.SetSelectionButton(this.modulesButton);
        }

        public void SelectTurret()
        {
            this.SetSelectionButton(this.turretButton);
            if (!base.gameObject.activeInHierarchy)
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate {
                    };
                }
                this.onEnable = <>f__am$cache1;
            }
        }

        public void SelectVisual()
        {
            this.SetSelectionButton(this.visualButton);
        }

        private void SetSelectionButton(GameObject button)
        {
            button.GetComponent<RadioButton>().Activate();
        }
    }
}

