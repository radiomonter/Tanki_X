namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using TMPro;
    using UnityEngine;

    public class ExitGameDialogComponent : BehaviourComponent
    {
        public GameObject content;
        public TextMeshProUGUI timer;
        public List<DailyBonusData> dataList;
        public List<long> ReceivedRewards;
        [SerializeField]
        private GameObject ContainerView;
        [SerializeField]
        private GameObject DetailView;
        [SerializeField]
        private GameObject XCryView;
        [SerializeField]
        private GameObject CryView;
        [SerializeField]
        private GameObject EnergyView;
        [SerializeField]
        private GameObject row1;
        [SerializeField]
        private GameObject row2;
        public GameObject[] textNotReady;
        public GameObject textReady;

        public void InstantiateContainerBonus(long marketItem)
        {
            GameObject obj2 = Instantiate<GameObject>(this.ContainerView, this.row1.transform);
            obj2.GetComponent<ContainerBonusView>().UpdateViewByMarketItem(marketItem);
            obj2.GetComponent<Animator>().SetTrigger("show");
        }

        public void InstantiateCryBonus(long amount)
        {
            Instantiate<GameObject>(this.CryView, this.row1.transform).GetComponent<MultipleBonusView>().UpdateView(amount);
        }

        public void InstantiateDetailBonus(long marketItem)
        {
            GameObject obj2 = Instantiate<GameObject>(this.DetailView, this.row1.transform);
            obj2.GetComponent<DetailBonusView>().UpdateViewByMarketItem(marketItem);
            obj2.GetComponent<Animator>().SetTrigger("show");
        }

        public void InstantiateEnergyBonus(long amount)
        {
            Instantiate<GameObject>(this.EnergyView, this.row1.transform).GetComponent<MultipleBonusView>().UpdateView(amount);
        }

        public void InstantiateXCryBonus(long amount)
        {
            Instantiate<GameObject>(this.XCryView, this.row1.transform).GetComponent<MultipleBonusView>().UpdateView(amount);
        }

        private void OnDisable()
        {
            this.row1.transform.DestroyChildren();
            this.row2.transform.DestroyChildren();
        }

        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

