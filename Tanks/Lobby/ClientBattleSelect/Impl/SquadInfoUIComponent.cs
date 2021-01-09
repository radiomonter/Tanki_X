namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;
    using UnityEngine;

    public class SquadInfoUIComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject addButton;
        [SerializeField]
        private GameObject teammate;
        [SerializeField]
        private RectTransform teammatesList;

        public void AddTeammate(long id, string avatarId, int rank)
        {
            foreach (UserLabelComponent component in this.teammatesList.GetComponentsInChildren<UserLabelComponent>(true))
            {
                if (component.UserId == id)
                {
                    return;
                }
            }
            UserLabelBuilder builder = new UserLabelBuilder(id, Instantiate<GameObject>(this.teammate.gameObject), avatarId, false);
            builder.SetLeague(rank);
            GameObject obj2 = builder.Build();
            obj2.transform.SetParent(this.teammatesList, false);
            obj2.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            UserLabelComponent[] componentsInChildren = this.teammatesList.GetComponentsInChildren<UserLabelComponent>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                Destroy(componentsInChildren[i].gameObject);
            }
        }

        public void RemoveTeammate(long id)
        {
            UserLabelComponent[] componentsInChildren = this.teammatesList.GetComponentsInChildren<UserLabelComponent>(true);
            int index = 0;
            while (true)
            {
                if (index < componentsInChildren.Length)
                {
                    UserLabelComponent component = componentsInChildren[index];
                    if (component.UserId != id)
                    {
                        index++;
                        continue;
                    }
                    Destroy(component.gameObject);
                }
                return;
            }
        }

        public void SwitchAddButton(bool value)
        {
            this.addButton.SetActive(value);
        }
    }
}

