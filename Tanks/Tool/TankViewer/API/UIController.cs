namespace Tanks.Tool.TankViewer.API
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIController : MonoBehaviour
    {
        public TankContentController tankContentController;
        public UIViewController viewController;
        public CameraController cameraController;
        public Dropdown modeDropdown;
        public GameObject coloringViewer;
        public GameObject dron;
        public GameObject spiderMine;
        public GameObject container;
        private bool createColoringState;

        private void Awake()
        {
            this.tankContentController.Init();
            this.viewController.ChangeHullName(this.tankContentController.CurrentHullName);
            this.viewController.ChangeWeaponName(this.tankContentController.CurrentWeaponName);
        }

        public void OnCreateColoringButtonClick()
        {
            this.createColoringState = true;
            this.modeDropdown.enabled = false;
        }

        public void OnCreateColoringFinished()
        {
            this.createColoringState = false;
            this.modeDropdown.enabled = true;
        }

        public void OnModeDropdownChange(Dropdown dropdown)
        {
            if (dropdown.value > 3)
            {
                throw new Exception("Invalid mode dropdown value: " + dropdown.value);
            }
            this.coloringViewer.SetActive(dropdown.value == 0);
            this.tankContentController.SetVisible(dropdown.value == 0);
            this.dron.SetActive(dropdown.value == 1);
            this.spiderMine.SetActive(dropdown.value == 2);
            this.container.SetActive(dropdown.value == 3);
        }

        public void Update()
        {
            if (!this.createColoringState)
            {
                if (this.cameraController != null)
                {
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        this.cameraController.ChangeMode();
                    }
                    if (Input.GetKeyUp(KeyCode.R))
                    {
                        this.cameraController.targetCameraController.SetDefaultTransform();
                    }
                    if (Input.GetKeyUp(KeyCode.F4))
                    {
                        this.cameraController.targetCameraController.AutoRotate = !this.cameraController.targetCameraController.AutoRotate;
                    }
                    if (Input.GetKeyUp(KeyCode.T))
                    {
                        this.viewController.cameraTransform.text = $"pos:{this.cameraController.transform.position}, rot: {this.cameraController.transform.rotation.eulerAngles}";
                    }
                    if (Input.GetKeyUp(KeyCode.G))
                    {
                        this.cameraController.ChangeController();
                    }
                }
                if (Input.GetKeyUp(KeyCode.P))
                {
                    string filePath = $"screen__{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.png";
                    ScreenShotUtil.TakeScreenshotAndOpenIt(Camera.main, filePath, 4);
                }
                if (Input.GetKeyUp(KeyCode.Home))
                {
                    this.tankContentController.SetNextHull();
                    this.viewController.ChangeHullName(this.tankContentController.CurrentHullName);
                }
                if (Input.GetKeyUp(KeyCode.End))
                {
                    this.tankContentController.SetPrevHull();
                    this.viewController.ChangeHullName(this.tankContentController.CurrentHullName);
                }
                if (Input.GetKeyUp(KeyCode.PageUp))
                {
                    this.tankContentController.SetNextWeapon();
                    this.viewController.ChangeWeaponName(this.tankContentController.CurrentWeaponName);
                }
                if (Input.GetKeyUp(KeyCode.PageDown))
                {
                    this.tankContentController.SetPrevWeapon();
                    this.viewController.ChangeWeaponName(this.tankContentController.CurrentWeaponName);
                }
                if (Input.GetKeyUp(KeyCode.Insert))
                {
                    this.tankContentController.SetNextColoring();
                }
                if (Input.GetKeyUp(KeyCode.Delete))
                {
                    this.tankContentController.SetPrevColoring();
                }
                if (Input.GetKeyUp(KeyCode.Q))
                {
                    this.tankContentController.ChangeVisibleParts();
                    this.viewController.hullName.gameObject.SetActive(this.tankContentController.IsHullVisible());
                    this.viewController.weaponName.gameObject.SetActive(this.tankContentController.IsWeaponVisible());
                }
            }
        }
    }
}

