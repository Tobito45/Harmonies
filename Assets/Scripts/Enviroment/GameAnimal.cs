using Harmonies.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace Harmonies.Enviroment
{
    //in future to scriptbleObjects
    public class GameAnimal : MonoBehaviour
    {
        [field: SerializeField]
        public AnimalType Index { get; private set; }

        /// <summary>
        /// From lower to high. Must have same elements as count animals
        /// </summary>
        [SerializeField]
        private List<int> scores = new List<int>();
        
        public int GetNewScore(int index)
        {
            if (index == 0)
                return scores[index];
            else
                return scores[index] - scores[index - 1]; 
        }
        
    }
}
