using System;
using Cinemachine;
using Overspace.Pattern.Singleton;

namespace Overspace.Camera
{
    public class CameraManager : MonoBehaviourSingleton<CameraManager>
    {
        public CinemachineVirtualCamera tableCam;
        public CinemachineVirtualCamera glassCam;
        public CinemachineVirtualCamera bottleCam;
        public CinemachineVirtualCamera potionCam;
        public CinemachineVirtualCamera pourCam;
        public CinemachineVirtualCamera pourPotionCam;

        public CinemachineVirtualCamera selectedCam;

        private void Start()
        {
            SetCamera(tableCam);
        }

        public void SetCamera(CinemachineVirtualCamera cam)
        {
            tableCam.gameObject.SetActive(false);
            glassCam.gameObject.SetActive(false);
            bottleCam.gameObject.SetActive(false);
            pourCam.gameObject.SetActive(false);
            potionCam.gameObject.SetActive(false);
            pourPotionCam.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);

            selectedCam = cam;
        }
    }
}