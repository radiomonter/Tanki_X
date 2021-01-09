namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;

    public class MVPModulesInfoComponent : MonoBehaviour
    {
        [SerializeField]
        private MVPModuleContainer moduleContainerPrefab;
        [SerializeField]
        private float moduleAnimationDelay = 0.2f;
        private float moduleSize = 160f;
        private bool animated;

        private void addModule(ModuleInfo m)
        {
            if (GarageItemsRegistry.GetItem<ModuleItem>(m.ModuleId).IsMutable())
            {
                Instantiate<MVPModuleContainer>(this.moduleContainerPrefab, base.transform).SetupModuleCard(m, this.moduleSize);
            }
        }

        [ContextMenu("Animate cards")]
        public void AnimateCards()
        {
            base.StartCoroutine(this.AnimateCards(base.GetComponentsInChildren<MVPModuleContainer>()));
        }

        [DebuggerHidden]
        private IEnumerator AnimateCards(MVPModuleContainer[] cards) => 
            new <AnimateCards>c__Iterator0 { 
                cards = cards,
                $this = this
            };

        private List<ModuleInfo> GetModulesByBehaviourType(List<ModuleInfo> modules, ModuleBehaviourType type)
        {
            <GetModulesByBehaviourType>c__AnonStorey1 storey = new <GetModulesByBehaviourType>c__AnonStorey1 {
                type = type,
                res = new List<ModuleInfo>()
            };
            modules.ForEach(new Action<ModuleInfo>(storey.<>m__0));
            storey.res.Sort(new ModuleComparer());
            return storey.res;
        }

        public void Set(List<ModuleInfo> modules)
        {
            this.animated = false;
            for (int i = 0; i < base.transform.childCount; i++)
            {
                Destroy(base.transform.GetChild(i).gameObject);
            }
            List<ModuleInfo> modulesByBehaviourType = this.GetModulesByBehaviourType(modules, ModuleBehaviourType.PASSIVE);
            this.GetModulesByBehaviourType(modules, ModuleBehaviourType.ACTIVE).ForEach(m => this.addModule(m));
            modulesByBehaviourType.ForEach(m => this.addModule(m));
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        [CompilerGenerated]
        private sealed class <AnimateCards>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal MVPModuleContainer[] cards;
            internal MVPModuleContainer[] $locvar0;
            internal int $locvar1;
            internal MVPModuleContainer <module>__1;
            internal MVPModulesInfoComponent $this;
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
                        this.$locvar0 = this.cards;
                        this.$locvar1 = 0;
                        break;

                    case 1:
                        this.$locvar1++;
                        break;

                    case 2:
                        this.$PC = -1;
                        goto TR_0000;

                    default:
                        goto TR_0000;
                }
                if (this.$locvar1 < this.$locvar0.Length)
                {
                    this.<module>__1 = this.$locvar0[this.$locvar1];
                    if (this.<module>__1 != null)
                    {
                        this.<module>__1.GetComponent<Animator>().SetBool("active", true);
                        this.$current = new WaitForSeconds(this.$this.moduleAnimationDelay);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                    }
                    else
                    {
                        goto TR_0000;
                    }
                    goto TR_0001;
                }
                else
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto TR_0001;
                }
            TR_0000:
                return false;
            TR_0001:
                return true;
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

        [CompilerGenerated]
        private sealed class <GetModulesByBehaviourType>c__AnonStorey1
        {
            internal ModuleBehaviourType type;
            internal List<ModuleInfo> res;

            internal void <>m__0(ModuleInfo m)
            {
                if (MVPModulesInfoComponent.GarageItemsRegistry.GetItem<ModuleItem>(m.ModuleId).ModuleBehaviourType == this.type)
                {
                    this.res.Add(m);
                }
            }
        }

        private class ModuleComparer : IComparer<ModuleInfo>
        {
            public int Compare(ModuleInfo x, ModuleInfo y)
            {
                ModuleItem item = MVPModulesInfoComponent.GarageItemsRegistry.GetItem<ModuleItem>(x.ModuleId);
                ModuleItem item2 = MVPModulesInfoComponent.GarageItemsRegistry.GetItem<ModuleItem>(y.ModuleId);
                return ((item.TankPartModuleType != item2.TankPartModuleType) ? ((item.TankPartModuleType != TankPartModuleType.WEAPON) ? 1 : -1) : 0);
            }
        }
    }
}

