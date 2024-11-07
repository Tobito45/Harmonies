using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Harmonies.Selectors
{
    public class GameBlock : MonoBehaviour
    {
        [field: SerializeField]
        public GameObject Prefab { get; private set; }

        [field: SerializeField]
        public int Index { get; private set; }
    }
}
