using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Overspace.Fluid.Container
{
    public class FluidBottleTester : MonoBehaviour
    {
        public Button randomizeButton;
        public Button randomizeColButton;

        public Button closeButton;
        public TextMeshProUGUI debug;
        public FluidBottleContainer target;
        [Range(0f, 1f)] public float fillPercentage = 1f;
        public Texture2D[] noiseTextures;
        private static readonly int MainTexture = Shader.PropertyToID("_MainTexture");
        private static readonly int NoiseTextureOne = Shader.PropertyToID("_NoiseTextureOne");
        private static readonly int NoiseTextureTwo = Shader.PropertyToID("_NoiseTextureTwo");
        private static readonly int OuterColor = Shader.PropertyToID("_OuterColor");
        private static readonly int InnerColor = Shader.PropertyToID("_InnerColor");

        private void Start()
        {
            Randomize(true, true);

            randomizeButton.onClick.AddListener(() => Randomize(true, false));
            randomizeColButton.onClick.AddListener(() => Randomize(false, true));

            closeButton.onClick.AddListener(() => { SceneManager.LoadScene("Main"); });
        }

        private void Update()
        {
            if (target != null)
            {
                target.fillPercentage = fillPercentage;
            }
        }

        private Texture2D main, noiseOne, noiseTwo;
        private Color outer, inner;

        private void Randomize(bool text, bool col)
        {
            MeshRenderer targetRend = target.GetComponent<MeshRenderer>();

            if (text)
            {
                main = noiseTextures[Random.Range(0, noiseTextures.Length)];
                noiseOne = noiseTextures[Random.Range(0, noiseTextures.Length)];
                noiseTwo = noiseTextures[Random.Range(0, noiseTextures.Length)];
            }

            if (col)
            {
                outer = Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f);
                inner = Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f);
            }

            debug.text = $"M:{main.name}\nN1:{noiseOne.name}\nN2:{noiseTwo.name}\nC1:{outer.ToString()}\nC2:{inner.ToString()}";

            targetRend.material.SetTexture(MainTexture, main);
            targetRend.material.SetTexture(NoiseTextureOne, noiseOne);
            targetRend.material.SetTexture(NoiseTextureTwo, noiseTwo);
            targetRend.material.SetColor(OuterColor, outer);
            targetRend.material.SetColor(InnerColor, inner);
        }
    }
}