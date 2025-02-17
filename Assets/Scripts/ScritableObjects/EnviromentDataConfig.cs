using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace Harmonies.ScroptableObjects
{
    [CreateAssetMenu(fileName = "NewEnviromentDataConfig", menuName = "Configs/Enviroment Data Config")]
    public class EnviromentDataConfig : ScriptableObject
    {
        [SerializeField]
        private GameObject[] _prefabEnviroments;
        [SerializeField]
        private GameObject[] _prefabAnimalsSpawn;
        [SerializeField]
        private GameObject[] _prefabEnviromentsSelectSpawn;
        [SerializeField]
        private GameObject[] _prefabConditions;
        [SerializeField]
        private Sprite[] _animalImages;

        
        public int GetRandomIndexSelectSpawnEnviroment => Random.Range(0, _prefabEnviromentsSelectSpawn.Length);

        public GameObject GetAnimal(int index) => _prefabAnimalsSpawn[index];
        public GameObject GetEnviromentsSelectSpawn(int index) => _prefabEnviromentsSelectSpawn[index];
        public GameObject GetEnviroments(int index) => _prefabEnviroments[index];
        public GameObject GetPrefabCondition(int index) => _prefabConditions[index];
        public Sprite GetImageIcon(int index) => _animalImages[index];
    }
}