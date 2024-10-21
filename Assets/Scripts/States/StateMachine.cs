using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.States {
    public class StateMachine
    {
        private IState _blocksSelectState;
        private IState _environmentSelectState;
        private IState _animalsSelectState;

        public TurnManager TurnManager { get; set; }

        public IState ActualState { get; private set; }

        public StateMachine()
        {
            _blocksSelectState = new BlocksSelectState(this);
            _animalsSelectState = new AnimalsSelectState(this);
            _environmentSelectState = new AnimalsEnvironmentSelectState(this);
        }

        public void SelectState(IState state)
        {
            if(ActualState != null)
               ActualState.Exit();

            ActualState = state;
            ActualState.Entry();
        }

        public void BlocksSelectState() => SelectState(_blocksSelectState);

        public void AnimalsEnvironmentSelectState() => SelectState(_environmentSelectState);
        public void AnimalsSelectState() => SelectState(_animalsSelectState);
    }
}
