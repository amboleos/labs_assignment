using System;
using System.Collections.Generic;
using System.Linq;
using Overspace.Fluid.Container;
using Overspace.Pattern.Singleton;
using Overspace.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Overspace.Fluid
{
    public class FluidManager : MonoBehaviourSingleton<FluidManager>
    {
        public float bottleOffset = .01f;
        
        public List<FluidData> fluidData = new();
        
        public FluidStream streamPrefab;
        public GameObject bottlePrefab;
        public GameObject potionRackPrefab;

        public Transform potionHolder;
        public Transform bottleHolder;
        
        public Transform pourPoint;
        public Transform pourPotionPoint;
        public Transform bottlePoint;
        
        public GameObject shakerPrefab;
        public List<GameObject> glassPrefabs;

        [HideInInspector] public List<GameObject> glassDisplays = new List<GameObject>();
        [HideInInspector] public List<FluidBottleContainer> bottles = new();
        private MonoBehaviourPool<FluidStream> streamPool;

        private void Start()
        {
            bottles = CreateBottles();
            streamPool = new MonoBehaviourPool<FluidStream>(streamPrefab, 15, 100, true, 1f);
            CreateGlassDisplays();
            ShowGlassDisplay(-1);
        }

        public FluidData GetRandomFluid()
        {
            return fluidData[Random.Range(0, fluidData.Count)];
        }
        
        public FluidStream CreateStream()
        {
            return streamPool.Get();
        }

        public void DestroyStream(FluidStream stream)
        {
            stream.transform.parent = null;
            streamPool.Release(stream);
        }

        public FluidShakerContainer CreateGlass(int glassId)
        {
            GameObject selectedPrefab = glassPrefabs.ElementAtOrDefault(glassId);
            if (selectedPrefab == null) selectedPrefab = glassPrefabs[0];
            GameObject shakerObject = Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);
            return shakerObject.GetComponent<FluidShakerContainer>();
        }

        public void CreateGlassDisplays()
        {
            foreach (GameObject glassPrefab in glassPrefabs)
            {
                GameObject glass = Instantiate(glassPrefab, pourPoint.position, pourPoint.rotation);
                glassDisplays.Add(glass);
            }
        }

        public void ShowGlassDisplay(int index)
        {
            for (int i = 0; i < glassDisplays.Count; i++)
            {
                if(i == index) glassDisplays[i].SetActive(true);
                else glassDisplays[i].SetActive(false);
            }
        }

        private GameObject potionObj;
        
        public void CreatePotions()
        {
            if(potionObj != null) DestroyPotions();
            potionObj = Instantiate(potionRackPrefab, potionHolder.position, potionHolder.rotation);
        }

        public void DestroyPotions()
        {
            if(potionObj != null) Destroy(potionObj);
        }
        
        private List<FluidBottleContainer> CreateBottles()
        {
            List<FluidBottleContainer> createdBottles = new();

            for (int i = 0; i < fluidData.Count; i++)
            {
                GameObject bottleObject = Instantiate(bottlePrefab, Vector3.zero, Quaternion.identity);
                Vector3 targetPos = i % 2 == 0 ? bottleHolder.position + Vector3.right * bottleOffset * Mathf.Ceil(i / 2f) : bottleHolder.position - Vector3.right * bottleOffset * Mathf.Ceil(i / 2f);
                bottleObject.transform.SetPositionAndRotation(targetPos, bottleHolder.rotation);
                FluidBottleContainer bottle = bottleObject.GetComponent<FluidBottleContainer>();
                bottle.holderPosition = bottle.transform.localPosition;
                bottle.holderRotation = bottle.transform.localRotation;
                bottle.SetBottleData(fluidData[i]);
                createdBottles.Add(bottle);
            }

            return createdBottles;
        }
    }
}