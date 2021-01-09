namespace Tanks.Lobby.ClientControls.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GlassBreaker : MonoBehaviour
    {
        public GameObject[] prefabs;
        private RectTransform buttonTransform;
        [SerializeField]
        private List<RectTransform> glassTransforms = new List<RectTransform>();
        private Dictionary<RectTransform, float> normalizedPositions = new Dictionary<RectTransform, float>();
        private int previousCrack;

        private void AdjustPostion()
        {
            foreach (RectTransform transform in this.glassTransforms)
            {
                transform.anchoredPosition = this.RandomPosition(transform);
            }
        }

        public void BreakGlass()
        {
            this.buttonTransform = base.GetComponent<RectTransform>();
            this.ClearInstances();
            this.previousCrack = -1;
            if (Random.Range(0, 2) == 0)
            {
                this.CreateTopGlassCrack();
            }
            if (Random.Range(0, 2) == 0)
            {
                this.CreateBottomGlassCrack();
            }
        }

        private void ClearInstances()
        {
            foreach (RectTransform transform in this.glassTransforms)
            {
                DestroyImmediate(transform.gameObject);
            }
            this.glassTransforms.Clear();
        }

        private void CreateBottomGlassCrack()
        {
            RectTransform rectTransform = this.CreateGlassCrack();
            rectTransform.localScale = new Vector3(1f, 1f);
            Vector2 vector = new Vector2(0f, 0f);
            rectTransform.anchorMin = vector;
            rectTransform.anchorMax = vector;
            vector = new Vector2();
            rectTransform.pivot = vector;
            rectTransform.anchoredPosition = this.RandomPosition(rectTransform);
        }

        private RectTransform CreateGlassCrack()
        {
            GameObject obj3 = Instantiate<GameObject>(this.RandomCrack());
            RectTransform component = obj3.GetComponent<RectTransform>();
            this.glassTransforms.Add(component);
            this.normalizedPositions.Add(component, Random.Range((float) 0f, (float) 1f));
            obj3.transform.SetParent(base.transform, false);
            return component;
        }

        private void CreateTopGlassCrack()
        {
            RectTransform rectTransform = this.CreateGlassCrack();
            rectTransform.localScale = new Vector3(1f, -1f);
            Vector2 vector = new Vector2(0f, 1f);
            rectTransform.anchorMin = vector;
            rectTransform.anchorMax = vector;
            vector = new Vector2();
            rectTransform.pivot = vector;
            rectTransform.anchoredPosition = this.RandomPosition(rectTransform);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - 5f);
        }

        private void OnRectTransformDimensionsChange()
        {
            this.AdjustPostion();
        }

        private GameObject RandomCrack()
        {
            int previousCrack = this.previousCrack;
            while (previousCrack == this.previousCrack)
            {
                previousCrack = Random.Range(0, this.prefabs.Length);
            }
            this.previousCrack = previousCrack;
            return this.prefabs[previousCrack];
        }

        private Vector2 RandomPosition(RectTransform rectTransform) => 
            new Vector2(this.normalizedPositions[rectTransform] * this.buttonTransform.rect.width, 0f);

        private void Start()
        {
            if (this.glassTransforms.Count == 0)
            {
                this.BreakGlass();
            }
        }
    }
}

