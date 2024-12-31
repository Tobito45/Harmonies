using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // ��������� Dropdown ����� ���������
    private Resolution[] resolutions;

    //public AudioMixer audioMixer; // ��������� ��� AudioMixer ����� ���������
    public Slider volumeSlider;  // ��������� Slider ����� ���������


    private void Start()
    {
        // �������� ��������� ����������
        resolutions = Screen.resolutions;

        // ������� � ��������� Dropdown
        resolutionDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);

        // ������������� ������� ����������
        int currentResolutionIndex = System.Array.FindIndex(resolutions, r =>
            r.width == Screen.currentResolution.width &&
            r.height == Screen.currentResolution.height);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


        // ������������� ������� �� ������� �������� ���������
        float volume;
        //audioMixer.GetFloat("MasterVolume", out volume);
        //volumeSlider.value = Mathf.Pow(10, volume / 20); // ����������� �� ��������������� �����
    }

    public void SetResolution(int resolutionIndex)
    {
        // ������������� ��������� ����������
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }


    public void ToggleFullscreen()
    {
        // ����������� ����� ������� � ������������� ��������
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }

    public void SetVolume(float volume)
    {
        // ������������� ��������� (��������������� ����� ��� ��������)
        //audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
}
