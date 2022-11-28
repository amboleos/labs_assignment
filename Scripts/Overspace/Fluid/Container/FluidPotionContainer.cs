using System.Collections;
using UnityEngine;

namespace Overspace.Fluid.Container
{
    public class FluidPotionContainer : FluidBaseContainer
    {
        public PotionData data;
        public GameObject cork;
        public ParticleSystem drip;
        private static readonly int OuterColor1 = Shader.PropertyToID("_OuterColor");
        private static readonly int InnerColor1 = Shader.PropertyToID("_InnerColor");
        private static readonly int MainTexture = Shader.PropertyToID("_MainTexture");
        private static readonly int NoiseOneTexture = Shader.PropertyToID("_NoiseOneTexture");
        private static readonly int NoiseTwoTexture = Shader.PropertyToID("_NoiseTwoTexture");

        protected override void Start()
        {
           SetData(data); 
        }

        public void OpenAndDrip()
        {
            cork.gameObject.SetActive(false);
            drip.Play();
            StartCoroutine(DestroyAfterDelay());
        }
        
        public void SetData(PotionData newData)
        {
            data = newData;
            fluidMeshRenderer.material.SetColor(OuterColor1, data.outer);
            fluidMeshRenderer.material.SetColor(InnerColor1, data.inner);
            fluidMeshRenderer.material.SetTexture(MainTexture, data.main);
            fluidMeshRenderer.material.SetTexture(NoiseOneTexture, data.noiseOne);
            fluidMeshRenderer.material.SetTexture(NoiseTwoTexture, data.noiseTwo);
        }

        public IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
            yield return null;
        }
    }
}