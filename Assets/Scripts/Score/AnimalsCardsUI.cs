using Harmonies.Enviroment;
using Harmonies.ScroptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Harmonies.Score.AnimalCard
{
    public class AnimalsCardsUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _prefab, _prefabInfo;
        [SerializeField]
        private Transform _positionSpawn;
        [SerializeField]
        private GameObject _canvasParent;

        private List<GameObject> _cards = new();
        private EnviromentDataConfig _enviromentDataConfig;

        private void Awake() => _enviromentDataConfig = Resources.Load<EnviromentDataConfig>("EnviromentDataConfig");

        public void SummonNewAnimalCard(GameAnimal animal)
        {
            GameObject obj = Instantiate(_prefab, _positionSpawn.transform.position, Quaternion.identity, _canvasParent.transform);
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            RectTransform prefabRectTransform = _prefab.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(
                prefabRectTransform.rect.width - 10 * _cards.Count,
                prefabRectTransform.rect.height
            );

            obj.transform.localPosition = new Vector3(
                _positionSpawn.transform.localPosition.x + (prefabRectTransform.rect.width - rectTransform.rect.width) / 2,
                _positionSpawn.transform.localPosition.y + rectTransform.rect.height * _cards.Count,
                0
            );

            List<AnimalCardInfo> infos = new();
            foreach(int i in animal.Scores)
            {
                GameObject info = Instantiate(_prefabInfo, obj.transform);
                AnimalCardInfo cardInfo = info.GetComponent<AnimalCardInfo>();
                cardInfo.Image.sprite = _enviromentDataConfig.GetImageIcon((int)animal.Index);
                cardInfo.Score.text = i.ToString();
                infos.Add(cardInfo);    
            }

            animal.OnSeleted += (i) =>
            {
                if(i != 0)
                    infos[i - 1].Active(false);

                infos[i].Active(true);
            };

            _cards.Add(obj);
        }
    }
}
