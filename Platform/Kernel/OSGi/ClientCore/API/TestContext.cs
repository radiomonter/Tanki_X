namespace Platform.Kernel.OSGi.ClientCore.API
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;

    public class TestContext
    {
        private ThreadLocal<IDictionary> data = new ThreadLocal<IDictionary>();
        private static ThreadLocal<TestContext> context = new ThreadLocal<TestContext>();

        private TestContext()
        {
            this.data.Set(new ListDictionary());
            this.SpyEntity = false;
        }

        public static void EnterTestMode()
        {
            context.Set(new TestContext());
        }

        public object GetData(object key) => 
            this.data.Get()[key];

        public bool IsDataExists(object key) => 
            this.data.Get().Contains(key);

        public void PutData(object key, object value)
        {
            this.data.Get().Add(key, value);
        }

        public bool SpyEntity { get; set; }

        public static bool IsTestMode =>
            context.Exists();

        public static TestContext Current =>
            context.Get();
    }
}

