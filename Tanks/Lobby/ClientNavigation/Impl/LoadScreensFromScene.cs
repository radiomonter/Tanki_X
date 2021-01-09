namespace Tanks.Lobby.ClientNavigation.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LoadScreensFromScene : MonoBehaviour
    {
        public string sceneName = string.Empty;
        public string pathToScreens = string.Empty;
        private bool loaded;

        [DebuggerHidden]
        private IEnumerator MoveScreensUnderSelf() => 
            new <MoveScreensUnderSelf>c__Iterator0 { $this = this };

        private void OnEnable()
        {
            if (!this.loaded)
            {
                this.loaded = true;
                SceneManager.LoadScene(this.sceneName, LoadSceneMode.Additive);
                base.StartCoroutine(this.MoveScreensUnderSelf());
            }
        }

        [CompilerGenerated]
        private sealed class <MoveScreensUnderSelf>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal GameObject <root>__0;
            internal Transform <screen>__0;
            internal LoadScreensFromScene $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;
            private <MoveScreensUnderSelf>c__AnonStorey1 $locvar0;

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
                        this.$locvar0 = new <MoveScreensUnderSelf>c__AnonStorey1();
                        this.$locvar0.<>f__ref$0 = this;
                        this.$current = new WaitForSeconds(0f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        string pathToScreens;
                        if (!this.$this.pathToScreens.Contains("/"))
                        {
                            pathToScreens = this.$this.pathToScreens;
                        }
                        else
                        {
                            char[] separator = new char[] { '/' };
                            pathToScreens = this.$this.pathToScreens.Split(separator)[0];
                        }
                        this.$locvar0.firstPathPart = pathToScreens;
                        this.<root>__0 = SceneManager.GetSceneByName(this.$this.sceneName).GetRootGameObjects().FirstOrDefault<GameObject>(new Func<GameObject, bool>(this.$locvar0.<>m__0));
                        this.<screen>__0 = null;
                        if (this.<root>__0 != null)
                        {
                            string name = !this.$this.pathToScreens.Contains("/") ? null : this.$this.pathToScreens.Substring(this.$this.pathToScreens.IndexOf('/') + 1);
                            this.<screen>__0 = (name == null) ? this.<root>__0.transform : this.<root>__0.transform.Find(name);
                        }
                        if (this.<screen>__0 == null)
                        {
                            Debug.LogWarning("LoadScreensFromScene can't find screen at " + this.$this.pathToScreens);
                        }
                        else
                        {
                            this.<screen>__0.SetParent(this.$this.transform, false);
                            SceneManager.UnloadSceneAsync(this.$this.sceneName);
                            this.$PC = -1;
                        }
                        break;

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

            private sealed class <MoveScreensUnderSelf>c__AnonStorey1
            {
                internal string firstPathPart;
                internal LoadScreensFromScene.<MoveScreensUnderSelf>c__Iterator0 <>f__ref$0;

                internal bool <>m__0(GameObject o) => 
                    o.name.Equals(this.firstPathPart);
            }
        }
    }
}

