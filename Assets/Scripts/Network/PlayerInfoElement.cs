using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoElement
{
    public PlayerInfoElement(TextMeshProUGUI textScore, string name, Image image, Color color, bool isMain)
    {
        TextScore = textScore;
        Name = name;
        Image = image;
        Color = color;
        IsMain = isMain;

        textScore.color = color;
    }

    public bool IsMain { get; private set; }
    public TextMeshProUGUI TextScore { get; private set; }
    public string Name { get; private set; }
    public Color Color { get; private set; }
    public Image Image { get; private set; }
    public int Score { get; private set; }
    public void UpdateInfo(ulong id, int score)
    {
        Score = score;
        if(IsMain)
            TextScore.text = $" : {score.ToString()}";
        else
            TextScore.text = $"{score.ToString()} : ";
    }
}
