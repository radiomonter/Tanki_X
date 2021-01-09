namespace Tanks.Tool.TankViewer.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIViewController : MonoBehaviour
    {
        public Text hullName;
        public Text weaponName;
        public Text cameraTransform;

        public void ChangeHullName(string currentHullName)
        {
            this.hullName.text = currentHullName;
        }

        public void ChangeWeaponName(string currentWeaponName)
        {
            this.weaponName.text = currentWeaponName;
        }
    }
}

