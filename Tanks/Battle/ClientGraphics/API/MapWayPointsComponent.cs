namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MapWayPointsComponent : MonoBehaviour, Component
    {
        public static int PATH_REMEMBER_LENGTH = 3;
        [SerializeField]
        private Vector3[] wayPoints;
        [SerializeField, HideInInspector]
        private int[] bestWays;
        private Dictionary<long, int> hash2index = new Dictionary<long, int>();
        private static float CELL_SIZE = 1f;
        private static int WORLD_CELLS_SIZE = 0x3e8;

        public void Create(GameObject waypointsRoot)
        {
            int childCount = waypointsRoot.transform.childCount;
            this.wayPoints = new Vector3[childCount];
            this.bestWays = new int[childCount * childCount];
            int num2 = 0;
            IEnumerator enumerator = waypointsRoot.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    this.wayPoints[num2++] = current.position;
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

        public int GetNearestPointIndex(Vector3 position)
        {
            float maxValue = float.MaxValue;
            int num2 = -1;
            for (int i = 0; i < this.wayPoints.Length; i++)
            {
                Vector3 vector = this.wayPoints[i] - position;
                float sqrMagnitude = vector.sqrMagnitude;
                if (sqrMagnitude < maxValue)
                {
                    num2 = i;
                    maxValue = sqrMagnitude;
                }
            }
            return num2;
        }

        public Vector3 GetPath(Vector3 from, Vector3 to)
        {
            int wayPointIndex = this.GetWayPointIndex(from);
            int num2 = this.GetWayPointIndex(to);
            if ((wayPointIndex < 0) || ((num2 < 0) || (wayPointIndex == num2)))
            {
                return to;
            }
            int index = (wayPointIndex * this.wayPoints.Length) + num2;
            int num4 = this.bestWays[index];
            return (((num4 < 0) || (num4 == num2)) ? to : this.wayPoints[num4]);
        }

        private long GetPositionHash(Vector3 position) => 
            ((((long) (position.x / CELL_SIZE)) % ((long) WORLD_CELLS_SIZE)) + (((((long) (position.z / CELL_SIZE)) % ((long) WORLD_CELLS_SIZE)) * WORLD_CELLS_SIZE) * 2L)) + (((((((long) (position.y / CELL_SIZE)) % ((long) WORLD_CELLS_SIZE)) * WORLD_CELLS_SIZE) * 2L) * WORLD_CELLS_SIZE) * 2L);

        public int GetWayPointIndex(Vector3 position)
        {
            long positionHash = this.GetPositionHash(position);
            if (this.hash2index.ContainsKey(positionHash))
            {
                return this.hash2index[positionHash];
            }
            int nearestPointIndex = this.GetNearestPointIndex(position);
            this.hash2index.Add(positionHash, nearestPointIndex);
            return nearestPointIndex;
        }

        public void ShowWay(Vector3 from, Vector3 to)
        {
            int num = 0;
            while (true)
            {
                if (num < 0x3e8)
                {
                    Vector3 path = this.GetPath(from, to);
                    from = path;
                    if (!(path == to))
                    {
                        num++;
                        continue;
                    }
                }
                return;
            }
        }

        public bool StorePath(Vector3 from, Vector3 to, Vector3 next)
        {
            int wayPointIndex = this.GetWayPointIndex(from);
            int num2 = this.GetWayPointIndex(to);
            int num3 = this.GetWayPointIndex(next);
            if ((wayPointIndex < 0) || (num2 < 0))
            {
                return false;
            }
            int index = (wayPointIndex * this.wayPoints.Length) + num2;
            this.bestWays[index] = num3;
            return true;
        }
    }
}

