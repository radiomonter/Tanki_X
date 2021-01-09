namespace Tanks.Lobby.ClientBattleSelect.API
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;

    public class UsersListDataProvider : DefaultListDataProvider
    {
        private int maxCount;

        public override void AddItem(object data)
        {
            int firstNullIndex = this.GetFirstNullIndex();
            if (firstNullIndex == -1)
            {
                base.dataStorage.Add(data);
            }
            else
            {
                base.dataStorage[firstNullIndex] = data;
            }
            base.SendChanged();
        }

        private int GetFirstNullIndex()
        {
            int num = 0;
            for (int i = 0; i < base.dataStorage.Count; i++)
            {
                if (base.dataStorage[i] == null)
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        public TeamListUserData GetUserDataByUid(string uid)
        {
            TeamListUserData data2;
            using (List<object>.Enumerator enumerator = base.dataStorage.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        TeamListUserData data = current as TeamListUserData;
                        if ((data == null) || (data.userUid != uid))
                        {
                            continue;
                        }
                        data2 = data;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return data2;
        }

        public int GetUsersCount()
        {
            int num = 0;
            foreach (object obj2 in base.dataStorage)
            {
                if (obj2 != null)
                {
                    num++;
                }
            }
            return num;
        }

        public void InitEmptyList(int maxCount)
        {
            this.maxCount = maxCount;
            for (int i = 0; i < maxCount; i++)
            {
                base.dataStorage.Add(null);
            }
            base.SendChanged();
        }

        protected override void OnDisable()
        {
        }

        public override void RemoveItem(object data)
        {
            base.dataStorage.Remove(data);
            base.dataStorage.Add(null);
            base.SendChanged();
        }

        public virtual void UpdateItem(object oldData, object newData)
        {
            int index = base.dataStorage.IndexOf(oldData);
            if (index != -1)
            {
                base.dataStorage[index] = newData;
                base.SendChanged();
            }
        }
    }
}

