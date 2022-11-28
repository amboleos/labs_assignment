using System;
using NaughtyAttributes;
using Overspace.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Overspace.Fluid.Container
{
    public abstract class FluidBaseContainer : MonoBehaviour
    {
        [Header("Fluid Components")]
        public GameObject fluidObject;
        public Transform levelPoint;

        [Header("Fluid Settings")]
        [Range(0f, 1f)] public float fillPercentage = 1f;
        [Range(0f, 1f)] public float fillPercentageCap = .99f;
        [Range(0f, 0.002f)] public float levelCalcOffset = 0.001f;

        public bool IsFull => fillPercentage >= fillPercentageCap;
        
        private static readonly int FillPosition = Shader.PropertyToID("_FillPosition");
        private static readonly int OuterColor = Shader.PropertyToID("_OuterColor");
        private static readonly int InnerColor = Shader.PropertyToID("_InnerColor");

        public MeshRenderer fluidMeshRenderer;
        private MeshFilter fluidMeshFilter;

        private Vector3 lowestFluidPoint;
        private Vector3 highestFluidPoint;
        
        private void Awake()
        {
            fluidMeshFilter = fluidObject.GetComponent<MeshFilter>();
            fluidMeshRenderer = fluidObject.GetComponent<MeshRenderer>();
        }

        protected virtual void Start()
        {
            SetColor(Color.red);
        }

        protected virtual void Update()
        {
            fillPercentage = Mathf.Clamp(fillPercentage, 0f, fillPercentageCap);
            
            CalculateFluidPoints();
        
            Vector3 targetLevelPointPos = CalculateLevelPointPos();
            levelPoint.position = targetLevelPointPos;
            
            fluidMeshRenderer.material.SetVector(FillPosition, targetLevelPointPos);
        }

        public void SetColor(Color newColor)
        {
            Color[] tints = ColorUtils.GenerateTints(newColor);
            
            fluidMeshRenderer.material.SetColor(OuterColor, newColor);
            fluidMeshRenderer.material.SetColor(InnerColor, tints[3]);
        }
        
        private void CalculateFluidPoints()
        {
            Matrix4x4 localToWorld = fluidObject.transform.localToWorldMatrix;
        
            float lowestY = Mathf.Infinity;
            float highestY = -Mathf.Infinity;
        
            foreach (Vector3 vertexPos in fluidMeshFilter.mesh.vertices)
            {
                Vector3 correctedVertexPos = localToWorld.MultiplyPoint3x4(vertexPos);

                if (correctedVertexPos.y < lowestY)
                {
                    lowestY = correctedVertexPos.y;
                    lowestFluidPoint = correctedVertexPos;
                }
                else if (correctedVertexPos.y > highestY)
                {
                    highestY = correctedVertexPos.y;
                    highestFluidPoint = correctedVertexPos;
                }
            }
        }
    
        private Vector3 CalculateLevelPointPos()
        {
            float targetY = Mathf.Lerp(lowestFluidPoint.y - levelCalcOffset, highestFluidPoint.y + levelCalcOffset, fillPercentage);
            Vector3 currentPosition = fluidObject.transform.position;
            return new Vector3(currentPosition.x, targetY, currentPosition.z);
        }
    }
}