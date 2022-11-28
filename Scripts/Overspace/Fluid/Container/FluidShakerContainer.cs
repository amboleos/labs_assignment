using System.Collections.Generic;
using System.Linq;
using colorKit;
using NaughtyAttributes;
using Overspace.Utils;
using UnityEngine;

namespace Overspace.Fluid.Container
{
    public class FluidShakerContainer : FluidBaseContainer
    {
        public readonly Dictionary<FluidData, float> Data = new();

        private bool dicDirtyFlag;

        protected override void Start()
        {
            foreach (FluidData fluidData in FluidManager.Instance.fluidData)
            {
                Data.Add(fluidData, 0f);
            }
            
            CalculateContents();
        }

        protected override void Update()
        {
            if(dicDirtyFlag) CalculateContents();
            base.Update();
        }

        private void CalculateContents()
        {
            if (Data.Count == 0 || Data == null)
            {
                SetColor(Color.white);
                dicDirtyFlag = false;
                return;
            }
            
            fillPercentage = Data.Sum(x => x.Value);

            Color[] colors = Data.Keys.ToList().Select(x => x.fluidColor).ToArray();
            float[] weights = Data.Values.ToArray();

            Color mixedColor = colorMixing.mixColors(colors, weights, colorSpace.RYB, mixingMethod.colorComponentAveraging);
            SetColor(mixedColor);

            dicDirtyFlag = false;
        }

        public void AddFluid(FluidData fluid, float val)
        {
            if(IsFull) return;
            Data[fluid] += val;
            Data[fluid] = Mathf.Clamp01(Data[fluid]);
            dicDirtyFlag = true;
        }

        public bool CompareToOrder(IEnumerable<FluidData> order)
        {
            bool isOk = true;
            
            foreach (FluidData fluid in order)
            {
                if (Data[fluid] < .175f) isOk = false;
            }

            return isOk;
        }
    }
}