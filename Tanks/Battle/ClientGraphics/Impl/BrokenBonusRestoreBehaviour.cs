namespace Tanks.Battle.ClientGraphics.Impl
{
    using LeopotamGroup.Collections;
    using System;
    using System.Collections;
    using UnityEngine;

    public class BrokenBonusRestoreBehaviour : MonoBehaviour
    {
        private readonly FastList<Vector3> _positions = new FastList<Vector3>();
        private readonly FastList<Quaternion> _rotations = new FastList<Quaternion>();

        private void Awake()
        {
            IEnumerator enumerator = base.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    this._positions.Add(current.localPosition);
                    this._rotations.Add(current.localRotation);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void OnDisable()
        {
            int num = 0;
            IEnumerator enumerator = base.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    current.localPosition = this._positions[num];
                    current.localRotation = this._rotations[num];
                    num++;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}

