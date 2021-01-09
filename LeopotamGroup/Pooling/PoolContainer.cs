namespace LeopotamGroup.Pooling
{
    using LeopotamGroup.Collections;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public sealed class PoolContainer : MonoBehaviour
    {
        [SerializeField]
        private string _prefabPath = "UnknownPrefab";
        public GameObject CustomPrefab;
        [SerializeField]
        private Transform _itemsRoot;
        private readonly FastStack<IPoolObject> _store = new FastStack<IPoolObject>(0x20, null);
        private Object _cachedAsset;
        private Vector3 _cachedScale;
        private bool _needToAddPoolObject;
        private Type _overridedType;

        public static PoolContainer CreatePool<T>(string prefabPath, Transform itemsRoot = null) where T: IPoolObject => 
            CreatePool(prefabPath, itemsRoot, typeof(T));

        public static PoolContainer CreatePool(string prefabPath, Transform itemsRoot = null, Type overridedType = null)
        {
            if (string.IsNullOrEmpty(prefabPath))
            {
                return null;
            }
            if ((overridedType != null) && !typeof(IPoolObject).IsAssignableFrom(overridedType))
            {
                object[] args = new object[] { overridedType, prefabPath };
                Debug.LogWarningFormat("Invalid IPoolObject-type \"{0}\" for prefab \"{1}\"", args);
                return null;
            }
            PoolContainer container = new GameObject().AddComponent<PoolContainer>();
            container._prefabPath = prefabPath;
            container._itemsRoot = itemsRoot;
            container._overridedType = overridedType;
            return container;
        }

        public IPoolObject Get()
        {
            bool flag;
            return this.Get(out flag);
        }

        public IPoolObject Get(out bool isNew)
        {
            IPoolObject obj2;
            if ((this._cachedAsset == null) && !this.LoadPrefab())
            {
                isNew = true;
                return null;
            }
            if (this._store.Count > 0)
            {
                obj2 = this._store.Pop();
                isNew = false;
            }
            else
            {
                obj2 = !this._needToAddPoolObject ? ((IPoolObject) Instantiate(this._cachedAsset)) : ((IPoolObject) ((GameObject) Instantiate(this._cachedAsset)).AddComponent(this._overridedType));
                obj2.PoolContainer = this;
                Transform poolTransform = obj2.PoolTransform;
                if (poolTransform != null)
                {
                    poolTransform.gameObject.SetActive(false);
                    poolTransform.SetParent(this._itemsRoot, false);
                    poolTransform.localScale = this._cachedScale;
                }
                isNew = true;
            }
            return obj2;
        }

        private bool LoadPrefab()
        {
            GameObject obj2 = (this.CustomPrefab == null) ? Resources.Load<GameObject>(this._prefabPath) : this.CustomPrefab;
            if (obj2 == null)
            {
                Debug.LogWarning("Cant load asset " + this._prefabPath, base.gameObject);
                return false;
            }
            this._cachedAsset = obj2.GetComponent(typeof(IPoolObject));
            this._needToAddPoolObject = ReferenceEquals(this._cachedAsset, null);
            if (this._needToAddPoolObject)
            {
                this._cachedAsset = obj2;
                this._overridedType = typeof(PoolObject);
            }
            else if (!ReferenceEquals(this._cachedAsset.GetType(), this._overridedType))
            {
                Debug.LogWarning("Prefab already contains another IPoolObject-component", base.gameObject);
                return false;
            }
            this._cachedScale = obj2.transform.localScale;
            this._store.UseCastToObjectComparer(true);
            this._itemsRoot ??= base.transform;
            return true;
        }

        public void Recycle(IPoolObject obj, bool checkForDoubleRecycle = true)
        {
            if (obj != null)
            {
                Transform poolTransform = obj.PoolTransform;
                if (poolTransform != null)
                {
                    poolTransform.gameObject.SetActive(false);
                    if (!ReferenceEquals(poolTransform.parent, this._itemsRoot))
                    {
                        poolTransform.SetParent(this._itemsRoot, true);
                    }
                }
                if (!checkForDoubleRecycle || !this._store.Contains(obj))
                {
                    this._store.Push(obj);
                }
            }
        }

        public string PrefabPath
        {
            get => 
                this._prefabPath;
            set => 
                this._prefabPath = value;
        }

        public Transform ItemsRoot
        {
            get => 
                this._itemsRoot;
            set => 
                this._itemsRoot = value;
        }
    }
}

