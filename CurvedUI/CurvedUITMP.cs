namespace CurvedUI
{
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class CurvedUITMP : MonoBehaviour
    {
        private CurvedUIVertexEffect crvdVE;
        private TextMeshProUGUI tmp;
        private CurvedUISettings mySettings;
        private Mesh savedMesh;
        private VertexHelper vh;
        private Vector2 savedSize;
        private Vector3 savedUp;
        private Vector3 savedPos;
        private Vector3 savedCanvasSize;
        private List<CurvedUITMPSubmesh> subMeshes = new List<CurvedUITMPSubmesh>();
        [HideInInspector]
        public bool Dirty;
        private bool curvingRequired;
        private bool tesselationRequired;

        private void FindSubmeshes()
        {
            foreach (TMP_SubMeshUI hui in base.GetComponentsInChildren<TMP_SubMeshUI>())
            {
                CurvedUITMPSubmesh item = hui.gameObject.AddComponentIfMissing<CurvedUITMPSubmesh>();
                if (!this.subMeshes.Contains(item))
                {
                    this.subMeshes.Add(item);
                }
            }
        }

        private void FindTMP()
        {
            if (base.GetComponent<TextMeshProUGUI>() != null)
            {
                this.tmp = base.gameObject.GetComponent<TextMeshProUGUI>();
                this.crvdVE = base.gameObject.GetComponent<CurvedUIVertexEffect>();
                this.mySettings = base.GetComponentInParent<CurvedUISettings>();
                base.transform.hasChanged = false;
                this.FindSubmeshes();
            }
        }

        private void LateUpdate()
        {
            if (this.tmp == null)
            {
                this.FindTMP();
            }
            else
            {
                if (this.savedSize != (base.transform as RectTransform).rect.size)
                {
                    this.tesselationRequired = true;
                }
                else if (this.savedCanvasSize != this.mySettings.transform.localScale)
                {
                    this.tesselationRequired = true;
                }
                else if (!this.savedPos.AlmostEqual(this.mySettings.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position), 0.01))
                {
                    this.curvingRequired = true;
                }
                else if (!this.savedUp.AlmostEqual(this.mySettings.transform.worldToLocalMatrix.MultiplyVector(base.transform.up), 0.01))
                {
                    this.curvingRequired = true;
                }
                if (this.Dirty || (this.tesselationRequired || ((this.savedMesh == null) || ((this.vh == null) || (this.curvingRequired && !Application.isPlaying)))))
                {
                    this.tmp.renderMode = TextRenderFlags.Render;
                    this.tmp.ForceMeshUpdate();
                    this.vh = new VertexHelper(this.tmp.mesh);
                    this.crvdVE.TesselationRequired = true;
                    this.crvdVE.ModifyMesh(this.vh);
                    this.savedMesh = new Mesh();
                    this.vh.FillMesh(this.savedMesh);
                    this.tmp.renderMode = TextRenderFlags.DontRender;
                    this.tesselationRequired = false;
                    this.Dirty = false;
                    this.savedSize = (base.transform as RectTransform).rect.size;
                    this.savedUp = this.mySettings.transform.worldToLocalMatrix.MultiplyVector(base.transform.up);
                    this.savedPos = this.mySettings.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
                    this.savedCanvasSize = this.mySettings.transform.localScale;
                    this.FindSubmeshes();
                    foreach (CurvedUITMPSubmesh submesh in this.subMeshes)
                    {
                        submesh.UpdateSubmesh(true, false);
                    }
                }
                if (this.curvingRequired)
                {
                    this.crvdVE.TesselationRequired = false;
                    this.crvdVE.CurvingRequired = true;
                    this.crvdVE.ModifyMesh(this.vh);
                    this.vh.FillMesh(this.savedMesh);
                    this.curvingRequired = false;
                    this.savedSize = (base.transform as RectTransform).rect.size;
                    this.savedUp = this.mySettings.transform.worldToLocalMatrix.MultiplyVector(base.transform.up);
                    this.savedPos = this.mySettings.transform.worldToLocalMatrix.MultiplyPoint3x4(base.transform.position);
                    foreach (CurvedUITMPSubmesh submesh2 in this.subMeshes)
                    {
                        submesh2.UpdateSubmesh(false, true);
                    }
                }
                this.tmp.canvasRenderer.SetMesh(this.savedMesh);
            }
        }

        private void OnDisable()
        {
            if (this.tmp != null)
            {
                this.tmp.UnregisterDirtyMaterialCallback(new UnityAction(this.TesselationRequiredCallback));
                TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<Object>(this.TMPTextChangedCallback));
            }
        }

        private void OnEnable()
        {
            this.FindTMP();
            if (this.tmp != null)
            {
                this.tmp.RegisterDirtyMaterialCallback(new UnityAction(this.TesselationRequiredCallback));
                TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<Object>(this.TMPTextChangedCallback));
            }
        }

        private void TesselationRequiredCallback()
        {
            this.tesselationRequired = true;
            this.curvingRequired = true;
        }

        private void TMPTextChangedCallback(object obj)
        {
            if (obj == this.tmp)
            {
                this.tesselationRequired = true;
            }
        }
    }
}

