namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class YamlClassSerializerException : Exception
    {
        public YamlClassSerializerException(string className, Exception e) : base("className=" + className, e)
        {
        }
    }
}

