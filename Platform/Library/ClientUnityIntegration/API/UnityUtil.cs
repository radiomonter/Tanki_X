namespace Platform.Library.ClientUnityIntegration.API
{
    using log4net;
    using Platform.Library.ClientLogger.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using YamlDotNet.Serialization;

    public static class UnityUtil
    {
        private static ILog log;

        [Conditional("UNITY_EDITOR")]
        public static void Debug(this object obj)
        {
        }

        [Conditional("UNITY_EDITOR")]
        public static void Debug(this object obj, string message)
        {
            MethodBase method = new StackTrace().GetFrame(1).GetMethod();
            UnityEngine.Debug.Log($"{Time.frameCount}: <i>{method.ReflectedType.Name}->{method.Name}:</i> <b>{obj}</b> {message}");
        }

        public static void Destroy(Object obj)
        {
            Object.Destroy(obj);
        }

        public static void DestroyChildren(this Transform root)
        {
            IEnumerator enumerator = root.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    Object.Destroy(current.gameObject);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public static void DestroyComponentsInChildren<T>(this GameObject go) where T: Component
        {
            foreach (T local in go.GetComponentsInChildren<T>(true))
            {
                Object.Destroy(local);
            }
        }

        public static string GetGameObjectPath(this GameObject obj)
        {
            string str = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                str = "/" + obj.name + str;
            }
            return str;
        }

        private static ILog GetLog()
        {
            log ??= LoggerProvider.GetLogger(typeof(UnityUtil));
            return log;
        }

        public static void InheritAndEmplace(Transform child, Transform parent)
        {
            child.parent = parent;
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
        }

        public static void LoadScene(Object sceneAsset, string sceneAssetName, bool additive)
        {
            LoadSceneMode mode = !additive ? LoadSceneMode.Single : LoadSceneMode.Additive;
            GetLog().InfoFormat("LoadLevel {0}", sceneAssetName);
            SceneManager.LoadScene(sceneAssetName, mode);
        }

        public static AsyncOperation LoadSceneAsync(Object sceneAsset, string sceneAssetName)
        {
            GetLog().InfoFormat("LoadSceneAsync {0}", sceneAssetName);
            return SceneManager.LoadSceneAsync(sceneAssetName, LoadSceneMode.Single);
        }

        public static void SetPropertiesFromYamlNode(object target, YamlNode componentYamlNode, INamingConvention nameConvertor)
        {
            foreach (PropertyInfo info in target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string key = nameConvertor.Apply(info.Name);
                if (componentYamlNode.HasValue(key) && info.CanWrite)
                {
                    try
                    {
                        info.SetValue(target, componentYamlNode.GetValue(key), null);
                    }
                    catch (ArgumentException)
                    {
                        object[] args = new object[] { info.PropertyType, componentYamlNode.GetValue(key).GetType() };
                        UnityEngine.Debug.LogFormat("Can't convert to {0} from {1}", args);
                    }
                }
            }
        }
    }
}

