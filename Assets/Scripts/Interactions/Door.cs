using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private TorchContainer[] connectedContainers;
        [SerializeField] private PressurePlate[] connectedPressurePlates;
        [SerializeField] private CombinationLock[] connectedCombinationLocks;
        public UnityEvent OnOpen;
        public UnityEvent OnClose;
    
        public bool openState;
        public int openRequirement;
        [ReadOnly]public int currentCount;

        public Animator doorAnimator;

        private void Start() {
            foreach (var container in connectedContainers) {
                container.OnTorchPlaced.AddListener(ConditionProgress);
                container.OnTorchRemoved.AddListener(ConditionRegress);
            }
            foreach (var pressurePlate in connectedPressurePlates) {
                pressurePlate.OnPress.AddListener(ConditionProgress);
                pressurePlate.OnRelease.AddListener(ConditionRegress);
            }
            foreach (var combinationLock in connectedCombinationLocks) {
                combinationLock.OnUnlock.AddListener(ConditionProgress);
            }
            if (openState) {
                OpenDoor();
            }
        }

        public void ConditionProgress() {
            currentCount++;
            if(currentCount >= openRequirement && !openState) {
                OpenDoor();
            }
        }

        public void ConditionRegress() {
            currentCount--;
            if(currentCount < openRequirement) {
                CloseDoor();
            }
        }

        public void OpenDoor() {
            openState = true;
            // Play animation
            doorAnimator.SetBool("open", openState);
            OnOpen?.Invoke();
        }

        public void CloseDoor() {
            openState = false;
            //PlayAnimation
            doorAnimator.SetBool("open", openState);
            OnClose?.Invoke();
        }
    }
}
