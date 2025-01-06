using Harmonies.Enviroment;
using Harmonies.Score;
using Harmonies.Selectors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Harmonies.InitObjets
{
    public static class InitObjectsFactory
    {
        public static Dictionary<Type, Action<object>> InitObjects = new();
        public static Dictionary<Type, Predicate<object>> InitPredicateObjects = new();
        public static void Init(TurnManager turnManager,
            EnvironmentController environmentController,
            SpawnBlocksController spawnBlocksController,
            BoardSceneGenerator[] boardSceneGenerator,
            ScoreController scoreController)
        {
            List<InitObjectBase> initObjectBases = new()
            {
                new GameCellObject(boardSceneGenerator, turnManager, spawnBlocksController, environmentController),
                new GameAnimalObject(environmentController, turnManager),
                new BlockSelectorControllerObject(spawnBlocksController, turnManager, scoreController),
                new AnimalSelectorControllerObject(turnManager, scoreController),
                new EnvironmentSelectObject(environmentController, turnManager)
            };

            InitAllDictionaries(initObjectBases);
        }
        private static void InitAllDictionaries(List<InitObjectBase> initObjectBases)
        {
            foreach (InitObjectBase item in initObjectBases)
            {
                if (item is IInitObject iinit)
                    InitObjects.Add(item.MainType, iinit.Init);
                if (item is IPredicateObject ipred)
                    InitPredicateObjects.Add(item.MainType, ipred.PredicateGameCell);
            }
        }

        public static IEnumerator WaitForCallbackWithPredicate(Type type, object obj, Action callback)
        {
            float timer = 0f;
            Predicate<object> predicate = InitPredicateObjects[type];
            while (!predicate.Invoke(obj))
            {
                timer += 0.1f;
                yield return null;
                if (timer > 5000)
                    break;
            }

            if (timer > 5000)
                throw new Exception("Problem in predicate");

            callback.Invoke();
        }

        public static void ClearDictionaries()
        {
            InitObjects.Clear();
            InitPredicateObjects.Clear();
        }
    }
}