using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsCardsUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Transform _positionSpawn;
    [SerializeField]
    private GameObject _canvasParent;

    private List<GameObject> _cards = new();
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
            SummonNewAnimalCard();

    }

    public void SummonNewAnimalCard()
    {
        GameObject obj = Instantiate(_prefab, _positionSpawn.transform.position, Quaternion.identity, _canvasParent.transform);
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        RectTransform prefabRectTransform = _prefab.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(
            prefabRectTransform.rect.width - 10 *_cards.Count, 
            prefabRectTransform.rect.height
        );

        obj.transform.localPosition = new Vector3(
            _positionSpawn.transform.localPosition.x + (prefabRectTransform.rect.width - rectTransform.rect.width) / 2,
            _positionSpawn.transform.localPosition.y + rectTransform.rect.height * _cards.Count,
            0
        );

        // Добавляем в список карт
        _cards.Add(obj);

        //GameObject obj = Instantiate(_prefab, _positionSpawn.transform.position, Quaternion.identity, _canvasParent.transform);
        //obj.transform.localScale = new Vector2(_prefab.transform.localScale.x - 0.1f * _cards.Count,
        //        _prefab.transform.localScale.y);
        //obj.transform.position += new Vector3(0.1f * _cards.Count, _prefab.GetComponent<RectTransform>().height * _cards.Count, 0);
        //_cards.Add(obj);
    }
}
