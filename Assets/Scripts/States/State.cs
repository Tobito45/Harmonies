using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.States
{
    public interface IState
    {
        public void Entry();
        public void Exit();
    }

    public abstract class BaseState : IState
    {
        protected StateMachine _stateMachine;
        protected TurnManager TurnManager => _stateMachine.TurnManager;
        protected BaseState(StateMachine stateMachine) =>
            _stateMachine = stateMachine;
        public abstract void Entry();
        public abstract void Exit();
    }


    public class BlocksPlaceSelectState : BaseState
    {
        public BlocksPlaceSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            Debug.Log(nameof(BlocksPlaceSelectState) + " entry");
        }

        public override void Exit()
        {
        }
    }

    public class AnimalsEnvironmentSelectState : BlocksPlaceSelectState
    {
        public AnimalsEnvironmentSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            Debug.Log(nameof(AnimalsEnvironmentSelectState) + " entry");
        }

        public override void Exit()
        {
        }
    }

    public class AnimalsSelectState : BlocksPlaceSelectState
    {
        public AnimalsSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            Debug.Log(nameof(AnimalsSelectState) + " entry");

            if (!TurnManager.IsAnyEnviroment())
                TurnManager.WasAnimalsSkiped();

        }

        public override void Exit()
        {
        }
    }

    public class BlockSelectState : BlocksPlaceSelectState
    {
        public BlockSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            Debug.Log(nameof(BlockSelectState) + " entry");
        }

        public override void Exit()
        {
        }
    }
}
