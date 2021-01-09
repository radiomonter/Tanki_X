namespace log4net.ObjectRenderer
{
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;

    public class RendererMap
    {
        private static readonly Type declaringType = typeof(RendererMap);
        private Hashtable m_map = Hashtable.Synchronized(new Hashtable());
        private Hashtable m_cache = new Hashtable();
        private static IObjectRenderer s_defaultRenderer = new log4net.ObjectRenderer.DefaultRenderer();

        public void Clear()
        {
            this.m_map.Clear();
            this.m_cache.Clear();
        }

        public string FindAndRender(object obj)
        {
            string str = obj as string;
            if (str != null)
            {
                return str;
            }
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            this.FindAndRender(obj, writer);
            return writer.ToString();
        }

        public void FindAndRender(object obj, TextWriter writer)
        {
            if (obj == null)
            {
                writer.Write(SystemInfo.NullText);
            }
            else
            {
                string str = obj as string;
                if (str != null)
                {
                    writer.Write(str);
                }
                else
                {
                    try
                    {
                        this.Get(obj.GetType()).RenderObject(this, obj, writer);
                    }
                    catch (Exception exception)
                    {
                        LogLog.Error(declaringType, "Exception while rendering object of type [" + obj.GetType().FullName + "]", exception);
                        string fullName = string.Empty;
                        if ((obj != null) && (obj.GetType() != null))
                        {
                            fullName = obj.GetType().FullName;
                        }
                        writer.Write("<log4net.Error>Exception rendering object type [" + fullName + "]");
                        if (exception != null)
                        {
                            string str3 = null;
                            try
                            {
                                str3 = exception.ToString();
                            }
                            catch
                            {
                            }
                            writer.Write("<stackTrace>" + str3 + "</stackTrace>");
                        }
                        writer.Write("</log4net.Error>");
                    }
                }
            }
        }

        public IObjectRenderer Get(object obj) => 
            (obj != null) ? this.Get(obj.GetType()) : null;

        public IObjectRenderer Get(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            IObjectRenderer renderer = null;
            renderer = (IObjectRenderer) this.m_cache[type];
            if (renderer == null)
            {
                Type baseType = type;
                while (true)
                {
                    if (baseType != null)
                    {
                        renderer = this.SearchTypeAndInterfaces(baseType);
                        if (renderer == null)
                        {
                            baseType = baseType.BaseType;
                            continue;
                        }
                    }
                    renderer ??= s_defaultRenderer;
                    this.m_cache[type] = renderer;
                    break;
                }
            }
            return renderer;
        }

        public void Put(Type typeToRender, IObjectRenderer renderer)
        {
            this.m_cache.Clear();
            if (typeToRender == null)
            {
                throw new ArgumentNullException("typeToRender");
            }
            if (renderer == null)
            {
                throw new ArgumentNullException("renderer");
            }
            this.m_map[typeToRender] = renderer;
        }

        private IObjectRenderer SearchTypeAndInterfaces(Type type)
        {
            IObjectRenderer renderer = (IObjectRenderer) this.m_map[type];
            if (renderer != null)
            {
                return renderer;
            }
            foreach (Type type2 in type.GetInterfaces())
            {
                renderer = this.SearchTypeAndInterfaces(type2);
                if (renderer != null)
                {
                    return renderer;
                }
            }
            return null;
        }

        public IObjectRenderer DefaultRenderer =>
            s_defaultRenderer;
    }
}

