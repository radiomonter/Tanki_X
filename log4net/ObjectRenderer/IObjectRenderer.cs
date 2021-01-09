namespace log4net.ObjectRenderer
{
    using System;
    using System.IO;

    public interface IObjectRenderer
    {
        void RenderObject(RendererMap rendererMap, object obj, TextWriter writer);
    }
}

