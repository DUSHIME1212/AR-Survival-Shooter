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
            GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, transform);
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }

        public void Return(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}