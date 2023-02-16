using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public class TorchContainer : PlaceableObject, IContainer
    {
        [SerializeField] private Transform holder;
        public UnityEvent OnTorchPlaced;
        public UnityEvent OnTorchRemoved;

        private bool _firstPutIn;
        private Torch _currentPickup;
        
        public bool PutIn(IPickup pickup)
        {
            if (holder.childCount != 0 || !_firstPutIn)
            {
                _firstPutIn = true;
                return false;
            }
            if(pickup is not Torch torch) return false;
            photonView.RPC(nameof(SetCurrentTorch), RpcTarget.AllBuffered, torch.photonView.ViewID);
            _currentPickup.Place(holder);
            _currentPickup.container = this;
            return true;
        }

        public IPickup TakeOut()
        {
            photonView.RPC(nameof(RemoveTorch), RpcTarget.AllBuffered);
            return _currentPickup;
        }

        [PunRPC]
        private void SetCurrentTorch(int photonViewID)
        {
            _firstPutIn = true;
            _currentPickup = PhotonNetwork.GetPhotonView(photonViewID).GetComponent<Torch>();
            OnTorchPlaced.Invoke();
        }

        [PunRPC]
        private void RemoveTorch()
        {
            _currentPickup = null;
            OnTorchRemoved.Invoke();
        }
    }
}