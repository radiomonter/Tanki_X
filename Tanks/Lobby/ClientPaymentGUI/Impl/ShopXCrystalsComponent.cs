namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ShopXCrystalsComponent : PurchaseItemComponent
    {
        [SerializeField]
        private XCrystalPackage packPrefab;
        [SerializeField]
        private RectTransform packsRoot;
        private Dictionary<long, XCrystalPackage> packs = new Dictionary<long, XCrystalPackage>();
        [CompilerGenerated]
        private static Comparison<XCrystalPackage> <>f__am$cache0;

        public void AddPackage(Entity entity, List<string> images)
        {
            <AddPackage>c__AnonStorey0 storey = new <AddPackage>c__AnonStorey0 {
                entity = entity,
                $this = this
            };
            if (this.packs.ContainsKey(storey.entity.Id))
            {
                Destroy(this.packs[storey.entity.Id].gameObject);
                this.packs.Remove(storey.entity.Id);
            }
            XCrystalPackage package = Instantiate<XCrystalPackage>(this.packPrefab);
            package.transform.SetParent(this.packsRoot, false);
            package.GetComponentInChildren<Button>().onClick.AddListener(new UnityAction(storey.<>m__0));
            this.packs.Add(storey.entity.Id, package);
            package.Init(storey.entity, images);
            package.UpdateData();
        }

        public void Arange()
        {
            List<XCrystalPackage> list = this.packs.Values.ToList<XCrystalPackage>();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = (a, b) => a.Entity.HasComponent<XCrystalsPackComponent>() ? (b.Entity.HasComponent<XCrystalsPackComponent>() ? a.Entity.GetComponent<XCrystalsPackComponent>().Amount.CompareTo(b.Entity.GetComponent<XCrystalsPackComponent>().Amount) : 1) : -1;
            }
            list.Sort(<>f__am$cache0);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].transform.SetSiblingIndex(i);
            }
        }

        private void Awake()
        {
            RectMask2D componentInParent = base.GetComponentInParent<RectMask2D>();
            if (componentInParent != null)
            {
                componentInParent.enabled = false;
            }
        }

        public void Clear()
        {
            foreach (KeyValuePair<long, XCrystalPackage> pair in this.packs)
            {
                Destroy(pair.Value.gameObject);
            }
            this.packs.Clear();
            base.methods.Clear();
        }

        private void OnPackClick(Entity entity)
        {
            base.OnPackClick(entity, true);
        }

        public void UpdatePackage(Entity entity)
        {
            base.shopDialogs.Cancel();
            if (this.packs.ContainsKey(entity.Id))
            {
                this.packs[entity.Id].UpdateData();
            }
        }

        [CompilerGenerated]
        private sealed class <AddPackage>c__AnonStorey0
        {
            internal Entity entity;
            internal ShopXCrystalsComponent $this;

            internal void <>m__0()
            {
                this.$this.OnPackClick(this.entity);
            }
        }
    }
}

