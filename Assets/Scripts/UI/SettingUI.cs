using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Привяжите Dropdown через инспектор
    private Resolution[] resolutions;

    //public AudioMixer audioMixer; // Привяжите ваш AudioMixer через инспектор
    public Slider volumeSlider;  // Привяжите Slider через инспектор


    private void Start()
    {
        // Получаем доступные разрешения
        resolutions = Screen.resolutions;

        // Очищаем и наполняем Dropdown
        resolutionDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);

        // Устанавливаем текущее разрешение
        int currentResolutionIndex = System.Array.FindIndex(resolutions, r =>
            r.width == Screen.currentResolution.width &&
            r.height == Screen.currentResolution.height);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


        // Устанавливаем слайдер на текущее значение громкости
        float volume;
        //audioMixer.GetFloat("MasterVolume", out volume);
        //volumeSlider.value = Mathf.Pow(10, volume / 20); // Преобразуем из логарифмической шкалы
    }

    public void SetResolution(int resolutionIndex)
    {
        // Устанавливаем выбранное разрешение
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }


    public void ToggleFullscreen()
    {
        // Переключаем между оконным и полноэкранным режимами
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
        // Устанавливаем громкость (логарифмическая шкала для точности)
        //audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
}
