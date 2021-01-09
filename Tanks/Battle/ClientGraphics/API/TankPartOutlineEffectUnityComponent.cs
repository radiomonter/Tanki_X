namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TankPartOutlineEffectUnityComponent : BehaviourComponent
    {
        private const string ALPHA_NAME = "_Alpha";
        [SerializeField]
        private GameObject outlineEffectGameObject;
        private MeshRenderer outlineMeshRenderer;
        private Material materialForTankPart;
        private int alphaPropertyId;

        private void Awake()
        {
            Mesh mesh = this.outlineEffectGameObject.GetComponent<MeshFilter>().mesh;
            mesh.bounds = new Bounds(mesh.bounds.center, mesh.bounds.size * 1000f);
            base.enabled = false;
            this.outlineEffectGameObject.SetActive(false);
        }

        public Material InitTankPartForOutlineEffect(Material materialForTankPart = null)
        {
            this.outlineMeshRenderer = this.outlineEffectGameObject.GetComponent<MeshRenderer>();
            this.outlineMeshRenderer.enabled = false;
            int length = this.outlineMeshRenderer.materials.Length;
            Material[] materialArray = new Material[length];
            this.alphaPropertyId = Shader.PropertyToID("_Alpha");
            materialForTankPart = (materialForTankPart != null) ? materialForTankPart : Instantiate<Material>(this.outlineMeshRenderer.materials[0]);
            for (int i = 0; i < length; i++)
            {
                materialArray[i] = materialForTankPart;
            }
            this.outlineMeshRenderer.materials = materialArray;
            this.materialForTankPart = materialForTankPart;
            materialForTankPart.SetFloat(this.alphaPropertyId, 1f);
            base.enabled = true;
            return materialForTankPart;
        }

        public void SwitchOutlineRenderer(bool enableRenderer)
        {
            this.outlineMeshRenderer.enabled = enableRenderer;
        }

        private void Update()
        {
            if (TankOutlineMapEffectComponent.IS_OUTLINE_EFFECT_RUNNING)
            {
                if (!this.outlineEffectGameObject.activeSelf)
                {
                    this.outlineEffectGameObject.SetActive(true);
                }
            }
            else if (this.outlineEffectGameObject.activeSelf)
            {
                this.outlineEffectGameObject.SetActive(false);
            }
        }

        public void UpdateTankPartOutlineEffectTransparency(float alpha)
        {
            this.materialForTankPart.SetFloat(this.alphaPropertyId, alpha);
        }

        public GameObject OutlineEffectGameObject
        {
            get => 
                this.outlineEffectGameObject;
            set => 
                this.outlineEffectGameObject = value;
        }

        public Material MaterialForTankPart =>
            this.materialForTankPart;
    }
}

