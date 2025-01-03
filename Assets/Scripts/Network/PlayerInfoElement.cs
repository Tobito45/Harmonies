using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoElement : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI TextScore { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI TextName { get; private set; }
    [field: SerializeField]
    public Image Image { get; private set; }

    public void UpdateInfo(ulong id, int score)
    {
        TextScore.text = score.ToString();
        TextName.text = $"Player {id}";
    }
}
