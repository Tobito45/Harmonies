using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.States {
    public class StateMachine
    {
        private IState _startRoundState;
        private IState _blocksPlaceState;
        private IState _environmentSelectState;
        private IState _animalsSelectState;
        private IState _blockSelectState;
        private IState _endRoundState;
        private IState _endGameState;
        public Action<IState> OnStateChange;

        public TurnManager TurnManager { get; set; }

        public IState ActualState { get; private set; }

        public StateMachine(EndScoreUI endScore, NetworkManagerUI networkManager)
        {
            _blocksPlaceState = new BlocksPlaceSelectState(this);
            _animalsSelectState = new AnimalsSelectState(this);
            _environmentSelectState = new AnimalsEnvironmentSelectState(this);
            _blockSelectState = new BlockSelectState(this);
            _endRoundState = new EndRoundState(this);
            _endGameState = new EndGameState(this, networkManager, endScore);
            _startRoundState = new StartRoundState(this);
        }

        public void SelectState(IState state)
        {
            if(ActualState != null)
               ActualState.Exit();

            ActualState = state;
            if (ActualState != null)
                ActualState.Entry();

            OnStateChange(ActualState);
        }

        public void StartRoundState() => SelectState(_startRoundState);
        public void BlocksPlaceState() => SelectState(_blocksPlaceState);
        public void AnimalsEnvironmentSelectState() => SelectState(_environmentSelectState);
        public void AnimalsSelectState() => SelectState(_animalsSelectState);
        public void BlocksSelectState() => SelectState(_blockSelectState);
        public void EndGameState() => SelectState(_endGameState);
        public void EndRoundState() => SelectState(_endRoundState);
    }
}
