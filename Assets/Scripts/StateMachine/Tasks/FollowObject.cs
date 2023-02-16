using UnityEngine;

namespace Team11.StateMachine
{
    public class FollowObject : Task
    {
        [SerializeField] private Transform targetObject;

        protected override void PerformTask()
        {
            transform.parent.position += (targetObject.position - transform.parent.position).normalized * (3 * Time.deltaTime);
        }
    }
}