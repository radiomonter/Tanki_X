namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [SerialVersionUID(0x8d2e9c4786d0b31L)]
    public class ScoreTableRowComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private RectTransform indicatorsContainer;
        [SerializeField]
        private Text position;
        [SerializeField]
        private Image background;
        private int positionNumber;
        public Dictionary<ScoreTableRowIndicator, ScoreTableRowIndicator> indicators = new Dictionary<ScoreTableRowIndicator, ScoreTableRowIndicator>();

        public void AddIndicator(ScoreTableRowIndicator indicatorPrefab)
        {
            ScoreTableRowIndicator indicator = Instantiate<ScoreTableRowIndicator>(indicatorPrefab);
            this.indicators.Add(indicatorPrefab, indicator);
            indicator.transform.SetParent(this.indicatorsContainer, false);
            EntityBehaviour component = indicator.GetComponent<EntityBehaviour>();
            if (component != null)
            {
                component.BuildEntity(base.GetComponent<EntityBehaviour>().Entity);
            }
            this.Sort();
        }

        public void AddIndicators(List<ScoreTableRowIndicator> indicatorsList)
        {
            foreach (ScoreTableRowIndicator indicator in indicatorsList)
            {
                ScoreTableRowIndicator indicator2 = Instantiate<ScoreTableRowIndicator>(indicator);
                this.indicators.Add(indicator, indicator2);
                indicator2.transform.SetParent(this.indicatorsContainer, false);
            }
            this.Sort();
        }

        public void HidePosition()
        {
            this.position.gameObject.SetActive(false);
        }

        public void RemoveIndicator(ScoreTableRowIndicator indicatorPrefab)
        {
            Destroy(this.indicators[indicatorPrefab].gameObject);
            this.indicators.Remove(indicatorPrefab);
        }

        public void SetLayoutDirty()
        {
            base.transform.parent.GetComponent<ScoreTableComponent>().SetDirty();
        }

        private void Sort()
        {
            foreach (ScoreTableRowIndicator indicator in this.indicators.Values)
            {
                indicator.transform.SetSiblingIndex(indicator.index);
            }
        }

        public int Position
        {
            get => 
                this.positionNumber;
            set
            {
                this.positionNumber = value;
                if (value == 0)
                {
                    this.position.text = string.Empty;
                    base.transform.SetAsLastSibling();
                }
                else
                {
                    this.position.text = value.ToString();
                    base.transform.SetSiblingIndex(this.positionNumber);
                }
                this.SetLayoutDirty();
            }
        }

        public UnityEngine.Color Color
        {
            get => 
                this.background.color;
            set => 
                this.background.color = value;
        }
    }
}

