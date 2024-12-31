using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    private GameObject _settingsPanel;
    
    [Header("Buttons")]
    [SerializeField]
    private Button _settingsButton;

    private void Start()
    {
        _settingsButton.onClick.AddListener(() => _settingsPanel.SetActive(!_settingsPanel.activeInHierarchy));
    }
}
