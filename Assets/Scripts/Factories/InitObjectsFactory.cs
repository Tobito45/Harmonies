using Harmonies.Conditions;
using Harmonies.Selectors;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Harmonies.InitObjets
{
    public static class InitObjectsFactory
    {
        public static Dictionary<Type, Action<object>> InitObject = new();
        public static Dictionary<Type, Predicate<object>> InitPredicateObject = new();

        public static void Init(TurnManager turnManager,
            EnvironmentController environmentController,
            SpawnBlocksController spawnBlocksController,
            BoardSceneGenerator[] boardSceneGenerator)
        {
            List<InitObjectBase> initObjectBases = new()
            {
                new GameCellObject(boardSceneGenerator, turnManager),
                new GameAnimalObject(environmentController, turnManager),
                new BlockSelectorControllerObject(spawnBlocksController, turnManager),
                new AnimalSelectorControllerObject(turnManager)
            };

            InitAllDictionaries(initObjectBases);
        }
        private static void InitAllDictionaries(List<InitObjectBase> initObjectBases)
        {
            foreach (InitObjectBase item in initObjectBases)
            {
                if (item is IInitObject iinit)
                    InitObject.Add(item.MainType, iinit.Init);
                if (item is IPredicateObject ipred)
                    InitPredicateObject.Add(item.MainType, ipred.PredicateGameCell);
            }
        }

        public static IEnumerator WaitForCallbackWithPredicate(Type type, object obj, Action callback)
        {
            float timer = 0f;
            Predicate<object> predicate = InitPredicateObject[type];
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
    }
}