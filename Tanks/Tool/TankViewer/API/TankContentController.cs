namespace Tanks.Tool.TankViewer.API
{
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class TankContentController : MonoBehaviour
    {
        private int weaponIndex;
        private int hullIndex;
        private int coloringIndex;
        public TankContentLibrary tankContentLibrary;
        public TankConstructor tankConstructor;

        public void ChangeVisibleParts()
        {
            GameObject hullInstance = this.tankConstructor.HullInstance;
            GameObject weaponInstance = this.tankConstructor.WeaponInstance;
            if (hullInstance.activeSelf && weaponInstance.activeSelf)
            {
                hullInstance.SetActive(false);
            }
            else if (weaponInstance.activeSelf)
            {
                hullInstance.SetActive(true);
                weaponInstance.SetActive(false);
            }
            else if (hullInstance.activeSelf)
            {
                hullInstance.SetActive(true);
                weaponInstance.SetActive(true);
            }
        }

        public ColoringComponent getCurrentColoring() => 
            this.tankContentLibrary.coloringList[this.coloringIndex];

        private int getNextIndex(int currentIndex, int length) => 
            (currentIndex >= (length - 1)) ? 0 : (currentIndex + 1);

        private int getPrevIndex(int currentIndex, int length) => 
            (currentIndex <= 0) ? (length - 1) : (currentIndex - 1);

        public void Init()
        {
            GameObject hull = this.tankContentLibrary.hullList[this.hullIndex];
            this.tankConstructor.BuildTank(hull, this.tankContentLibrary.weaponList[this.weaponIndex], this.tankContentLibrary.coloringList[this.coloringIndex]);
        }

        public bool IsHullVisible() => 
            this.tankConstructor.HullInstance.activeSelf;

        public bool IsWeaponVisible() => 
            this.tankConstructor.WeaponInstance.activeSelf;

        public void SetNextColoring()
        {
            this.coloringIndex = this.getNextIndex(this.coloringIndex, this.tankContentLibrary.coloringList.Count);
            this.tankConstructor.ChangeColoring(this.tankContentLibrary.coloringList[this.coloringIndex]);
        }

        public void SetNextHull()
        {
            this.hullIndex = this.getNextIndex(this.hullIndex, this.tankContentLibrary.hullList.Count);
            this.tankConstructor.ChangeHull(this.tankContentLibrary.hullList[this.hullIndex]);
        }

        public void SetNextWeapon()
        {
            this.weaponIndex = this.getNextIndex(this.weaponIndex, this.tankContentLibrary.weaponList.Count);
            this.tankConstructor.ChangeWeapon(this.tankContentLibrary.weaponList[this.weaponIndex]);
        }

        public void SetPrevColoring()
        {
            this.coloringIndex = this.getPrevIndex(this.coloringIndex, this.tankContentLibrary.coloringList.Count);
            this.tankConstructor.ChangeColoring(this.tankContentLibrary.coloringList[this.coloringIndex]);
        }

        public void SetPrevHull()
        {
            this.hullIndex = this.getPrevIndex(this.hullIndex, this.tankContentLibrary.hullList.Count);
            this.tankConstructor.ChangeHull(this.tankContentLibrary.hullList[this.hullIndex]);
        }

        public void SetPrevWeapon()
        {
            this.weaponIndex = this.getPrevIndex(this.weaponIndex, this.tankContentLibrary.weaponList.Count);
            this.tankConstructor.ChangeWeapon(this.tankContentLibrary.weaponList[this.weaponIndex]);
        }

        public void SetVisible(bool visible)
        {
            this.tankConstructor.HullInstance.SetActive(visible);
            this.tankConstructor.WeaponInstance.SetActive(visible);
        }

        public string CurrentHullName =>
            this.tankContentLibrary.hullList[this.hullIndex].name;

        public string CurrentWeaponName =>
            this.tankContentLibrary.weaponList[this.weaponIndex].name;
    }
}

