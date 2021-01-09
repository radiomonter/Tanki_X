namespace Platform.Library.ClientResources.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class AssetTypeRegistry
    {
        private static Dictionary<int, Type> hash2Type = new Dictionary<int, Type>();
        private static Dictionary<string, Type> extension2Type = new Dictionary<string, Type>();

        static AssetTypeRegistry()
        {
            RegisterAssetType<Object>();
            RegisterAssetType<Texture>();
            RegisterAssetType<Material>();
            RegisterAssetType<GameObject>();
            RegisterTypeExtension<Texture>(".png");
            RegisterTypeExtension<Texture>(".jpg");
            RegisterTypeExtension<Material>(".mat");
            RegisterTypeExtension<GameObject>(".fbx");
            RegisterTypeExtension<GameObject>(".prefab");
        }

        public static Type GetType(int hash) => 
            hash2Type[hash];

        public static Type GetTypeByExtension(string extension)
        {
            Type type;
            return (!extension2Type.TryGetValue(extension.ToLower(), out type) ? typeof(Object) : type);
        }

        public static int GetTypeHash(Type type) => 
            !ReferenceEquals(type, typeof(Object)) ? type.FullName.GetHashCode() : 0;

        public static int GetTypeHashByExtension(string extension) => 
            GetTypeHash(GetTypeByExtension(extension));

        private static void RegisterAssetType<T>()
        {
            RegisterAssetType(typeof(T));
        }

        private static void RegisterAssetType(Type type)
        {
            hash2Type.Add(GetTypeHash(type), type);
        }

        private static void RegisterTypeExtension<T>(string extension)
        {
            extension2Type.Add(extension.ToLower(), typeof(T));
        }
    }
}

