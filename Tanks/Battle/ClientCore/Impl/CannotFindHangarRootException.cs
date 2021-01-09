namespace Tanks.Battle.ClientCore.Impl
{
    using System;

    public class CannotFindHangarRootException : Exception
    {
        public CannotFindHangarRootException(string mapname) : base($"mapname={mapname}")
        {
        }
    }
}

