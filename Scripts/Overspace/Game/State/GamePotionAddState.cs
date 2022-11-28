using System.Collections;
using Overspace.Camera;
using Overspace.Fluid;
using Overspace.Fluid.Container;
using Overspace.GUI;
using Overspace.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Overspace.Game.State
{
    public class GamePotionAddState : GameBaseState
    {
        public override string StateName => "POTION ADD";
        public override void Enter()
        {
            pingPong = false;

            GUIManager.Instance.helpText.text = "Tap when the reaction bubble turns green";
            GUIManager.Instance.helpText.gameObject.SetActive(true);
            
            Image reaction = GUIManager.Instance.reaction;
            
            reaction.rectTransform.localScale = Vector3.one;
            reaction.rectTransform.root.gameObject.SetActive(true);
            reaction.gameObject.SetActive(true);

            CameraManager.Instance.SetCamera(CameraManager.Instance.pourPotionCam);
            
            if (GameManager.Instance.selectedPotion == null)
            {
                GameManager.Instance.ChangeState(GameManager.Instance.PotionState);
            }
            else
            {
                GameManager.Instance.selectedPotion.transform.SetPositionAndRotation(FluidManager.Instance.pourPotionPoint.position, FluidManager.Instance.pourPotionPoint.rotation);
            }
        }

        public override void Exit()
        {
            pingPong = false;
            didClick = false;
            buffer = 0f;
            
            GUIManager.Instance.reaction.rectTransform.localScale = Vector3.one;
            GUIManager.Instance.reaction.gameObject.SetActive(false);
            GUIManager.Instance.helpText.gameObject.SetActive(false);
            GameManager.Instance.selectedPotion = null;
        }

        private bool pingPong;
        private bool didClick;
        private float buffer;
        private static readonly int MainTexture = Shader.PropertyToID("_MainTexture");
        private static readonly int NoiseOneTexture = Shader.PropertyToID("_NoiseOneTexture");
        private static readonly int NoiseTwoTexture = Shader.PropertyToID("_NoiseTwoTexture");

        public override void Tick()
        {
            buffer += Time.deltaTime;
            
            Image reaction = GUIManager.Instance.reaction;

            if (didClick) { return; }
            
            if (pingPong == false)
            {
                reaction.rectTransform.localScale = Vector3.Lerp(reaction.rectTransform.localScale, Vector3.one * 2.1f, Time.deltaTime);
                if (reaction.rectTransform.localScale.x >= 2f)
                {
                    pingPong = true;
                }
            }
            else
            {
                reaction.rectTransform.localScale = Vector3.Lerp(reaction.rectTransform.localScale, Vector3.one * 0.9f, Time.deltaTime);
                if (reaction.rectTransform.localScale.x <= 1f)
                {
                    pingPong = false;
                }
            }

            bool isValid = reaction.rectTransform.localScale.x <= 1.25f;
            reaction.color = isValid ? Color.green : Color.white;
            
            Vector3 input = InputManager.Instance.ReadInputAsScreenPosition();
            
            if (input != Vector3.zero && buffer > 1f)
            {
                didClick = true;
                FluidPotionContainer selectedPotion = GameManager.Instance.selectedPotion;
                selectedPotion.OpenAndDrip();
                
                GameManager.Instance.correctPotion = isValid;
                GUIManager.Instance.helpText.gameObject.SetActive(false);
                GUIManager.Instance.reaction.gameObject.SetActive(false);
                
                FluidShakerContainer shaker = GameManager.Instance.shaker;
                
                shaker.fluidMeshRenderer.material.SetTexture(MainTexture, selectedPotion.data.main);
                shaker.fluidMeshRenderer.material.SetTexture(NoiseOneTexture, selectedPotion.data.noiseOne);
                shaker.fluidMeshRenderer.material.SetTexture(NoiseTwoTexture, selectedPotion.data.noiseTwo);
                
                GameManager.Instance.ChangeStateAfterDelay(GameManager.Instance.EndState, 2.5f);
            }

            Debug.Log(isValid);
        }
    }
}