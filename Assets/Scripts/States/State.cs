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
    public class StartRoundState : BaseState
    {
        public StartRoundState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            Debug.Log(nameof(StartRoundState) + " entry");
            TurnManager.SpawnSelectEnvironmentToPlayerZone();
            _stateMachine.BlocksSelectState();
        }

        public override void Exit() { }
    }

    public class BlocksPlaceSelectState : BaseState
    {
        public BlocksPlaceSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry() => Debug.Log(nameof(BlocksPlaceSelectState) + " entry");

        public override void Exit() { }
    }

    public class AnimalsEnvironmentSelectState : BlocksPlaceSelectState
    {
        public AnimalsEnvironmentSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry() => Debug.Log(nameof(AnimalsEnvironmentSelectState) + " entry");

        public override void Exit() { }
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

        public override void Exit() { }
    }

    public class BlockSelectState : BlocksPlaceSelectState
    {
        public BlockSelectState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry() => Debug.Log(nameof(BlockSelectState) + " entry");

        public override void Exit() { }
    }

    public class EndRoundState : BlocksPlaceSelectState
    {
        public EndRoundState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry()
        {
            Debug.Log(nameof(EndGameState) + " entry");
            if (TurnManager.IsPlayerEnded())
                TurnManager.PlayerEndsPlay();
            else
                TurnManager.SelectNextPlayer();
        }

        public override void Exit() { }
    }

    public class EndGameState : BlocksPlaceSelectState
    {
        public EndGameState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Entry() => Debug.Log(nameof(EndGameState) + " entry");

        public override void Exit() { }
    }
}
