using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//cut on differentControllers
public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    private GameObject _settingsPanel;
    [SerializeField]
    private GameObject _exitPanel;
    [SerializeField]
    private GameObject _codePanel;

    [Header("Buttons")]
    [SerializeField]
    private Button _settingsButton;
    [SerializeField]
    private Button _settingsBackgroundButton;
    [SerializeField]
    private Button _exitButton;
    [SerializeField]
    private Button _exitBackgroundButton;
    [SerializeField]
    private Button _exitNoButton;
    [SerializeField]
    private Button _exitYesButton;
    [SerializeField]
    private Button _codeButton;
    [SerializeField]
    private Button _codeYesButton;
    [SerializeField]
    private Button _codeBackgroundButton;
    [SerializeField]
    private Button _hostButton;

    [Header("InputFields")]
    [SerializeField]
    private TMP_InputField _codeInputField;

    private void Start()
    {
        _settingsButton.onClick.AddListener(() => _settingsPanel.SetActive(!_settingsPanel.activeInHierarchy));
        _settingsBackgroundButton.onClick.AddListener(() => _settingsPanel.SetActive(!_settingsPanel.activeInHierarchy));
        _exitButton.onClick.AddListener(() => _exitPanel.SetActive(!_exitPanel.activeInHierarchy));
        _exitBackgroundButton.onClick.AddListener(() => _exitPanel.SetActive(!_exitPanel.activeInHierarchy));
        _exitYesButton.onClick.AddListener(() => Application.Quit());   
        _exitNoButton.onClick.AddListener(() => _exitPanel.SetActive(!_exitPanel.activeInHierarchy));
        _codeButton.onClick.AddListener(() => _codePanel.SetActive(!_codePanel.activeInHierarchy));
        _codeBackgroundButton.onClick.AddListener(() => _codePanel.SetActive(!_codePanel.activeInHierarchy));

        _codeYesButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
            DataConnecterController.Singlton.StartAsClient(_codeInputField.text);
        });
        _hostButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("SampleScene");
            DataConnecterController.Singlton.StartAsHost();
        });
    }
}
