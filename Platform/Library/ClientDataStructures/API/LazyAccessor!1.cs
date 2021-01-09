namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public class LazyAccessor<T>
    {
        private T value;
        private readonly Func<T> initializer;

        public LazyAccessor(Func<T> initializer)
        {
            this.initializer = initializer;
        }

        public LazyAccessor(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get
            {
                if (this.value == null)
                {
                    this.value = this.initializer();
                }
                return this.value;
            }
        }
    }
}

