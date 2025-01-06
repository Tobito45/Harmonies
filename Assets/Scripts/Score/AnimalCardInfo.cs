using Harmonies.Enviroment;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Harmonies.Score.AnimalCard
{
    public class AnimalCardInfo : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI Score { get; private set; }
        [field: SerializeField]
        public Image Image { get; private set; }
        [SerializeField]
        private GameObject _check;

        private void Awake() => Active(false);

        public void Active(bool enabled) => _check.SetActive(enabled);
    }
}
