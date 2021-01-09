namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UsersEnergyCellsListUIComponent : BehaviourComponent
    {
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private UserEnergyCellUIComponent cell;
        private List<UserEnergyCellUIComponent> cells = new List<UserEnergyCellUIComponent>();

        public UserEnergyCellUIComponent AddUserCell()
        {
            UserEnergyCellUIComponent item = Instantiate<UserEnergyCellUIComponent>(this.cell);
            item.transform.SetParent(this.content, false);
            item.gameObject.SetActive(true);
            this.cells.Add(item);
            this.UpdateCells();
            return item;
        }

        private void OnDisable()
        {
            this.content.DestroyChildren();
            this.cells.Clear();
        }

        public void RemoveUserCell(UserEnergyCellUIComponent user)
        {
            if (this.cells.Contains(user))
            {
                this.cells.Remove(user);
            }
            Destroy(user.gameObject);
            this.UpdateCells();
        }

        private void UpdateCells()
        {
            for (int i = 0; i < this.cells.Count; i++)
            {
                this.cells[i].CellIsFirst = i == 0;
            }
        }
    }
}

