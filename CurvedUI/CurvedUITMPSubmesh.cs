namespace CurvedUI
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class CurvedUITMPSubmesh : MonoBehaviour
    {
        private VertexHelper vh;
        private Mesh savedMesh;

        private void ModifyMesh(CurvedUIVertexEffect crvdVE)
        {
            crvdVE.ModifyMesh(this.vh);
        }

        public void UpdateSubmesh(bool tesselate, bool curve)
        {
            TMP_SubMeshUI component = base.gameObject.GetComponent<TMP_SubMeshUI>();
            if (component != null)
            {
                CurvedUIVertexEffect crvdVE = base.gameObject.AddComponentIfMissing<CurvedUIVertexEffect>();
                if (tesselate || ((this.savedMesh == null) || ((this.vh == null) || !Application.isPlaying)))
                {
                    this.vh = new VertexHelper(component.mesh);
                    this.ModifyMesh(crvdVE);
                    this.savedMesh = new Mesh();
                    this.vh.FillMesh(this.savedMesh);
                    crvdVE.TesselationRequired = true;
                }
                else if (curve)
                {
                    this.ModifyMesh(crvdVE);
                    this.vh.FillMesh(this.savedMesh);
                    crvdVE.CurvingRequired = true;
                }
                component.canvasRenderer.SetMesh(this.savedMesh);
            }
        }
    }
}

