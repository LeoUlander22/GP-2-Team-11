using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.StateMachine
{
    [Serializable]
    public class StateMachine : MonoBehaviour
    {
        public State[] states;
        [Space] public int defaultStateIndex;
        [Space] public UnityEvent onStateChange;

        private State _currentState;
        
        public State CurrentState
        {
            get => _currentState;
            private set => _currentState = value;
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            CurrentState = GetDefaultState();
            CurrentState.OnEnterState(this);
        }

        public void ChangeStateBasedOnDecision(bool conditionMet, string stateIfTrue, string stateIfFalse)
        {
            var st = TryGetState(conditionMet ? stateIfTrue : stateIfFalse, out bool result);
            if(!result) return;
            
            EnterState(st);
        }

        private void TryEnterState(State state)
        {
            var st = TryGetState(state, out bool result);
            if(!result) return;
            
            EnterState(st);
        }
        
        private void EnterState(State state)
        {
            ExitState(CurrentState);

            CurrentState = state;
            
            CurrentState.OnEnterState(this);
            
            onStateChange?.Invoke();
        }
        
        private void ExitState(State state)
        {
            state.OneExitState(this);
        }

        public State TryGetState(string stateName, out bool result)
        {
            var state = states.FirstOrDefault(s => s.state == stateName);
            result = state != null;
            return state;
        }
        
        public State TryGetState(State state, out bool result)
        {
            var st = states.FirstOrDefault(s => s.Equals(state));
            result = st != null;
            return st;
        }
        
        public State GetState(string stateName) => 
            states.FirstOrDefault(s => s.state == stateName);

        public State GetState(State state) => 
            states.FirstOrDefault(s => s.Equals(state));

        public State GetDefaultState() => states[defaultStateIndex];
    }
}