using UnityEngine;

namespace Team11.Health
{
    public class LightChecker : MonoBehaviour
    {
        protected int SafetyCount;
        
        protected bool IsInSafety => SafetyCount > 0;
        
        private void OnTriggerEnter(Collider other)
        {
            var safeArea = other.gameObject.GetComponent<SafeArea>();
            if (safeArea == null) return;

            SafetyCount++;
        }
        
        private void OnTriggerExit(Collider other)
        {
            var safeArea = other.gameObject.GetComponent<SafeArea>();
            if (safeArea == null) return;

            SafetyCount--;
        }
    }
}