using Overspace.Camera;
using Overspace.Fluid.Container;
using Overspace.GUI;
using Overspace.Input;
using UnityEngine;

namespace Overspace.Game.State
{
    public class GameBottleState : GameBaseState
    {
        public override string StateName => "BOTTLE SELECT";

        private float buffer = 0f;
        
        public override void Enter()
        {
            buffer = 0f;
            CameraManager.Instance.SetCamera(CameraManager.Instance.bottleCam);
            GUIManager.Instance.helpText.text = "Tap on a bottle to select";
            GUIManager.Instance.helpText.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            GUIManager.Instance.helpText.gameObject.SetActive(false);
        }

        public override void Tick()
        {
            buffer += Time.deltaTime;
            if(buffer <= 1f) return;
            
            if (GameManager.Instance.selectedBottle == null)
            {
                Vector3 input = InputManager.Instance.ReadInputAsScreenPosition();
                if (input != Vector3.zero)
                {
                    Ray ray = UnityEngine.Camera.main.ScreenPointToRay(input);
        
                    if (Physics.Raycast(ray, out RaycastHit hit)) {
                        Transform objectHit = hit.transform;
                        if (objectHit != null)
                        {
                            if (objectHit.TryGetComponent(out FluidBottleContainer bottle))
                            {
                                bottle.SetCapState(false);
                                GameManager.Instance.selectedBottle = bottle;
                                GameManager.Instance.ChangeState(GameManager.Instance.PourState);
                            }
                        }
                    }
                }
            }
        }
    }
}