namespace MeshBrush
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class RuntimeAPI : MonoBehaviour
    {
        private Transform thisTransform;
        private GameObject brushObj;
        private Transform brushTransform;
        private RaycastHit hit;
        private GameObject paintedMesh;
        private Transform paintedMeshTransform;
        private GameObject holder;
        private Transform holderTransform;
        private ushort _amount = 1;
        private float _delayBetweenPaintStrokes = 0.15f;
        private float _brushRadius = 1f;
        private float scatteringInsetThreshold;
        private float _slopeInfluence = 100f;
        private float _maxSlopeFilterAngle = 30f;
        private float slopeAngle;
        private float randomWidth;
        private float randomHeight;
        private Vector4 _randomScale = new Vector4(1f, 1f, 1f, 1f);

        private void AddConstantScale(GameObject sMesh)
        {
            Transform transform = sMesh.transform;
            transform.localScale += new Vector3(Mathf.Clamp(this.additiveScale.x, -0.9f, this.additiveScale.x), Mathf.Clamp(this.additiveScale.y, -0.9f, this.additiveScale.y), Mathf.Clamp(this.additiveScale.z, -0.9f, this.additiveScale.z));
        }

        private void ApplyMeshOffset(GameObject oMesh, Vector3 offsetDirection)
        {
            oMesh.transform.Translate((offsetDirection.normalized * this.meshOffset) * 0.01f, Space.World);
        }

        private void ApplyRandomRotation(GameObject rMesh)
        {
            rMesh.transform.Rotate(new Vector3(0f, Random.Range((float) 0f, (float) (3.6f * Mathf.Clamp(this.randomRotation, 0f, 100f))), 0f));
        }

        private void ApplyRandomScale(GameObject sMesh)
        {
            this.randomWidth = Random.Range(this.randomScale.x, this.randomScale.y);
            this.randomHeight = Random.Range(this.randomScale.z, this.randomScale.w);
            sMesh.transform.localScale = new Vector3(this.randomWidth, this.randomHeight, this.randomWidth);
        }

        public void Paint_MultipleMeshes(RaycastHit paintHit)
        {
            this.scatteringInsetThreshold = (this.brushRadius * 0.01f) * this.scattering;
            if (this.brushObj == null)
            {
                this.brushObj = new GameObject("Brush");
                this.brushTransform = this.brushObj.transform;
                this.brushTransform.position = this.thisTransform.position;
                this.brushTransform.parent = paintHit.collider.transform;
            }
            if (!paintHit.collider.transform.Find("Holder"))
            {
                this.holder = new GameObject("Holder");
                this.holderTransform = this.holder.transform;
                this.holderTransform.position = paintHit.collider.transform.position;
                this.holderTransform.rotation = paintHit.collider.transform.rotation;
                this.holderTransform.parent = paintHit.collider.transform;
            }
            for (int i = this.amount; i > 0; i--)
            {
                this.brushTransform.position = paintHit.point + (paintHit.normal * 0.5f);
                this.brushTransform.rotation = Quaternion.LookRotation(paintHit.normal);
                this.brushTransform.up = this.brushTransform.forward;
                Vector2 insideUnitCircle = Random.insideUnitCircle;
                Vector2 vector2 = Random.insideUnitCircle;
                Vector2 vector3 = Random.insideUnitCircle;
                Vector2 vector4 = Random.insideUnitCircle;
                this.brushTransform.Translate(Random.Range((float) (-insideUnitCircle.x * this.scatteringInsetThreshold), (float) (vector2.x * this.scatteringInsetThreshold)), 0f, Random.Range((float) (-vector3.y * this.scatteringInsetThreshold), (float) (vector4.y * this.scatteringInsetThreshold)), Space.Self);
                if (Physics.Raycast(this.brushTransform.position, -paintHit.normal, out this.hit, 2.5f))
                {
                    this.slopeAngle = !this.activeSlopeFilter ? (!this.inverseSlopeFilter ? 0f : 180f) : Vector3.Angle(this.hit.normal, !this.manualRefVecSampling ? Vector3.up : this.sampledSlopeRefVector);
                    if (!this.inverseSlopeFilter ? (this.slopeAngle < this.maxSlopeFilterAngle) : (this.slopeAngle > this.maxSlopeFilterAngle))
                    {
                        this.paintedMesh = Instantiate<GameObject>(this.setOfMeshesToPaint[Random.Range(0, this.setOfMeshesToPaint.Length)], this.hit.point, Quaternion.LookRotation(this.hit.normal));
                        this.paintedMeshTransform = this.paintedMesh.transform;
                        this.paintedMeshTransform.up = !this.yAxisIsTangent ? Vector3.Lerp(Vector3.up, this.paintedMeshTransform.forward, this.slopeInfluence * 0.01f) : Vector3.Lerp(this.paintedMeshTransform.up, this.paintedMeshTransform.forward, this.slopeInfluence * 0.01f);
                        this.paintedMeshTransform.parent = this.holderTransform;
                    }
                    this.ApplyRandomScale(this.paintedMesh);
                    this.ApplyRandomRotation(this.paintedMesh);
                    this.ApplyMeshOffset(this.paintedMesh, this.hit.normal);
                }
            }
        }

        public void Paint_SingleMesh(RaycastHit paintHit)
        {
            if (!paintHit.collider.transform.Find("Holder"))
            {
                this.holder = new GameObject("Holder");
                this.holderTransform = this.holder.transform;
                this.holderTransform.position = paintHit.collider.transform.position;
                this.holderTransform.rotation = paintHit.collider.transform.rotation;
                this.holderTransform.parent = paintHit.collider.transform;
            }
            this.slopeAngle = !this.activeSlopeFilter ? (!this.inverseSlopeFilter ? 0f : 180f) : Vector3.Angle(paintHit.normal, !this.manualRefVecSampling ? Vector3.up : this.sampledSlopeRefVector);
            if (!this.inverseSlopeFilter ? (this.slopeAngle < this.maxSlopeFilterAngle) : (this.slopeAngle > this.maxSlopeFilterAngle))
            {
                this.paintedMesh = Instantiate<GameObject>(this.setOfMeshesToPaint[Random.Range(0, this.setOfMeshesToPaint.Length)], paintHit.point, Quaternion.LookRotation(paintHit.normal));
                this.paintedMeshTransform = this.paintedMesh.transform;
                this.paintedMeshTransform.up = !this.yAxisIsTangent ? Vector3.Lerp(Vector3.up, this.paintedMeshTransform.forward, this.slopeInfluence * 0.01f) : Vector3.Lerp(this.paintedMeshTransform.up, this.paintedMeshTransform.forward, this.slopeInfluence * 0.01f);
                this.paintedMeshTransform.parent = this.holderTransform;
                this.ApplyRandomScale(this.paintedMesh);
                this.ApplyRandomRotation(this.paintedMesh);
                this.ApplyMeshOffset(this.paintedMesh, this.hit.normal);
            }
        }

        private void Start()
        {
            this.thisTransform = base.transform;
            this.scattering = 75f;
        }

        public GameObject[] setOfMeshesToPaint { get; set; }

        public ushort amount
        {
            get => 
                this._amount;
            set => 
                this._amount = (ushort) Mathf.Clamp(value, 1, 100);
        }

        public float delayBetweenPaintStrokes
        {
            get => 
                this._delayBetweenPaintStrokes;
            set => 
                this._delayBetweenPaintStrokes = Mathf.Clamp(value, 0.1f, 1f);
        }

        public float brushRadius
        {
            get => 
                this._brushRadius;
            set
            {
                this._brushRadius = value;
                if (this._brushRadius <= 0.1f)
                {
                    this._brushRadius = 0.1f;
                }
            }
        }

        public float meshOffset { get; set; }

        public float scattering { get; set; }

        public bool yAxisIsTangent { get; set; }

        public float slopeInfluence
        {
            get => 
                this._slopeInfluence;
            set => 
                this._slopeInfluence = Mathf.Clamp(value, 0f, 100f);
        }

        public bool activeSlopeFilter { get; set; }

        public float maxSlopeFilterAngle
        {
            get => 
                this._maxSlopeFilterAngle;
            set => 
                this._maxSlopeFilterAngle = Mathf.Clamp(value, 0f, 180f);
        }

        public bool inverseSlopeFilter { get; set; }

        public bool manualRefVecSampling { get; set; }

        public Vector3 sampledSlopeRefVector { get; set; }

        public float randomRotation { get; set; }

        public Vector4 randomScale
        {
            get => 
                this._randomScale;
            set => 
                this._randomScale = new Vector4(Mathf.Clamp(value.x, 0.01f, value.x), Mathf.Clamp(value.y, 0.01f, value.y), Mathf.Clamp(value.z, 0.01f, value.z), Mathf.Clamp(value.w, 0.01f, value.w));
        }

        public Vector3 additiveScale { get; set; }
    }
}

