namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using UnityEngine;

    public class InBattleQuestsContainerGUIComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject questPrefab;
        [SerializeField]
        private GameObject questsContainer;

        public void CreateQuest()
        {
            this.CreateQuestItem();
        }

        public GameObject CreateQuestItem()
        {
            GameObject obj2 = Instantiate<GameObject>(this.questPrefab);
            obj2.transform.SetParent(this.questsContainer.transform, false);
            base.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
            return obj2;
        }

        public void DeleteAllQuests()
        {
            IEnumerator enumerator = this.questsContainer.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    Destroy(current.gameObject);
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

        public void RemoveQuest()
        {
            Destroy(this.questsContainer.transform.GetChild(this.questsContainer.transform.childCount - 1).gameObject);
        }
    }
}

