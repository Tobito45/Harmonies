using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggleWindow;

    [SerializeField]
    private TMP_Dropdown _resolutionDropdown;
    private Resolution[] _resolutions;

    [SerializeField]
    private Slider _volumeSlider; 

    private AudioSource _audioSource;

    private void Start()
    {
        ResolutionSetup();
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

        _audioSource = FindObjectOfType<AudioSourceDontDestroy>().GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("Sound"))
            _audioSource.volume = PlayerPrefs.GetFloat("Sound");
        else
        {
            PlayerPrefs.SetFloat("Sound", _audioSource.volume);
            PlayerPrefs.Save();
        }

        _volumeSlider.value = _audioSource.volume;
        _volumeSlider.onValueChanged.AddListener(SetVolume);

        _toggleWindow.onValueChanged.AddListener((x) => ToggleFullscreen());

        SetResolution(_resolutions.Max(n => n.height));
    }

    private void ResolutionSetup()
    {
        _resolutions = Screen.resolutions;

        _resolutionDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
        }

        _resolutionDropdown.AddOptions(options);

        int currentResolutionIndex = System.Array.FindIndex(_resolutions, r =>
            r.width == Screen.currentResolution.width &&
            r.height == Screen.currentResolution.height);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

        _resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }


    private void ToggleFullscreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    private void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Sound", _audioSource.volume);
        PlayerPrefs.Save();
        _audioSource.volume = volume;
    }
}
