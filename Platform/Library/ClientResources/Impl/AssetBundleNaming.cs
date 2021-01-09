namespace Platform.Library.ClientResources.Impl
{
    using System;

    public static class AssetBundleNaming
    {
        public static readonly string DB_PATH = "/db/db.json";
        public static readonly string DB_DIR_PATH = "/db";
        public static readonly string DB_FILENAME = "db.json";
        public static readonly string EMBEDDED_BUNDLES_FILENAME = "embedded_bundles.txt";

        public static string AddCrcToBundleName(string assetBundleName, uint crc) => 
            $"{assetBundleName}_{crc:x8}.bundle";

        public static string GetAssetBundleUrl(string baseUrl, string assetBundleName) => 
            $"{baseUrl}{assetBundleName}".Replace('\\', '/');
    }
}

