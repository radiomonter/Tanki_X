namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class TrackRenderData
    {
        public TrackSector[] sectors;
        public int maxSectorCountPerTrack;
        public int lastSectorIndex;
        public int sectorCount;
        public int firstSectorToHide;
        public float baseAlpha;
        public int parts;
        public float texturePart;
        public int currentPart;

        public TrackRenderData(int maxSectorCountPerTrack, int firstSectorToHide, float baseAlpha, int parts)
        {
            this.maxSectorCountPerTrack = maxSectorCountPerTrack;
            this.firstSectorToHide = firstSectorToHide;
            this.baseAlpha = baseAlpha;
            this.parts = parts;
            this.texturePart = 1f / ((float) parts);
            this.sectors = new TrackSector[maxSectorCountPerTrack];
            this.Reset();
        }

        public void Reset()
        {
            this.lastSectorIndex = -1;
            this.sectorCount = 0;
            this.currentPart = 0;
        }
    }
}

