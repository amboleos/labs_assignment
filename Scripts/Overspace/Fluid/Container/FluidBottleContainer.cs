using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Overspace.Fluid.Container
{
    public class FluidBottleContainer : FluidBaseContainer
    {
        [Header("Bottle Components")] 
        public Transform pourPoint;
        public GameObject capObject;
        public Image labelImage;

        [Header("Bottle Data")] 
        public FluidData defaultData;

        [Header("Bottle Settings")] 
        [Range(0f, 0.05f)] public float pourOffset;
        
        [HideInInspector] public bool isCapOn = true;
        [HideInInspector] public FluidData data;
        [HideInInspector] public bool isPouring;

        private FluidStream currentStream;

        [HideInInspector] public Vector3 holderPosition;
        [HideInInspector] public Quaternion holderRotation;
        
        protected override void Start()
        {
            if (defaultData != null)
            {
                SetBottleData(defaultData);
            }
        }

        protected override void Update()
        {
            base.Update();
            HandlePourLogic();
        }

        public void SetBottleData(FluidData newData)
        {
            data = newData;
            SetColor(data.fluidColor);
            labelImage.sprite = data.fluidIcon;
        }
        
        private bool CheckIfShouldPour()
        {
            if (isCapOn) return false;
            bool angleCheck = pourPoint.position.y - pourOffset < levelPoint.position.y;
            return angleCheck;
        }

        private void HandlePourLogic()
        {
            if (isPouring != CheckIfShouldPour())
            {
                isPouring = CheckIfShouldPour();

                if (isPouring)
                {
                    StartPour();
                }
                else
                {
                    EndPour();
                }
            }

            if (isPouring)
            {
                fillPercentage -= .05f * Time.deltaTime;
            }
        }

        public void SetCapState(bool state)
        {
            capObject.SetActive(state);
            isCapOn = state;
        }
        
        private void StartPour()
        {
            currentStream = FluidManager.Instance.CreateStream();
            currentStream.SetData(data);
            currentStream.transform.parent = pourPoint;
            currentStream.transform.localPosition = Vector3.zero;
            currentStream.Begin();
        }
    
        private void EndPour()
        {
            currentStream.End();
            currentStream = null;
        }

        public void ReturnToHolder()
        {
            SetCapState(true);
            transform.localPosition = holderPosition;
            transform.localRotation = holderRotation;
        }

        public void Fill()
        {
            fillPercentage = fillPercentageCap;
        }
        
        [Button]
        public void ToggleCap()
        {
            SetCapState(!isCapOn);
        }
    }
}