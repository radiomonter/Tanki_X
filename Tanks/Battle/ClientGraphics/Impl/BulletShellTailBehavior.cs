namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class BulletShellTailBehavior : MonoBehaviour
    {
        [SerializeField]
        private int yFrames = 4;
        [SerializeField]
        private int fps = 4;
        [SerializeField]
        private LineRenderer lineRenderer;
        [SerializeField]
        private float zFrom = 0.5f;
        [SerializeField]
        private float zTo = -3f;
        [SerializeField]
        private float zTime = 0.25f;
        private float timer;
        private Vector2 size;
        private int lastIndex;
        private float frameOffset;
        private bool tailGrow;

        private void OnEnable()
        {
            this.timer = 0f;
            this.frameOffset = 1f / ((float) this.yFrames);
            this.lineRenderer.material.SetTextureScale("_MainTex", new Vector2(1f, this.frameOffset));
            this.lastIndex = -1;
            this.tailGrow = false;
        }

        private void Update()
        {
            this.timer += Time.deltaTime;
            int num = Mathf.RoundToInt(this.timer * this.fps) % this.yFrames;
            if (num != this.lastIndex)
            {
                Vector2 vector = new Vector2(0f, this.frameOffset * num);
                this.lineRenderer.material.SetTextureOffset("_MainTex", vector);
                this.lastIndex = num;
            }
            if (this.timer <= this.zTime)
            {
                Vector3[] positions = new Vector3[] { new Vector3(0f, 0f, Mathf.Lerp(this.zFrom, this.zTo, this.timer / this.zTime)), new Vector3(0f, 0f, this.zFrom) };
                this.lineRenderer.SetPositions(positions);
            }
            else if (!this.tailGrow)
            {
                this.tailGrow = true;
                Vector3[] positions = new Vector3[] { new Vector3(0f, 0f, this.zTo), new Vector3(0f, 0f, this.zFrom) };
                this.lineRenderer.SetPositions(positions);
            }
        }
    }
}

