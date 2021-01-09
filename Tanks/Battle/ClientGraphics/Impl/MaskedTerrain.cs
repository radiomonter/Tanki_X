namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class MaskedTerrain
    {
        private readonly Mask mask;
        private readonly List<MeshCollider> meshColliders;
        private Bounds terrainBounds;
        private float pixelWidthInWorld;
        private float pixelLengthInWorld;
        private int counter;

        public MaskedTerrain(Terrain terrain, Mask mask)
        {
            this.meshColliders = terrain.MeshColliders;
            this.terrainBounds = terrain.Bounds;
            this.mask = mask;
            this.pixelWidthInWorld = this.terrainBounds.size.x / ((float) mask.Width);
            this.pixelLengthInWorld = this.terrainBounds.size.z / ((float) mask.Height);
        }

        public float CalculateMarkedSquare() => 
            (float) ((int) ((this.pixelWidthInWorld * this.pixelLengthInWorld) * this.mask.MarkedPixels.Count));

        public bool FindPosition(int pixelX, int pixelY, out GrassPosition position)
        {
            bool flag;
            float z = this.terrainBounds.min.z + (pixelY * this.pixelLengthInWorld);
            Ray ray = new Ray(new Vector3(this.terrainBounds.min.x + (pixelX * this.pixelWidthInWorld), (this.terrainBounds.max.y + 10f) + 1f, z), new Vector3(0f, -1f, 0f));
            float maxDistance = this.terrainBounds.size.y + 100f;
            position = new GrassPosition();
            using (List<MeshCollider>.Enumerator enumerator = this.meshColliders.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        RaycastHit hit;
                        RaycastHit hit2;
                        MeshCollider current = enumerator.Current;
                        Renderer component = current.GetComponent<Renderer>();
                        if (!current.Raycast(ray, out hit, maxDistance))
                        {
                            continue;
                        }
                        position.position = hit.point;
                        if (Physics.Raycast(position.position + (Vector3.up * 2f), -Vector3.up, out hit2, 2f) && ((hit2.point.y > (position.position.y + 0.01f)) && (hit2.collider != hit.collider)))
                        {
                            continue;
                        }
                        int lightmapIndex = component.lightmapIndex;
                        if (lightmapIndex < 0)
                        {
                            position.lightmapId = lightmapIndex;
                            flag = true;
                        }
                        else
                        {
                            Vector2 lightmapCoord = hit.lightmapCoord;
                            position.lightmapUV = lightmapCoord;
                            position.lightmapId = lightmapIndex;
                            flag = true;
                        }
                    }
                    else
                    {
                        position.position = ray.origin;
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        public List<GrassPosition> FindPositions(List<Vector2> pixelsCoords)
        {
            List<GrassPosition> list = new List<GrassPosition>();
            for (int i = 0; i < pixelsCoords.Count; i++)
            {
                GrassPosition position;
                Vector2 vector = pixelsCoords[i];
                if (this.FindPosition((int) vector.x, (int) vector.y, out position))
                {
                    list.Add(position);
                }
            }
            return list;
        }
    }
}

