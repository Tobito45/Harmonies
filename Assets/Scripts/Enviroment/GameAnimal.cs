using Harmonies.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace Harmonies.Enviroment
{
    public class GameAnimal : MonoBehaviour
    {
        [field: SerializeField]
        public AnimalType Index { get; private set; }
    }
}
