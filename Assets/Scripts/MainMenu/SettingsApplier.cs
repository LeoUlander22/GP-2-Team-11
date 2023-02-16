using Cinemachine;
using UnityEngine;

namespace Team11.MainMenu
{
    public class SettingsApplier : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;
        
        private void Start()
        {
            _camera = FindObjectOfType<CinemachineVirtualCamera>();
            SettingsMenu.OnApplySettings += ApplySettings;
        }

        private void ApplySettings(SettingsData data)
        {
            SetScreenSettings(data);
            SetVolumes(data);
            if (_camera != null)
                _camera.m_Lens.FieldOfView = data.fov;
        }

        private void SetScreenSettings(SettingsData data)
        {
            if (!IsSameResolution(data.resolution))
                Screen.SetResolution(data.resolution.x, data.resolution.y, data.fullScreen);
            Application.targetFrameRate = GetFrameTarget(data.frameLimiter);
            QualitySettings.vSyncCount = data.vsync ? 1 : 0;
        }

        private static void SetVolumes(SettingsData data)
        {
            FMOD.Studio.Bus master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
            FMOD.Studio.Bus ambience = FMODUnity.RuntimeManager.GetBus("bus:/Master/Amb");
            FMOD.Studio.Bus sfx = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
            master.setVolume(data.masterVolume);
            ambience.setVolume(data.ambienceVolume);
            sfx.setVolume(data.soundEffectVolume);
        }

        private bool IsSameResolution(Vector2Int currentRes)
        {
            return Screen.currentResolution.width == currentRes.x && Screen.currentResolution.height == currentRes.y;
        }

        private int GetFrameTarget(FrameOptions frameOptions)
        {
            return frameOptions switch
            {
                FrameOptions.Thirty => 30,
                FrameOptions.Sixty => 60,
                FrameOptions.OneTwenty => 120,
                _ => 120
            };
        }
    }
}