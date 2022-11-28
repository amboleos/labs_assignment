using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Overspace.Pooling
{
    public class MonoBehaviourPool<T> where T : MonoBehaviour
    {
        private readonly T prefab;
        private readonly int prewarmCount;

        private readonly ObjectPool<T> internalPool;

        private bool IsReady => internalPool != null;
        
        public MonoBehaviourPool(T prefab, int defaultCapacity, int maxCapacity, bool shouldPrewarm = false, float prewarmPercentage = 0)
        {
            this.prefab = prefab;

            if (defaultCapacity < 5)
            {
                Debug.LogWarning($"{this} : POOL DEFAULT CAPACITY IS TOO LOW, SETTING TO 5");
                defaultCapacity = 5;
            }

            int minimumMaxCapacity = (int)Math.Round(defaultCapacity * 1.5);
            if (maxCapacity <= minimumMaxCapacity)
            {
                Debug.LogWarning($"{this} : POOL MAXIMUM CAPACITY IS TOO LOW, SETTING TO {minimumMaxCapacity}");
                maxCapacity = minimumMaxCapacity;
            }

            internalPool = new ObjectPool<T>(OnObjectCreate, OnObjectGet, OnObjectRelease, OnObjectDestroy, true,
                defaultCapacity, maxCapacity);
            if (!shouldPrewarm) return;
            prewarmCount = (int)Math.Clamp(defaultCapacity * prewarmPercentage, 1, defaultCapacity);
            Prewarm();
        }

        private void Prewarm()
        {
            List<T> prewarmList = GetMultiple(prewarmCount);
            foreach (T poolable in prewarmList)
            {
                Release(poolable);
            }
        }

        public T Get()
        {
            return !IsReady ? null : internalPool.Get();
        }

        public List<T> GetMultiple(int count)
        {
            if (!IsReady) return null;
            List<T> getList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                T newPoolable = Get();
                if (newPoolable) getList.Add(newPoolable);
            }

            return getList;
        }

        public void Release(T poolable)
        {
            if (!IsReady) return;
            internalPool.Release(poolable);
        }

        private T OnObjectCreate()
        {
            T poolable = Object.Instantiate(prefab);
            poolable.gameObject.name = $"{prefab.gameObject.name} - {typeof(T).Name}";
            return poolable;
        }

        private void OnObjectGet(T poolable)
        {
            poolable.gameObject.SetActive(true);
        }

        private void OnObjectRelease(T poolable)
        {
            poolable.gameObject.SetActive(false);
        }

        private void OnObjectDestroy(T poolable)
        {
            Object.Destroy(poolable);
        }
    }
}