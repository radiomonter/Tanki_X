namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class SelectCountryDialogComponent : ConfirmDialogComponent
    {
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private SelectCountryItem itemPrefab;
        [SerializeField]
        private TMP_InputField searchInput;
        public KeyValuePair<string, string> country;
        private string filterString = string.Empty;

        private void Clean()
        {
            this.scrollRect.content.DestroyChildren();
        }

        private void CountrySelected(KeyValuePair<string, string> country)
        {
            this.country = country;
        }

        private void CreateItem(KeyValuePair<string, string> pair)
        {
            SelectCountryItem item = Instantiate<SelectCountryItem>(this.itemPrefab);
            item.transform.SetParent(this.scrollRect.content, false);
            item.Init(pair);
            item.gameObject.SetActive(true);
            item.countrySelected += new Tanks.Lobby.ClientGarage.Impl.CountrySelected(this.CountrySelected);
        }

        public void Init(List<KeyValuePair<string, string>> countries)
        {
            this.Clean();
            foreach (KeyValuePair<string, string> pair in countries)
            {
                this.CreateItem(pair);
            }
            this.searchInput.text = string.Empty;
            this.FilterString = string.Empty;
        }

        private void OnDisable()
        {
            this.Clean();
            this.searchInput.onValueChanged.RemoveListener(new UnityAction<string>(this.OnSearchingInputValueChanged));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.searchInput.ActivateInputField();
            this.searchInput.scrollSensitivity = 0f;
            this.searchInput.onValueChanged.AddListener(new UnityAction<string>(this.OnSearchingInputValueChanged));
        }

        private void OnSearchingInputValueChanged(string value)
        {
            this.FilterString = value;
        }

        [DebuggerHidden]
        private IEnumerator ScrollToSelection(Vector3 targetScrollPotision) => 
            new <ScrollToSelection>c__Iterator0 { 
                targetScrollPotision = targetScrollPotision,
                $this = this
            };

        public void Select(string code)
        {
            SelectCountryItem[] componentsInChildren = base.GetComponentsInChildren<SelectCountryItem>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (componentsInChildren[i].CountryCode == code)
                {
                    componentsInChildren[i].GetComponent<Toggle>().isOn = true;
                    VerticalLayoutGroup component = this.scrollRect.content.GetComponent<VerticalLayoutGroup>();
                    LayoutElement element = this.itemPrefab.GetComponent<LayoutElement>();
                    float y = ((i * element.preferredHeight) + (i * component.spacing)) - (this.scrollRect.viewport.rect.height / 2.2f);
                    Vector3 targetScrollPotision = (Vector3) new Vector2(this.scrollRect.content.anchoredPosition.x, y);
                    base.StartCoroutine(this.ScrollToSelection(targetScrollPotision));
                    return;
                }
            }
        }

        public string FilterString
        {
            get => 
                this.filterString;
            set
            {
                this.filterString = value;
                foreach (SelectCountryItem item in this.scrollRect.content.GetComponentsInChildren<SelectCountryItem>(true))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        item.gameObject.SetActive(true);
                    }
                    else
                    {
                        item.gameObject.SetActive(item.CountryName.ToLower().Contains(this.filterString.ToLower()));
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ScrollToSelection>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Vector3 targetScrollPotision;
            internal SelectCountryDialogComponent $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$this.scrollRect.inertia = false;
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        break;

                    case 1:
                        if (this.targetScrollPotision.y > (this.$this.scrollRect.content.rect.height - this.$this.scrollRect.viewport.rect.height))
                        {
                            this.targetScrollPotision.y = this.$this.scrollRect.content.rect.height - this.$this.scrollRect.viewport.rect.height;
                        }
                        this.$this.scrollRect.content.anchoredPosition = this.targetScrollPotision;
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        break;

                    case 2:
                        this.$this.scrollRect.inertia = true;
                        this.$PC = -1;
                        goto TR_0000;

                    default:
                        goto TR_0000;
                }
                return true;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

