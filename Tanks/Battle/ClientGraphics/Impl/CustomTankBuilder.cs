namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class CustomTankBuilder : BehaviourComponent
    {
        [SerializeField]
        private Transform tankContainer;
        [SerializeField]
        private GameObject tankPrefab;
        [SerializeField]
        private BattleResultTankCameraController camera;
        [SerializeField]
        private Light light;

        private void Awake()
        {
            this.light = base.GetComponentInChildren<Light>(true);
            this.light.transform.localPosition = new Vector3(14.1f, -4.98f, 2.73f);
            this.light.range = 150f;
            this.light.intensity = 2.5f;
            this.light.color = (Color) new Color32(0xf4, 0x9d, 0x1b, 0xff);
            this.light.gameObject.SetActive(false);
        }

        public void BuildTank(string hull, string weapon, string paint, string cover, bool bestPlayerScreen, RenderTexture newRenderTexture)
        {
            this.ClearContainer();
            this.light.gameObject.SetActive(true);
            this.camera.gameObject.SetActive(true);
            BattleResultsTankPositionComponent component = this.tankPrefab.GetComponent<BattleResultsTankPositionComponent>();
            component.hullGuid = hull;
            component.weaponGuid = weapon;
            component.paintGuid = paint;
            component.coverGuid = cover;
            Instantiate<GameObject>(this.tankPrefab, this.tankContainer).SetActive(true);
            if (bestPlayerScreen)
            {
                this.camera.SetupForBestPlayer();
            }
            else
            {
                this.camera.SetupForAwardScren();
            }
            this.camera.SetRenderTexture(newRenderTexture);
        }

        public void ClearContainer()
        {
            this.light.gameObject.SetActive(false);
            this.tankContainer.DestroyChildren();
        }
    }
}

