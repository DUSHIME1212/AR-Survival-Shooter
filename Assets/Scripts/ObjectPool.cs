// Scripts/Pooling/ObjectPool.cs
using UnityEngine;
using System.Collections.Generic;

namespace ARSurvivalShooter
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initialSize = 20;

        private Queue<GameObject> pool = new Queue<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < initialSize; i++)
                CreateNew();
        }

        private void CreateNew()
        {
            GameObject obj = Instantiate(prefab, transform); // parent keeps hierarchy clean
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            if (prefab == null) { Debug.LogError($"[ObjectPool] Prefab is null on {gameObject.name}!"); return null; }
            
            bool fromPool = pool.Count > 0;
            GameObject obj = fromPool ? pool.Dequeue() : Instantiate(prefab, null); 
            
            Debug.Log($"[ObjectPool] {gameObject.name} providing object. From pool: {fromPool}. Remaining: {pool.Count}");
            
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.transform.SetParent(null); 
            obj.SetActive(true);
            return obj;
        }

        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform); // Parent back to pool for organization
            pool.Enqueue(obj);
        }
    }
}