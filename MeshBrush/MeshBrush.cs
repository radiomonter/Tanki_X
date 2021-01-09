namespace MeshBrush
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class MeshBrush : MonoBehaviour
    {
        [HideInInspector]
        public bool isActive = true;
        [HideInInspector]
        public GameObject brush;
        [HideInInspector]
        public Transform holderObj;
        [HideInInspector]
        public Transform brushTransform;
        [HideInInspector]
        public string groupName = "<group name>";
        public GameObject[] setOfMeshesToPaint = new GameObject[1];
        [HideInInspector]
        public List<GameObject> paintBuffer = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> deletionBuffer = new List<GameObject>();
        [HideInInspector]
        public KeyCode paintKey = KeyCode.P;
        [HideInInspector]
        public KeyCode deleteKey = KeyCode.L;
        [HideInInspector]
        public KeyCode combineAreaKey = KeyCode.K;
        [HideInInspector]
        public KeyCode increaseRadiusKey = KeyCode.O;
        [HideInInspector]
        public KeyCode decreaseRadiusKey = KeyCode.I;
        [HideInInspector]
        public float hRadius = 0.3f;
        [HideInInspector]
        public Color hColor = Color.white;
        [HideInInspector]
        public int meshCount = 1;
        [HideInInspector]
        public bool useRandomMeshCount;
        [HideInInspector]
        public int minNrOfMeshes = 1;
        [HideInInspector]
        public int maxNrOfMeshes = 1;
        [HideInInspector]
        public float delay = 0.25f;
        [HideInInspector]
        public float meshOffset;
        [HideInInspector]
        public float slopeInfluence = 100f;
        [HideInInspector]
        public bool activeSlopeFilter;
        [HideInInspector]
        public float maxSlopeFilterAngle = 30f;
        [HideInInspector]
        public bool inverseSlopeFilter;
        [HideInInspector]
        public bool manualRefVecSampling;
        [HideInInspector]
        public bool showRefVecInSceneGUI = true;
        [HideInInspector]
        public Vector3 slopeRefVec = Vector3.up;
        [HideInInspector]
        public Vector3 slopeRefVec_HandleLocation = Vector3.zero;
        [HideInInspector]
        public bool yAxisIsTangent;
        [HideInInspector]
        public bool invertY;
        [HideInInspector]
        public float scattering = 60f;
        [HideInInspector]
        public bool autoStatic = true;
        [HideInInspector]
        public bool uniformScale = true;
        [HideInInspector]
        public bool constUniformScale = true;
        [HideInInspector]
        public bool rWithinRange;
        [HideInInspector]
        public bool b_CustomKeys;
        [HideInInspector]
        public bool b_Slopes = true;
        [HideInInspector]
        public bool b_Randomizers = true;
        [HideInInspector]
        public bool b_AdditiveScale = true;
        [HideInInspector]
        public bool b_opt = true;
        [HideInInspector]
        public float rScaleW;
        [HideInInspector]
        public float rScaleH;
        [HideInInspector]
        public float rScale;
        [HideInInspector]
        public Vector2 rUniformRange = Vector2.zero;
        [HideInInspector]
        public Vector4 rNonUniformRange = Vector4.zero;
        [HideInInspector]
        public float cScale;
        [HideInInspector]
        public Vector3 cScaleXYZ = Vector3.zero;
        [HideInInspector]
        public float rRot;
        [HideInInspector]
        public bool autoSelectOnCombine = true;

        private void OnDestroy()
        {
            if (this.deletionBuffer.Count > 0)
            {
                int num = 0;
                while (true)
                {
                    if (num >= this.deletionBuffer.Count)
                    {
                        this.deletionBuffer.Clear();
                        break;
                    }
                    if (this.deletionBuffer[num])
                    {
                        DestroyImmediate(this.deletionBuffer[num]);
                    }
                    num++;
                }
            }
            if (this.paintBuffer.Count > 0)
            {
                int num2 = 0;
                while (true)
                {
                    if (num2 >= this.paintBuffer.Count)
                    {
                        this.paintBuffer.Clear();
                        break;
                    }
                    if (this.paintBuffer[num2])
                    {
                        DestroyImmediate(this.paintBuffer[num2]);
                    }
                    num2++;
                }
            }
        }

        public void ResetRandomizers()
        {
            this.rScale = 0f;
            this.rScaleW = 0f;
            this.rScaleH = 0f;
            this.rRot = 0f;
            this.rUniformRange = Vector2.zero;
            this.rNonUniformRange = Vector4.zero;
        }

        public void ResetSlopeSettings()
        {
            this.slopeInfluence = 100f;
            this.maxSlopeFilterAngle = 30f;
            this.activeSlopeFilter = false;
            this.inverseSlopeFilter = false;
            this.manualRefVecSampling = false;
            this.showRefVecInSceneGUI = true;
        }
    }
}

