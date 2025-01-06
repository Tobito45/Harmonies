using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerTurnInfo : MonoBehaviour
{
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private TextMeshProUGUI _name;

    [SerializeField]
    private GameObject _panel;

    [SerializeField]
    private float _speedFloat = 0.05f, _timer = 0.01f, _timerBetween = 1f;
    
    private TurnManager _turnManager;
    private NetworkManagerUI _networkManagerUI;

    private Dictionary<GameObject, float> _basicAlpha = new();
    private Dictionary<GameObject, Image> _objectsImages = new();
    private Dictionary<GameObject, TextMeshProUGUI> _objectsTexts = new();


    [Inject]
    private void Construct(TurnManager turnManager, NetworkManagerUI networkManagerUI)
    {
        _turnManager = turnManager;
        _networkManagerUI = networkManagerUI;
    }

    private void Start()
    {
        _panel.SetActive(false);
        _turnManager.OnRoundEnded += OnRoundBegin;
        _turnManager.OnGameStarted += OnGameStart;
        foreach (Transform child in _panel.transform)
        {
            Image img = child.GetComponent<Image>();
            TextMeshProUGUI txt = child.GetComponent<TextMeshProUGUI>();
            if (img != null)
            {
                _basicAlpha.Add(child.gameObject, img.color.a);
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
                _objectsImages.Add(child.gameObject, img);
            }
            else
            {
                _basicAlpha.Add(child.gameObject, txt.color.a);
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
                _objectsTexts.Add(child.gameObject, txt);
            }
        }
    }

    private void OnGameStart(ulong next) => OnRoundBegin(0, next);

    private void OnRoundBegin(ulong prev, ulong next)
    {
        PlayerInfoElement playerInfo = _networkManagerUI.PlayerInfoById(next);
        _name.text = playerInfo.Name;
        _icon.sprite = playerInfo.Image.sprite;
        StartCoroutine(ShowPanel());
    }

    public IEnumerator ShowPanel()
    {
        _panel.SetActive(true);
        while (true)
        {
            bool somethingWas = false;
            foreach(var imgs in _objectsImages)
            {
                if (imgs.Value.color.a >= _basicAlpha[imgs.Key])
                    continue;

                imgs.Value.color = new Color(imgs.Value.color.r, imgs.Value.color.g, 
                    imgs.Value.color.g, imgs.Value.color.a + _speedFloat);
                somethingWas = true;
            }

            foreach (var txt in _objectsTexts)
            {
                if (txt.Value.color.a >= _basicAlpha[txt.Key])
                    continue;

                txt.Value.color = new Color(txt.Value.color.r, txt.Value.color.g,
                    txt.Value.color.g, txt.Value.color.a + _speedFloat);
                somethingWas = true;
            }

            if (!somethingWas)
                break;

            yield return new WaitForSeconds(_timer);
        }
        yield return new WaitForSeconds(_timerBetween);
        StartCoroutine(HidePanel());
    }

    public IEnumerator HidePanel()
    {
        while (true)
        {
            bool somethingWas = false;
            foreach (var imgs in _objectsImages)
            {
                if (imgs.Value.color.a <= 0)
                    continue;

                imgs.Value.color = new Color(imgs.Value.color.r, imgs.Value.color.g,
                    imgs.Value.color.g, imgs.Value.color.a - _speedFloat);
                somethingWas = true;
            }

            foreach (var txt in _objectsTexts)
            {
                if (txt.Value.color.a <= 0)
                    continue;

                txt.Value.color = new Color(txt.Value.color.r, txt.Value.color.g,
                    txt.Value.color.g, txt.Value.color.a - _speedFloat);
                somethingWas = true;
            }

            if (!somethingWas)
                break;

            yield return new WaitForSeconds(_timer);
        }
        _panel.SetActive(false);
    }
}
