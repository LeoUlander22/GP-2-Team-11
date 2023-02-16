using UnityEngine;
using UnityEngine.Events;

namespace Team11.StateMachine
{
    public abstract class Task : MonoBehaviour
    {
        public bool isContinuousTask;
        public UnityEvent onTaskEnded;

        private bool _canPerformTask = false;

        private void Update()
        {
            if(isContinuousTask && _canPerformTask)
                PerformTask();
        }

        public void StartTask()
        {
            if (!isContinuousTask)
                PerformTask();
            _canPerformTask = true;
        }

        public void EndTask()
        {
            _canPerformTask = false;
        }

        protected abstract void PerformTask();

        protected virtual void OnTaskEnd()
        {
            onTaskEnded?.Invoke();
        }
    }
}