using UnityEngine;

namespace Team11.StateMachine
{
    public class IsCloseToObject : Decision
    {
        [SerializeField] private Transform targetObject;
        [SerializeField] private float distanceThreshold;
    
        private void Update()
        {
            if(HasListener)
                MakeDecision();
        }

        protected override void MakeDecision()
        {
            if (Vector3.Distance(transform.position, targetObject.position) <= distanceThreshold)
            {
                InvokeDecision(true);
            }
        }

        protected override void OnGetListener()
        {
            
        }

        protected override void OnLoseListener()
        {
            
        }
    }
}