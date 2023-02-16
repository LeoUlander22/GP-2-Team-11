using System;
using UnityEngine;

namespace Team11.StateMachine
{
    public abstract class Decision : MonoBehaviour
    {
        protected event Action<bool, string, string> OnConditionsMet;

        [SerializeField] protected string stateOnTrue;
        [SerializeField] protected string stateOnFalse;

        protected bool HasListener;

        protected abstract void MakeDecision();
        protected abstract void OnGetListener();
        protected abstract void OnLoseListener();

        public void AddListenerToDecision(Action<bool, string, string> action)
        {
            OnConditionsMet += action;
            HasListener = true;
            OnGetListener();
        }

        public void RemoveListenerToDecision(Action<bool, string, string> action)
        {
            OnLoseListener();
            HasListener = false;
            OnConditionsMet -= action;
        }

        protected void InvokeDecision(bool conditionMet)
        {
            OnConditionsMet?.Invoke(conditionMet, stateOnTrue, stateOnFalse);
        }
    }
}