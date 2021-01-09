namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class BrokenEffectComponent : BehaviourComponent
    {
        [SerializeField]
        private GameObject brokenEffect;
        [SerializeField]
        private string trackObjectNamePrefix = "track";
        [SerializeField]
        private string trackMaterialNamePrefix = "Track";
        private Rigidbody rigidbody;
        private Rigidbody parentRigidbody;
        public GameObject effectInstance;
        private Rigidbody[] partRigidbodies;
        private Renderer[] renderers;
        private List<Material> materials;
        private string[] materialNames;
        private float lastAlpha = -1f;
        private bool[] rendererIsTrack;
        private List<Vector3> partPositions;
        private List<Quaternion> partRotations;
        private bool inited;
        private float effectStartTime;
        private float effectLifeTime;
        private float fadeTime = 2f;
        public float partDetachProbability = 0.7f;
        public float LifeTime = 6f;
        private Dictionary<string, Material> nameToMaterial;

        private void Awake()
        {
            base.enabled = false;
        }

        private void CacheMaterials(Material[] materials)
        {
            if (this.nameToMaterial.Count <= 0)
            {
                foreach (Material material in materials)
                {
                    string key = material.name.Replace("(Instance)", string.Empty).Replace(" ", string.Empty);
                    if (!this.nameToMaterial.ContainsKey(key))
                    {
                        this.nameToMaterial.Add(key, material);
                    }
                }
                this.materialNames = new string[this.renderers.Length];
                for (int i = 0; i < this.materialNames.Length; i++)
                {
                    Material sharedMaterial = this.renderers[i].sharedMaterial;
                    this.materialNames[i] = sharedMaterial.name.Replace("(Instance)", string.Empty).Replace(" ", string.Empty);
                }
            }
        }

        private void Disable()
        {
            if (this.effectInstance)
            {
                this.effectInstance.SetActive(false);
            }
            base.enabled = false;
        }

        private void Enable()
        {
            if (this.effectInstance)
            {
                this.effectInstance.gameObject.SetActive(true);
            }
            base.enabled = true;
        }

        private bool FadeAlpha()
        {
            float clampedAlpha = 1f - Mathf.Clamp01((Time.timeSinceLevelLoad - ((this.effectStartTime + this.effectLifeTime) - this.fadeTime)) / this.fadeTime);
            if (clampedAlpha != this.lastAlpha)
            {
                this.lastAlpha = clampedAlpha;
                foreach (Material material in this.materials)
                {
                    TankMaterialsUtil.SetAlpha(material, clampedAlpha);
                }
            }
            return (Time.timeSinceLevelLoad < (this.effectStartTime + this.effectLifeTime));
        }

        public void Init()
        {
            this.rigidbody = base.GetComponent<Rigidbody>();
            this.effectInstance = Instantiate<GameObject>(this.brokenEffect);
            PhysicsUtil.SetGameObjectLayer(this.effectInstance, Layers.MINOR_VISUAL);
            this.partRigidbodies = this.effectInstance.GetComponentsInChildren<Rigidbody>();
            this.renderers = this.effectInstance.GetComponentsInChildren<Renderer>();
            this.nameToMaterial = new Dictionary<string, Material>();
            this.SaveTransforms();
            this.Disable();
            this.inited = true;
        }

        private void OnDestroy()
        {
            if (this.effectInstance)
            {
                Destroy(this.effectInstance);
            }
        }

        private void RecoverTransforms()
        {
            for (int i = 0; i < this.partRigidbodies.Length; i++)
            {
                Rigidbody rigidbody = this.partRigidbodies[i];
                rigidbody.transform.localPosition = this.partPositions[i];
                rigidbody.transform.localRotation = this.partRotations[i];
            }
        }

        private void SaveTransforms()
        {
            this.partPositions = new List<Vector3>(this.partRigidbodies.Length);
            this.partRotations = new List<Quaternion>(this.partRigidbodies.Length);
            foreach (Rigidbody rigidbody in this.partRigidbodies)
            {
                this.partPositions.Add(rigidbody.transform.localPosition);
                this.partRotations.Add(rigidbody.transform.localRotation);
            }
        }

        private void SetVelocityFromParent(float maxDepenetrationVelocity)
        {
            foreach (Rigidbody rigidbody in this.partRigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.velocity = this.parentRigidbody.velocity;
                rigidbody.angularVelocity = this.parentRigidbody.angularVelocity;
                rigidbody.gameObject.SetActive(true);
                rigidbody.maxDepenetrationVelocity = (Random.value >= this.partDetachProbability) ? 1f : maxDepenetrationVelocity;
            }
        }

        public void StartEffect(GameObject root, Rigidbody parentRigidbody, Renderer parentRenderer, Shader overloadShader, float maxDepenetrationVelocity)
        {
            if (!this.inited || !this.effectInstance)
            {
                this.Init();
            }
            this.parentRigidbody = parentRigidbody;
            this.effectInstance.transform.SetParent(root.transform);
            this.effectInstance.transform.position = this.rigidbody.transform.position;
            this.effectInstance.transform.rotation = this.rigidbody.transform.rotation;
            this.UpdateMaterialsFromParentMaterials(parentRenderer, overloadShader);
            this.RecoverTransforms();
            this.SetVelocityFromParent(maxDepenetrationVelocity);
            this.effectStartTime = Time.timeSinceLevelLoad;
            this.effectLifeTime = this.LifeTime;
            this.Enable();
        }

        public void Update()
        {
            if (!this.FadeAlpha())
            {
                this.Disable();
            }
        }

        private void UpdateMaterialsFromParentMaterials(Renderer parentRenderer, Shader overloadShader)
        {
            this.materials = new List<Material>();
            Material[] sharedMaterials = parentRenderer.sharedMaterials;
            this.CacheMaterials(sharedMaterials);
            for (int i = 0; i < this.renderers.Length; i++)
            {
                Renderer renderer = this.renderers[i];
                Material source = sharedMaterials[0];
                if (this.nameToMaterial.ContainsKey(this.materialNames[i]))
                {
                    source = this.nameToMaterial[this.materialNames[i]];
                }
                Material item = new Material(source);
                if (overloadShader)
                {
                    item.shader = overloadShader;
                }
                renderer.material = item;
                if (!this.materials.Contains(item))
                {
                    this.materials.Add(item);
                }
            }
        }
    }
}

