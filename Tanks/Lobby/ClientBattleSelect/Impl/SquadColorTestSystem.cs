namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class SquadColorTestSystem : ECSSystem
    {
        [DebuggerHidden]
        private IEnumerator AddGruops(GameObject go, long squadGroupKey, TeamColor teamColor) => 
            new <AddGruops>c__Iterator0 { 
                go = go,
                squadGroupKey = squadGroupKey,
                teamColor = teamColor
            };

        private void CreateUserCell(long squadGroupKey, int x, int y, TeamColor teamColor)
        {
            GameObject obj2 = GameObject.Find("MainScreen");
            GameObject go = new GameObject("User with squad group " + squadGroupKey);
            go.SetActive(false);
            go.transform.SetParent(obj2.transform, false);
            GameObject obj4 = new GameObject("Color image");
            obj4.transform.SetParent(go.transform, false);
            go.AddComponent<UserSquadColorComponent>().Image = obj4.AddComponent<Image>();
            RectTransform component = obj4.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(20f, 20f);
            component.anchoredPosition = new Vector2(x * 30f, y * 30f);
            go.AddComponent<EntityBehaviour>().handleAutomaticaly = true;
            go.SetActive(true);
            go.GetComponent<MonoBehaviour>().StartCoroutine(this.AddGruops(go, squadGroupKey, teamColor));
        }

        [OnEventFire]
        public void InitTest(NodeAddedEvent e, SelfSquadUser user)
        {
            TeamColorComponent component = new TeamColorComponent {
                TeamColor = TeamColor.BLUE
            };
            user.Entity.AddComponent(component);
            int x = -6;
            this.CreateUserCell(user.squadGroup.Key, x, -2, TeamColor.BLUE);
            this.CreateUserCell(user.squadGroup.Key, x, -3, TeamColor.BLUE);
            for (int i = 0; i < 10; i++)
            {
                TeamColor teamColor = (i >= 4) ? TeamColor.RED : TeamColor.BLUE;
                long squadGroupKey = (long) Random.Range((float) 0f, (float) 1.251235E+10f);
                x = i - 5;
                this.CreateUserCell(squadGroupKey, x, -2, teamColor);
                this.CreateUserCell(squadGroupKey, x, -3, teamColor);
            }
        }

        [CompilerGenerated]
        private sealed class <AddGruops>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal GameObject go;
            internal Entity <entity>__0;
            internal long squadGroupKey;
            internal TeamColor teamColor;
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
                        this.$current = new WaitForSeconds(1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                    {
                        this.<entity>__0 = this.go.GetComponent<EntityBehaviour>().Entity;
                        this.<entity>__0.AddComponent(new UserGroupComponent(this.<entity>__0.Id));
                        this.<entity>__0.AddComponent(new SquadGroupComponent(this.squadGroupKey));
                        TeamColorComponent component = new TeamColorComponent {
                            TeamColor = this.teamColor
                        };
                        this.<entity>__0.AddComponent(component);
                        this.$PC = -1;
                        break;
                    }
                    default:
                        break;
                }
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

        public class SelfSquadUser : Node
        {
            public UserGroupComponent userGroup;
            public SquadGroupComponent squadGroup;
            public SelfUserComponent selfUser;
        }
    }
}

