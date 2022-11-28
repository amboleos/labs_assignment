using System;
using Cinemachine;
using Overspace.Camera;
using Overspace.Game;
using Overspace.Game.State;
using Overspace.Pattern.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Overspace.DebugMenu
{
    public class DebugMenuManager : MonoBehaviourSingleton<DebugMenuManager>
    {
        public Button debugButton;
        public TextMeshProUGUI debugText;
        public RectTransform debugPanel;

        [Header("General")]
        public Button showDebugTextButton;
        public Button hideDebugTextButton;
        
        public Button thirtyFpsButton;
        public Button sixtyFpsButton;
        public Button noLimitFpsButton;

        public Button experimentalOnButton;
        public Button experimentalOffButton;

        [Header("Camera")]
        public Button tableCamButton;
        public Button glassCamButton;
        public Button bottleCamButton;
        public Button pourCamButton;
        
        public Button bodyUpButton;
        public Button bodyDownButton;
        public Button bodyRightButton;
        public Button bodyLeftButton;        
        
        public Button aimUpButton;
        public Button aimDownButton;
        public Button aimRightButton;
        public Button aimLeftButton;
        
        public Button fovIncreaseButton;
        public Button fovDecreaseButton;

        [Header("Shader")] 
        public Button shaderGenButton;
        
        [Header("Tabs")]
        public Button[] tabButtons;
        public RectTransform[] tabPanels;
        
        private bool showDebugText = false;
        private bool showDebugPanel = false;

        private double deltaTime = 0.0;
        private double fps = 0.0;
        private string debugTextStr = "";

        private void Start()
        {
            debugButton.onClick.AddListener(() => showDebugPanel = !showDebugPanel);
            
            thirtyFpsButton.onClick.AddListener(() => Application.targetFrameRate = 30);
            sixtyFpsButton.onClick.AddListener(() => Application.targetFrameRate = 60);
            noLimitFpsButton.onClick.AddListener(() => Application.targetFrameRate = -1);
            
            experimentalOffButton.image.color = Color.green;
            
            experimentalOnButton.onClick.AddListener(() =>
            {
                GameManager.Instance.enablePotionSteps = true;
                experimentalOnButton.image.color = Color.green;
                experimentalOffButton.image.color = Color.black;
            });            
            
            experimentalOffButton.onClick.AddListener(() =>
            {
                GameManager.Instance.enablePotionSteps = false;
                experimentalOffButton.image.color = Color.green;
                experimentalOnButton.image.color = Color.black;
            });
            
            showDebugTextButton.onClick.AddListener(() => showDebugText = true);
            hideDebugTextButton.onClick.AddListener(() => showDebugText = false);

            tableCamButton.onClick.AddListener(() => CameraManager.Instance.SetCamera(CameraManager.Instance.tableCam));
            glassCamButton.onClick.AddListener(() => CameraManager.Instance.SetCamera(CameraManager.Instance.glassCam));
            bottleCamButton.onClick.AddListener(() => CameraManager.Instance.SetCamera(CameraManager.Instance.bottleCam));
            pourCamButton.onClick.AddListener(() => CameraManager.Instance.SetCamera(CameraManager.Instance.pourCam));
            
            shaderGenButton.onClick.AddListener(() =>
            {
                GameObject go = new GameObject("Sacrificial Lamb");
                DontDestroyOnLoad(go);
                foreach(GameObject root in go.scene.GetRootGameObjects()) Destroy(root);
                SceneManager.LoadScene("ShaderTesting");
            });
            
            bodyUpButton.onClick.AddListener(() =>
            {
                Vector3 currentFollowOffset = CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
                currentFollowOffset.y += .1f;
                CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentFollowOffset;
            });
            
            bodyDownButton.onClick.AddListener(() =>
            {
                Vector3 currentFollowOffset = CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
                currentFollowOffset.y -= .1f;
                CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentFollowOffset;
            });
            
            bodyLeftButton.onClick.AddListener(() =>
            {
                Vector3 currentFollowOffset = CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
                currentFollowOffset.x -= .1f;
                CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentFollowOffset;
            });
            
            bodyRightButton.onClick.AddListener(() =>
            {
                Vector3 currentFollowOffset = CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
                currentFollowOffset.x += .1f;
                CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentFollowOffset;
            });
            
            aimUpButton.onClick.AddListener(() =>
            {
                Vector3 currentTrackOffset = CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                currentTrackOffset.y += .05f;
                CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = currentTrackOffset;
            });
            
            aimDownButton.onClick.AddListener(() =>
            {
                Vector3 currentTrackOffset = CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                currentTrackOffset.y -= .05f;
                CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = currentTrackOffset;
            });
            
            aimRightButton.onClick.AddListener(() =>
            {
                Vector3 currentTrackOffset = CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                currentTrackOffset.x += .05f;
                CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = currentTrackOffset;
            });
            
            aimLeftButton.onClick.AddListener(() =>
            {
                Vector3 currentTrackOffset = CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
                currentTrackOffset.x -= .05f;
                CameraManager.Instance.selectedCam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = currentTrackOffset;
            });
            
            fovIncreaseButton.onClick.AddListener(() =>
            {
                CameraManager.Instance.selectedCam.m_Lens.FieldOfView += 1f;
            });
            
            fovDecreaseButton.onClick.AddListener(() =>
            {
                CameraManager.Instance.selectedCam.m_Lens.FieldOfView -= 1f;
            });

            for (int i = 0; i < tabButtons.Length; i++)
            {
                int index = i;
                tabButtons[i].onClick.AddListener(() => ShowPanel(index));
            }
            
            ShowPanel(0);
        }

        public void ShowPanel(int index)
        {
            for (int i = 0; i < tabPanels.Length; i++)
            {
                if(i == index) tabPanels[i].gameObject.SetActive(true);
                else tabPanels[i].gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            CalculateFPS();
            HandleDebugText();
            HandleDebugPanel();
        }

        private void CalculateFPS()
        {
            deltaTime += Time.deltaTime;
            deltaTime /= 2.0;
            fps = 1.0/deltaTime;
        }
        
        private void HandleDebugText()
        {
            if(debugText.gameObject.activeSelf != showDebugText) debugText.gameObject.SetActive(showDebugText);
            
            if (showDebugText)
            {
                debugTextStr = "";
                debugTextStr += $"FPS: {(int)fps}\n";
                debugTextStr += $"STAGE: {GameManager.Instance.CurrentState.StateName}\n";
                debugTextStr += $"CAM: {CameraManager.Instance.selectedCam.gameObject.name}";
                debugText.text = debugTextStr;
            }
        }

        private void HandleDebugPanel()
        {
            if(debugPanel.gameObject.activeSelf != showDebugPanel) debugPanel.gameObject.SetActive(showDebugPanel);
        }
    }
}