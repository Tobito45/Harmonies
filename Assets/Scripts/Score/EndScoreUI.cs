using Harmonies.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScoreUI : MonoBehaviour
{
    [SerializeField]
    private ScoreUIPanel[] _scoreUIPanels;

    public void ShowPanel(List<PlayerInfoElement> infoElements, int rounds)
    {
        _scoreUIPanels[infoElements.Count - 1].gameObject.SetActive(true);
        _scoreUIPanels[infoElements.Count - 1].SetInfo(infoElements, rounds);
    }
}
