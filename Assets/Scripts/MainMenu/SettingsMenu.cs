using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Team11.MainMenu
{
    [RequireComponent(typeof(SettingsApplier))]
    public class SettingsMenu : MonoBehaviour
    {
        public static event Action<SettingsData> OnApplySettings; 

        [SerializeField] private Slider masterVolSlider;
        [SerializeField] private Slider soundEffectVolSlider;
        [SerializeField] private Slider ambienceVolSlider;
        [SerializeField] private Slider fovSlider;
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private Slider brightnessSlider;
        [SerializeField] private TMP_Dropdown frameLimitDropdown;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vsyncToggle;
        [SerializeField] private Button applyButton;
        
        private string _savePath;
        private SettingsData _finalSettingsData;
        private SettingsData _editSettingsData;
        
        private Resolution[] _resolutions;

        private void Start()
        {
            _savePath = Application.persistentDataPath + "/gamedata.json";
            _resolutions = Screen.resolutions;
            resolutionDropdown.options = new List<TMP_Dropdown.OptionData>();
            foreach (var resolution in _resolutions)
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(GetVector(resolution).ToString()));
            resolutionDropdown.value = resolutionDropdown.options.Count - 1;
            Load();
            SetListeners();
            applyButton.onClick.AddListener(Save);
        }

        private void Save()
        {
            _finalSettingsData = new SettingsData(_editSettingsData);
            string data = JsonUtility.ToJson(_finalSettingsData, true);
            File.WriteAllText(_savePath, data);
            OnApplySettings?.Invoke(_finalSettingsData);
        }

        private void Load()
        {
            if(File.Exists(_savePath))
            {
                string data = File.ReadAllText(_savePath);
                _finalSettingsData = JsonUtility.FromJson<SettingsData>(data);
                _editSettingsData = _finalSettingsData;
                SetValues();
                OnApplySettings?.Invoke(_finalSettingsData);
                return;
            }

            _finalSettingsData = new SettingsData();
            _editSettingsData = new SettingsData(_finalSettingsData);
            Save();
            SetValues();
        }

        private void SetListeners()
        {
            masterVolSlider.onValueChanged.AddListener(vol => _editSettingsData.SetValue(nameof(_editSettingsData.MasterVolume), vol));
            soundEffectVolSlider.onValueChanged.AddListener(vol => _editSettingsData.SetValue(nameof(_editSettingsData.SoundEffectVolume), vol));
            ambienceVolSlider.onValueChanged.AddListener(vol => _editSettingsData.SetValue(nameof(_editSettingsData.AmbienceVolume), vol));
            fovSlider.onValueChanged.AddListener(vol => _editSettingsData.SetValue(nameof(_editSettingsData.FOV), vol));
            sensitivitySlider.onValueChanged.AddListener(vol => _editSettingsData.SetValue(nameof(_editSettingsData.Sensitivity), vol));
            brightnessSlider.onValueChanged.AddListener(vol => _editSettingsData.SetValue(nameof(_editSettingsData.Brightness), vol));
            frameLimitDropdown.onValueChanged.AddListener(index => _editSettingsData.SetValue(nameof(_editSettingsData.FrameLimiter), (FrameOptions) index));
            resolutionDropdown.onValueChanged.AddListener(index => _editSettingsData.SetValue(nameof(_editSettingsData.Resolution1), GetVector(_resolutions[index])));
            fullscreenToggle.onValueChanged.AddListener(active => _editSettingsData.SetValue(nameof(_editSettingsData.FullScreen), active));
            vsyncToggle.onValueChanged.AddListener(active => _editSettingsData.SetValue(nameof(_editSettingsData.Vsync), active));
        }

        private void SetValues()
        { 
            masterVolSlider.value = _finalSettingsData.masterVolume;
            soundEffectVolSlider.value = _finalSettingsData.soundEffectVolume;
            ambienceVolSlider.value = _finalSettingsData.ambienceVolume;
            fovSlider.value = _finalSettingsData.fov;
            sensitivitySlider.value = _finalSettingsData.sensitivity;
            brightnessSlider.value = _finalSettingsData.brightness;
            frameLimitDropdown.value = (int)_finalSettingsData.frameLimiter;
            resolutionDropdown.value = GetIndex(_finalSettingsData.resolution);
            fullscreenToggle.isOn = _finalSettingsData.fullScreen;
            vsyncToggle.isOn = _finalSettingsData.vsync;
        }

        private Vector2Int GetVector(Resolution resolution)
        {
            return new Vector2Int(resolution.width, resolution.height);
        }

        private int GetIndex(Vector2Int resolution)
        {
            for (var i = 0; i < _resolutions.Length; i++)
            {
                var res = _resolutions[i];
                if (res.width == resolution.x && res.height == resolution.y)
                {
                    return i;
                }
            }

            return resolutionDropdown.options.Count - 1;
        }
    }
}