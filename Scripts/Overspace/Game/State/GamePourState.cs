using Overspace.Camera;
using Overspace.Fluid;
using Overspace.Fluid.Container;
using Overspace.GUI;
using Overspace.Input;
using UnityEngine;

namespace Overspace.Game.State
{
    public class GamePourState : GameBaseState
    {
        public override string StateName => "POUR";

        public override void Enter()
        {
            CameraManager.Instance.SetCamera(CameraManager.Instance.pourCam);

            GUIManager.Instance.helpText.text = "Tap and hold to pour";
            GUIManager.Instance.helpText.gameObject.SetActive(true);

            GUIManager.Instance.serveButton.onClick.RemoveAllListeners();
            GUIManager.Instance.serveButton.onClick.AddListener(() =>
            {
                GameManager.Instance.shouldServe = true;
                GUIManager.Instance.serveButton.gameObject.SetActive(false);
            });

            GUIManager.Instance.backButton.gameObject.SetActive(true);
            GUIManager.Instance.backButton.onClick.RemoveAllListeners();
            GUIManager.Instance.backButton.onClick.AddListener(() =>
            {
                GameManager.Instance.selectedBottle.ReturnToHolder();
                GameManager.Instance.selectedBottle = null;
                GameManager.Instance.ChangeState(GameManager.Instance.BottleState);
                GUIManager.Instance.backButton.gameObject.SetActive(false);
            });

            if (GameManager.Instance.selectedBottle == null)
            {
                GameManager.Instance.ChangeState(GameManager.Instance.BottleState);
            }
            else
            {
                if (GameManager.Instance.shaker == null)
                {
                    GameManager.Instance.shaker = FluidManager.Instance.CreateGlass(GameManager.Instance.selectedGlass);
                    GameManager.Instance.shaker.transform.SetPositionAndRotation(FluidManager.Instance.pourPoint.position, FluidManager.Instance.pourPoint.rotation);
                }

                GameManager.Instance.selectedBottle.transform.SetPositionAndRotation(FluidManager.Instance.bottlePoint.position, FluidManager.Instance.bottlePoint.rotation);
            }
        }

        public override void Exit()
        {
            GUIManager.Instance.backButton.gameObject.SetActive(false);
            GUIManager.Instance.backButton.onClick.RemoveAllListeners();
            GUIManager.Instance.helpText.gameObject.SetActive(false);
            GUIManager.Instance.serveButton.gameObject.SetActive(false);
        }

        public override void Tick()
        {
            if (GameManager.Instance.shaker != null)
            {
                GameManager.Instance.isOrderReady = GameManager.Instance.shaker.CompareToOrder(GameManager.Instance.currentOrder);

                GUIManager.Instance.serveButton.gameObject.SetActive(GameManager.Instance.isOrderReady);

                int index = 0;
                foreach (FluidData order in GameManager.Instance.currentOrder)
                {
                    FluidShakerContainer shaker = GameManager.Instance.shaker;

                    bool isDone = shaker.Data[order] >= 0.175f;

                    if (GUIManager.Instance.orderView.CheckOrderIconState(index) != isDone)
                    {
                        GUIManager.Instance.orderView.SetCheck(index, isDone);

                        if (isDone)
                        {
                            if (GameManager.Instance.isOrderReady)
                            {
                                GameManager.Instance.shouldServe = true;
                                return;
                            }

                            GameManager.Instance.selectedBottle.ReturnToHolder();
                            GameManager.Instance.selectedBottle = null;
                            
                            GameManager.Instance.ChangeState(GameManager.Instance.BottleState);
                            GUIManager.Instance.backButton.gameObject.SetActive(false);
                            return;
                        }
                    }

                    index++;
                }

                if (GameManager.Instance.shaker.IsFull || GameManager.Instance.shouldServe)
                {
                    GameManager.Instance.selectedBottle.ReturnToHolder();
                    GameManager.Instance.selectedBottle = null;

                    if (! GameManager.Instance.enablePotionSteps)
                    {
                        GameManager.Instance.correctPotion = true;
                        GameManager.Instance.ChangeState(GameManager.Instance.EndState);
                    }
                    else
                    {
                        GameManager.Instance.ChangeState(GameManager.Instance.PotionState);
                    }
                    
                    return;
                }
            }

            Vector3 input = InputManager.Instance.ReadInputAsScreenPosition();
            FluidBottleContainer bottle = GameManager.Instance.selectedBottle;

            if (input != Vector3.zero)
            {
                bottle.transform.localRotation = Quaternion.Lerp(bottle.transform.localRotation, Quaternion.Euler(0f, 180f, -105f), Time.deltaTime * 2.5f);
            }
            else if (input == Vector3.zero)
            {
                bottle.transform.localRotation = Quaternion.Lerp(bottle.transform.localRotation, Quaternion.Euler(0f, 180f, -25f), Time.deltaTime * 5f);
            }
        }
    }
}