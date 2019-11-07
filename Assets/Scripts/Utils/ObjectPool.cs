using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MOBRitual.Utils
{
    public sealed class ObjectPool : MonoBehaviour
    {
        public enum StartupPoolMode { Awake, Start, CallManually };

        [System.Serializable]
        public class StartupPool
        {
            public int Size;
            public GameObject Prefab;
        }

        [System.Serializable]
        public class NetworkStartupPool
        {
            public int Size;
            public string PrefabName;
        }

        static ObjectPool _instance;
        static List<GameObject> tempList = new List<GameObject>();

        private Dictionary<GameObject, List<GameObject>> _pooledObjects = new Dictionary<GameObject, List<GameObject>>();
        private Dictionary<GameObject, GameObject> _spawnedObjects = new Dictionary<GameObject, GameObject>();
        private Dictionary<int, string> _networkSpawnedObjects = new Dictionary<int, string>();
        private Dictionary<int, int> _idTranspositions = new Dictionary<int, int>();

        [SerializeField] private StartupPoolMode _startupPoolMode;
        [SerializeField] private StartupPool[] _startupPools;
        [SerializeField] private NetworkStartupPool[] _networkStartupPools;

        bool startupPoolsCreated;

        void Awake()
        {
            _instance = this;

            if (_startupPoolMode == StartupPoolMode.Awake)
                CreateStartupPools();
        }

        void Start()
        {
            if (_startupPoolMode == StartupPoolMode.Start)
                CreateStartupPools();
        }

        public static void CreateStartupPools()
        {
            if (!Instance.startupPoolsCreated)
            {
                Instance.startupPoolsCreated = true;
                var pools = Instance._startupPools;
                if (pools != null && pools.Length > 0)
                    for (int i = 0; i < pools.Length; ++i)
                        CreatePool(pools[i].Prefab, pools[i].Size);
            }
        }

        public static void CreatePoolIfNotExists<T>(T prefab, int initialPoolSize) where T : Component
        {
            if (prefab != null)
                CreatePoolIfNotExists(prefab.gameObject, initialPoolSize);
        }

        public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
        {
            CreatePool(prefab.gameObject, initialPoolSize);
        }

        public static void CreatePoolIfNotExists(GameObject prefab, int initialPoolSize)
        {
            if (!IsPoolExist(prefab))
                CreatePool(prefab, initialPoolSize);
        }

        public static void CreatePool(GameObject prefab, int initialPoolSize)
        {
            if (prefab != null && Instance != null && !Instance._pooledObjects.ContainsKey(prefab))
            {
                var list = new List<GameObject>();
                Instance._pooledObjects.Add(prefab, list);

                if (initialPoolSize > 0)
                {
                    bool active = prefab.activeSelf;
                    prefab.SetActive(false);
                    Transform parent = Instance.transform;
                    while (list.Count < initialPoolSize)
                    {
                        var obj = Instantiate(prefab);
                        obj.transform.SetParent(parent);
                        list.Add(obj);
                    }
                    prefab.SetActive(active);
                }
            }
        }

        public static bool IsPoolExist(GameObject prefab)
        {
            return prefab != null && Instance._pooledObjects.ContainsKey(prefab);
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Vector3 position, bool setActive) where T : Component
            {
                return Spawn(prefab.gameObject, null, position, Quaternion.identity, true, setActive).GetComponent<T>();
            }
        public static T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab, Transform parent, bool worldPositionStays) where T : Component
        {
            return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity, worldPositionStays, true).GetComponent<T>();
        }
        public static T Spawn<T>(T prefab) where T : Component
        {
            return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }
        public static T Spawn<T>(GameObject prefab, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion)) where T : Component
        {
            return Spawn(prefab, null, position, rotation).GetComponent<T>();
        }
        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            return Spawn(prefab, parent, position, rotation, true, true);
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, bool worldPositionStays, bool setActive)
        {
            List<GameObject> list;
            Transform trans;
            GameObject obj;
            if (Instance._pooledObjects.TryGetValue(prefab, out list))
            {
                obj = null;
                if (list.Count > 0)
                {
                    while (obj == null && list.Count > 0)
                    {
                        obj = list[0];
                        list.RemoveAt(0);
                    }
                }

                if (obj == null)
                    obj = Instantiate(prefab);
                trans = obj.transform;
                trans.SetParent(parent, worldPositionStays);
                obj.SetActive(setActive);
                trans.localPosition = position;
                trans.localRotation = rotation;
                Instance._spawnedObjects.Add(obj, prefab);

                return obj;
            }

            obj = Instantiate(prefab);
            obj.SetActive(setActive);
            trans = obj.transform;
            trans.SetParent(parent, worldPositionStays);
            trans.localPosition = position;
            trans.localRotation = rotation;
            return obj;
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
        {
            return Spawn(prefab, parent, position, Quaternion.identity);
        }
        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Spawn(prefab, null, position, rotation);
        }
        public static GameObject Spawn(GameObject prefab, Transform parent)
        {
            return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return Spawn(prefab, null, position, Quaternion.identity);
        }
        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(T obj, bool worldPosStay = true) where T : Component
        {
            Recycle(obj.gameObject, worldPosStay);
        }
        public static void Recycle(GameObject obj, bool worldPosStay = true)
        {
            GameObject prefab;
            if (Instance != null && Instance._spawnedObjects.TryGetValue(obj, out prefab))
                Recycle(obj, prefab, worldPosStay);
            else
                Destroy(obj);
        }

        public static void Recycle(GameObject obj, float time)
        {
            Instance.StartCoroutine(Wait(time, () => Recycle(obj)));
        }
        static IEnumerator Wait(float time, System.Action a)
        {
            yield return new WaitForSeconds(time);
            a();
        }

        static void Recycle(GameObject obj, GameObject prefab, bool worldPositionStays = true)
        {
            if (obj == null)
                return;

            if (Instance == null)
            {
                Destroy(obj);
                return;
            }

            Instance._pooledObjects[prefab].Add(obj);
            Instance._spawnedObjects.Remove(obj);

            obj.transform.SetParent(Instance.transform, worldPositionStays);
            obj.SetActive(false);
        }

        public static void RecycleAll<T>(T prefab) where T : Component
        {
            RecycleAll(prefab.gameObject);
        }
        public static void RecycleAll(GameObject prefab)
        {
            foreach (var item in Instance._spawnedObjects)
                if (item.Value == prefab)
                    tempList.Add(item.Key);
            for (int i = 0; i < tempList.Count; ++i)
                Recycle(tempList[i]);
            tempList.Clear();
        }

        public static void RecycleAll()
        {
            tempList.AddRange(Instance._spawnedObjects.Keys);
            int i;
            for (i = 0; i < tempList.Count; ++i)
                Recycle(tempList[i]);
            tempList.Clear();
        }

        public static bool IsSpawned(GameObject obj)
        {
            return Instance._spawnedObjects.ContainsKey(obj);
        }

        public static int CountPooled<T>(T prefab) where T : Component
        {
            return CountPooled(prefab.gameObject);
        }
        public static int CountPooled(GameObject prefab)
        {
            List<GameObject> list;
            if (Instance._pooledObjects.TryGetValue(prefab, out list))
                return list.Count;
            return 0;
        }

        public static int CountSpawned<T>(T prefab) where T : Component
        {
            return CountSpawned(prefab.gameObject);
        }
        public static int CountSpawned(GameObject prefab)
        {
            int count = 0;
            foreach (var instancePrefab in Instance._spawnedObjects.Values)
                if (prefab == instancePrefab)
                    ++count;
            return count;
        }

        public static int CountAllPooled()
        {
            int count = 0;
            foreach (var list in Instance._pooledObjects.Values)
                count += list.Count;
            return count;
        }

        public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null)
                list = new List<GameObject>();
            if (!appendList)
                list.Clear();
            List<GameObject> pooled;
            if (Instance._pooledObjects.TryGetValue(prefab, out pooled))
                list.AddRange(pooled);
            return list;
        }
        public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null)
                list = new List<T>();
            if (!appendList)
                list.Clear();
            List<GameObject> pooled;
            if (Instance._pooledObjects.TryGetValue(prefab.gameObject, out pooled))
                for (int i = 0; i < pooled.Count; ++i)
                    list.Add(pooled[i].GetComponent<T>());
            return list;
        }

        public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null)
                list = new List<GameObject>();
            if (!appendList)
                list.Clear();
            foreach (var item in Instance._spawnedObjects)
                if (item.Value == prefab)
                    list.Add(item.Key);
            return list;
        }
        public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null)
                list = new List<T>();
            if (!appendList)
                list.Clear();
            var prefabObj = prefab.gameObject;
            foreach (var item in Instance._spawnedObjects)
                if (item.Value == prefabObj)
                    list.Add(item.Key.GetComponent<T>());
            return list;
        }

        public static ObjectPool Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindObjectOfType<ObjectPool>();
                if (_instance != null)
                    return _instance;

                return null;
            }
        }
    }

    public static class ObjectPoolExtensions
    {
        public static void CreatePool<T>(this T prefab) where T : Component
        {
            ObjectPool.CreatePool(prefab, 0);
        }
        public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
        {
            ObjectPool.CreatePool(prefab, initialPoolSize);
        }
        public static void CreatePool(this GameObject prefab)
        {
            ObjectPool.CreatePool(prefab, 0);
        }
        public static void CreatePool(this GameObject prefab, int initialPoolSize)
        {
            ObjectPool.CreatePool(prefab, initialPoolSize);
        }

        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, position, rotation);
        }
        public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, position, rotation);
        }
        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }
        public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }
        public static T Spawn<T>(this T prefab, Transform parent) where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public static T Spawn<T>(this T prefab) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            return ObjectPool.Spawn(prefab, parent, position, rotation);
        }
        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return ObjectPool.Spawn(prefab, null, position, rotation);
        }
        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
        {
            return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab, Vector3 position)
        {
            return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab, Transform parent)
        {
            return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }
        public static GameObject Spawn(this GameObject prefab)
        {
            return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(this T obj) where T : Component
        {
            ObjectPool.Recycle(obj);
        }
        public static void Recycle(this GameObject obj)
        {
            ObjectPool.Recycle(obj);
        }

        public static void RecycleAll<T>(this T prefab) where T : Component
        {
            ObjectPool.RecycleAll(prefab);
        }
        public static void RecycleAll(this GameObject prefab)
        {
            ObjectPool.RecycleAll(prefab);
        }

        public static int CountPooled<T>(this T prefab) where T : Component
        {
            return ObjectPool.CountPooled(prefab);
        }
        public static int CountPooled(this GameObject prefab)
        {
            return ObjectPool.CountPooled(prefab);
        }

        public static int CountSpawned<T>(this T prefab) where T : Component
        {
            return ObjectPool.CountSpawned(prefab);
        }
        public static int CountSpawned(this GameObject prefab)
        {
            return ObjectPool.CountSpawned(prefab);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return ObjectPool.GetSpawned(prefab, list, appendList);
        }
        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
        {
            return ObjectPool.GetSpawned(prefab, list, false);
        }
        public static List<GameObject> GetSpawned(this GameObject prefab)
        {
            return ObjectPool.GetSpawned(prefab, null, false);
        }
        public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, list, appendList);
        }
        public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, list, false);
        }
        public static List<T> GetSpawned<T>(this T prefab) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, null, false);
        }

        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return ObjectPool.GetPooled(prefab, list, appendList);
        }
        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
        {
            return ObjectPool.GetPooled(prefab, list, false);
        }
        public static List<GameObject> GetPooled(this GameObject prefab)
        {
            return ObjectPool.GetPooled(prefab, null, false);
        }
        public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return ObjectPool.GetPooled(prefab, list, appendList);
        }
        public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
        {
            return ObjectPool.GetPooled(prefab, list, false);
        }
        public static List<T> GetPooled<T>(this T prefab) where T : Component
        {
            return ObjectPool.GetPooled(prefab, null, false);
        }
    }
}