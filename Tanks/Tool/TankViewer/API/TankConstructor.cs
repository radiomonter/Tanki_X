namespace Tanks.Tool.TankViewer.API
{
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class TankConstructor : MonoBehaviour
    {
        private GameObject hullInstance;
        private GameObject weaponInstance;
        private ColoringComponent coloring;

        public void BuildTank(GameObject hull, GameObject weapon, ColoringComponent coloring)
        {
            this.CreateHull(hull);
            this.CreateWeapon(weapon);
            this.SetWeaponPosition();
            this.SetColoring(coloring);
        }

        public void ChangeColoring(ColoringComponent coloring)
        {
            this.SetColoring(coloring);
        }

        public void ChangeHull(GameObject hull)
        {
            Destroy(this.hullInstance);
            this.CreateHull(hull);
            this.SetWeaponPosition();
            this.SetColoring(this.coloring);
        }

        public void ChangeWeapon(GameObject weapon)
        {
            Destroy(this.weaponInstance);
            this.CreateWeapon(weapon);
            this.SetWeaponPosition();
            this.SetColoring(this.coloring);
        }

        private void CreateHull(GameObject hull)
        {
            this.hullInstance = Instantiate<GameObject>(hull);
            this.hullInstance.transform.SetParent(base.transform, false);
            this.hullInstance.transform.localPosition = Vector3.zero;
            this.hullInstance.transform.localRotation = Quaternion.identity;
        }

        private void CreateWeapon(GameObject weapon)
        {
            this.weaponInstance = Instantiate<GameObject>(weapon);
            this.weaponInstance.transform.SetParent(base.transform, false);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        private void SetColoring(ColoringComponent coloring)
        {
            this.coloring = coloring;
            TankMaterialsUtil.ApplyColoring(TankBuilderUtil.GetHullRenderer(this.hullInstance), coloring);
            TankMaterialsUtil.ApplyColoring(TankBuilderUtil.GetWeaponRenderer(this.weaponInstance), coloring);
        }

        private void SetWeaponPosition()
        {
            MountPointComponent component = this.hullInstance.GetComponent<MountPointComponent>();
            this.weaponInstance.transform.position = component.MountPoint.position;
        }

        public GameObject HullInstance =>
            this.hullInstance;

        public GameObject WeaponInstance =>
            this.weaponInstance;
    }
}

