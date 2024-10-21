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
        protected BaseState(StateMachine stateMachine) =>
            _stateMachine = stateMachine;
        public abstract void Entry();
        public abstract void Exit();
    }


    public class BlocksSelectState : BaseState
    {
        public BlocksSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            _stateMachine.TurnManager.SpawnBlocks();
        }

        public override void Exit()
        {
            Debug.Log(nameof(BlocksSelectState) + " exit");
        }
    }

    public class AnimalsEnvironmentSelectState : BlocksSelectState
    {
        public AnimalsEnvironmentSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            _stateMachine.TurnManager.SpawnEnvironmentToPlayerZone();
        }

        public override void Exit()
        {
            Debug.Log(nameof(AnimalsEnvironmentSelectState) + " exit");
        }
    }

    public class AnimalsSelectState : BlocksSelectState
    {
        public AnimalsSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            Debug.Log(nameof(AnimalsSelectState) + " entry");
        }

        public override void Exit()
        {
            Debug.Log(nameof(AnimalsSelectState) + " exit");
        }
    }
}
