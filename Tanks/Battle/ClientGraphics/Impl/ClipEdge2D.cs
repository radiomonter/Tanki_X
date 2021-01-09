﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class ClipEdge2D
    {
        public Vector2 from;
        public Vector2 to;

        public ClipEdge2D(Vector2 from, Vector2 to)
        {
            this.from = from;
            this.to = to;
        }
    }
}

