using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreUIPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] _scores;

    [SerializeField]
    private TextMeshProUGUI[] _names;

    [SerializeField]
    private TextMeshProUGUI _rounds;

    [SerializeField]
    private Button _okButton, _retryButton;

    private void Start()
    {
        _okButton.onClick.AddListener(NetworkTools.CloseAndGoToMain);
        _retryButton.onClick.AddListener(NetworkTools.CloseAndGoToMain);
    }

    public void SetInfo(List<PlayerInfoElement> infoElements,int rounds)
    {
        infoElements = infoElements.OrderByDescending(n => n.Score).ToList();

        for (int i = 0; i < infoElements.Count; i++)
        {
            _scores[i].text = $"SCORE: {infoElements[i].Score.ToString()}";
            _names[i].text= infoElements[i].Name.ToString();
            _names[i].color = infoElements[i].Color;
        }
        _rounds.text = $" {rounds.ToString()} rounds";
    }

}
