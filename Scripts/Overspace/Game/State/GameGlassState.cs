using Overspace.Camera;
using Overspace.Fluid;
using Overspace.GUI;

namespace Overspace.Game.State
{
    public class GameGlassState : GameBaseState
    {
        public override string StateName => "GLASS SELECT";
        
        public override void Enter()
        {
            CameraManager.Instance.SetCamera(CameraManager.Instance.glassCam);
            GameManager.Instance.selectedGlass = 0;
            FluidManager.Instance.ShowGlassDisplay(0);
            
            GUIManager.Instance.helpText.gameObject.SetActive(true);
            GUIManager.Instance.helpText.text = "Choose a glass";
            
            GUIManager.Instance.rightButton.onClick.RemoveAllListeners();
            GUIManager.Instance.rightButton.onClick.AddListener(() =>
            {
                if (GameManager.Instance.selectedGlass + 1 >= FluidManager.Instance.glassPrefabs.Count)
                {
                    GameManager.Instance.selectedGlass = 0;
                }
                else
                {
                    GameManager.Instance.selectedGlass++;
                }
                
                FluidManager.Instance.ShowGlassDisplay(GameManager.Instance.selectedGlass);
            });
            GUIManager.Instance.rightButton.gameObject.SetActive(true);
            
            GUIManager.Instance.leftButton.onClick.RemoveAllListeners();
            GUIManager.Instance.leftButton.onClick.AddListener(() =>
            {
                if (GameManager.Instance.selectedGlass - 1 < 0)
                {
                    GameManager.Instance.selectedGlass = FluidManager.Instance.glassPrefabs.Count - 1;
                }
                else
                {
                    GameManager.Instance.selectedGlass--;
                }
                
                FluidManager.Instance.ShowGlassDisplay(GameManager.Instance.selectedGlass);
            });
            GUIManager.Instance.leftButton.gameObject.SetActive(true);
            
            GUIManager.Instance.serveButton.onClick.RemoveAllListeners();
            GUIManager.Instance.serveButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ChangeState(GameManager.Instance.BottleState);
            });
            GUIManager.Instance.serveButton.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            GUIManager.Instance.rightButton.gameObject.SetActive(false);
            GUIManager.Instance.leftButton.gameObject.SetActive(false);
            GUIManager.Instance.serveButton.gameObject.SetActive(false);
            GUIManager.Instance.helpText.gameObject.SetActive(false);
            FluidManager.Instance.ShowGlassDisplay(-1);
        }

        public override void Tick()
        {
        }
    }
}
