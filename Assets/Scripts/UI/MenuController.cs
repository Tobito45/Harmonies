using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject SettingsWindow;

    public void OpenSettings() => SettingsWindow.SetActive(true);

    public void CloseSettings() => SettingsWindow.SetActive(false);
}
