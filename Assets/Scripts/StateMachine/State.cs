namespace Team11.StateMachine
{
    [System.Serializable]
    public class State
    {
        public string state;
        public Task[] tasks;
        public Decision decision;

        public void OnEnterState(StateMachine stateMachine)
        {
            DoAllTasks();
            if(decision != null)
                decision.AddListenerToDecision(stateMachine.ChangeStateBasedOnDecision);
        }
        
        public void OneExitState(StateMachine stateMachine)
        {
            StopAllTasks();
            if(decision != null)
                decision.RemoveListenerToDecision(stateMachine.ChangeStateBasedOnDecision);
        }

        private void DoAllTasks()
        {
            foreach (var task in tasks)
            {
                task.StartTask();
            }
        }

        private void StopAllTasks()
        {
            foreach (var task in tasks)
            {
                task.EndTask();
            }
        }

        public override string ToString()
        {
            return state;
        }
    }
}