namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientBattleSelect.Impl;
    using UnityEngine;

    public class MVPOtherStatComponent : MonoBehaviour
    {
        [SerializeField]
        private MVPStatElementComponent flagsDelivered;
        [SerializeField]
        private MVPStatElementComponent flagsReturned;
        [SerializeField]
        private MVPStatElementComponent damage;
        [SerializeField]
        private MVPStatElementComponent killStreak;
        [SerializeField]
        private MVPStatElementComponent bonuseTaken;
        private UserResult mvp;
        private List<UserResult> allUsers;
        private int showedItems;
        private static int MAX_SHOWED_ITEM = 4;
        [CompilerGenerated]
        private static UserField <>f__am$cache0;
        [CompilerGenerated]
        private static UserField <>f__am$cache1;
        [CompilerGenerated]
        private static UserField <>f__am$cache2;
        [CompilerGenerated]
        private static UserField <>f__am$cache3;
        [CompilerGenerated]
        private static UserField <>f__am$cache4;

        private bool isBest(UserResult mvp, List<UserResult> allUsers, UserField field)
        {
            <isBest>c__AnonStorey0 storey = new <isBest>c__AnonStorey0 {
                field = field
            };
            allUsers.Sort(new Comparison<UserResult>(storey.<>m__0));
            return ((storey.field(allUsers[0]) == storey.field(mvp)) && (storey.field(mvp) > 0));
        }

        public void Set(UserResult mvp, BattleResultForClient battleResults)
        {
            this.mvp = mvp;
            this.allUsers = new List<UserResult>();
            this.allUsers.AddRange(battleResults.DmUsers);
            this.allUsers.AddRange(battleResults.RedUsers);
            this.allUsers.AddRange(battleResults.BlueUsers);
            this.showedItems = 0;
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => x.Flags;
            }
            this.SetStatItem(this.flagsDelivered, mvp, this.allUsers, <>f__am$cache0);
            <>f__am$cache1 ??= x => x.FlagReturns;
            this.SetStatItem(this.flagsReturned, mvp, this.allUsers, <>f__am$cache1);
            <>f__am$cache2 ??= x => x.Damage;
            this.SetStatItem(this.damage, mvp, this.allUsers, <>f__am$cache2);
            <>f__am$cache3 ??= x => x.KillStrike;
            this.SetStatItem(this.killStreak, mvp, this.allUsers, <>f__am$cache3);
            <>f__am$cache4 ??= x => x.BonusesTaken;
            this.SetStatItem(this.bonuseTaken, mvp, this.allUsers, <>f__am$cache4);
        }

        private void SetStatItem(MVPStatElementComponent item, UserResult mvp, List<UserResult> allUsers, UserField field)
        {
            if (this.showedItems >= MAX_SHOWED_ITEM)
            {
                item.Hide();
            }
            else
            {
                item.Count = field(mvp);
                item.SetBest(this.isBest(mvp, allUsers, field));
                if (item.ShowIfCan())
                {
                    this.showedItems++;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <isBest>c__AnonStorey0
        {
            internal MVPOtherStatComponent.UserField field;

            internal int <>m__0(UserResult x, UserResult y) => 
                this.field(y) - this.field(x);
        }

        private delegate int UserField(UserResult user);
    }
}

