using Overspace.Camera;
using Overspace.Fluid;
using Overspace.Fluid.Container;
using Overspace.GUI;
using Overspace.Input;
using UnityEngine;

namespace Overspace.Game.State
{
    public class GamePotionState : GameBaseState
    {
        public override string StateName => "POTION SELECT";

        public override void Enter()
        {
            CameraManager.Instance.SetCamera(CameraManager.Instance.potionCam);
            GUIManager.Instance.helpText.text = "Tap on a potion to select";
            GUIManager.Instance.helpText.gameObject.SetActive(true);
            FluidManager.Instance.CreatePotions();
        }

        public override void Exit()
        {
            GUIManager.Instance.helpText.gameObject.SetActive(false);
            FluidManager.Instance.DestroyPotions();
        }

        public override void Tick()
        {
            if (GameManager.Instance.selectedPotion == null)
            {
                Vector3 input = InputManager.Instance.ReadInputAsScreenPosition();
                if (input != Vector3.zero)
                {
                    Ray ray = UnityEngine.Camera.main.ScreenPointToRay(input);
        
                    if (Physics.Raycast(ray, out RaycastHit hit)) {
                        Transform objectHit = hit.transform;
                        if (objectHit != null)
                        {
                            if (objectHit.TryGetComponent(out FluidPotionContainer bottle))
                            {
                                GameManager.Instance.selectedPotion = bottle;
                                GameManager.Instance.selectedPotion.transform.parent = null;
                                Debug.Log(bottle.name);
                                GameManager.Instance.ChangeState(GameManager.Instance.PotionAddState);
                            }
                        }
                    }
                }
            }
        }
    }
}