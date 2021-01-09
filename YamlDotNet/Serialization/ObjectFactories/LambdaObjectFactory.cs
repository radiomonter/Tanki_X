namespace YamlDotNet.Serialization.ObjectFactories
{
    using System;
    using YamlDotNet.Serialization;

    public sealed class LambdaObjectFactory : IObjectFactory
    {
        private readonly Func<Type, object> _factory;

        public LambdaObjectFactory(Func<Type, object> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            this._factory = factory;
        }

        public object Create(Type type) => 
            this._factory(type);
    }
}

