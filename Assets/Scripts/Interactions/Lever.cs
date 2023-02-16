using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public class Lever : MonoBehaviourPunCallbacks, IInteractable
    {
        public UnityEvent OnPress;
        public UnityEvent OnRevert;

        private bool _pushed;
        
        public void Interact()
        {
            if (_pushed) return;
            photonView.RPC(nameof(Toggle), RpcTarget.AllBuffered);
            OnPress?.Invoke();
        }
        
        public void Revert()
        {
            if (!_pushed) return;
            photonView.RPC(nameof(Toggle), RpcTarget.AllBuffered);
            OnRevert?.Invoke();
        }

        [PunRPC]
        public void Toggle()
        {
            _pushed = !_pushed;
        }
    }
}