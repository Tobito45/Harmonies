using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WaitPanelUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _waitPanelClient, _waitPanelHost;

    [SerializeField]
    private Button _buttonBackClient, _buttonBackHost, _buttonPlayHost;

    [Inject]
    private void Construct(TurnManager turnManager)
    {
        _waitPanelHost.SetActive(DataConnecterController.Singlton.UserType == Harmonies.Enums.UserType.Host);
        _waitPanelClient.SetActive(DataConnecterController.Singlton.UserType == Harmonies.Enums.UserType.Client);

        _buttonBackClient.onClick.AddListener(NetworkTools.CloseAndGoToMain);
        _buttonBackHost.onClick.AddListener(NetworkTools.CloseAndGoToMain);
        _buttonPlayHost.onClick.AddListener(() =>
        {
            _waitPanelClient.SetActive(false);
            _waitPanelHost.SetActive(false);
            StartCoroutine(turnManager.StartGame());
            _buttonPlayHost.interactable = false;
        });

        turnManager.OnGameStarted += (id) =>
        {
            _waitPanelClient.SetActive(false);
            _waitPanelHost.SetActive(false);
        };
    }

}
