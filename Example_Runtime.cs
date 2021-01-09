using MeshBrush;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Example_Runtime : MonoBehaviour
{
    private RuntimeAPI mb;
    private Ray paintRay;
    private RaycastHit hit;
    public GameObject[] exampleCubes = new GameObject[2];

    [DebuggerHidden]
    private IEnumerator PaintExampleCubes() => 
        new <PaintExampleCubes>c__Iterator0 { $this = this };

    private void Start()
    {
        base.StartCoroutine(this.PaintExampleCubes());
        if (!base.GetComponent<RuntimeAPI>())
        {
            base.gameObject.AddComponent<RuntimeAPI>();
        }
        this.mb = base.GetComponent<RuntimeAPI>();
        for (int i = 0; i < this.exampleCubes.Length; i++)
        {
            if (this.exampleCubes[i] == null)
            {
                Debug.LogError("One or more GameObjects in the set of meshes to paint are unassigned.");
            }
        }
        this.mb.brushRadius = 10f;
        this.mb.amount = 7;
        this.mb.delayBetweenPaintStrokes = 0.2f;
        this.mb.randomScale = new Vector4(0.4f, 1.4f, 0.5f, 1.5f);
        this.mb.randomRotation = 100f;
        this.mb.meshOffset = 1.5f;
        this.mb.scattering = 75f;
    }

    [CompilerGenerated]
    private sealed class <PaintExampleCubes>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Example_Runtime $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 2:
                    if (!Input.GetKey(KeyCode.P))
                    {
                        break;
                    }
                    this.$this.mb.setOfMeshesToPaint = this.$this.exampleCubes;
                    this.$this.paintRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(this.$this.paintRay, out this.$this.hit))
                    {
                        this.$this.mb.Paint_MultipleMeshes(this.$this.hit);
                    }
                    this.$current = new WaitForSeconds(this.$this.mb.delayBetweenPaintStrokes);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto TR_0001;

                case 1:
                    break;

                default:
                    return false;
            }
            this.$current = null;
            if (!this.$disposing)
            {
                this.$PC = 2;
            }
        TR_0001:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

