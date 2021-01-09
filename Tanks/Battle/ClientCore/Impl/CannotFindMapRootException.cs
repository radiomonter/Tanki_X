namespace Tanks.Battle.ClientCore.Impl
{
    using System;

    public class CannotFindMapRootException : Exception
    {
        public CannotFindMapRootException(string mapname) : base($"mapname={mapname}")
        {
        }
    }
}

