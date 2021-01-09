namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ScoreTableHeaderComponent : MonoBehaviour, Component
    {
        public List<ScoreTableRowIndicator> headers = new List<ScoreTableRowIndicator>();
        [SerializeField]
        private RectTransform headerTitle;
        [SerializeField]
        private RectTransform scoreHeaderContainer;

        public void AddHeader(ScoreTableRowIndicator headerPrefab)
        {
            Instantiate<ScoreTableRowIndicator>(headerPrefab).transform.SetParent(this.scoreHeaderContainer, false);
        }

        public void Clear()
        {
            IEnumerator enumerator = this.scoreHeaderContainer.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (current != this.headerTitle)
                    {
                        Destroy(current.gameObject);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public void SetDirty()
        {
            LayoutRebuilder.MarkLayoutForRebuild(this.scoreHeaderContainer);
        }
    }
}

