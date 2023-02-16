using Photon.Pun;
using UnityEngine;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView), typeof(PhotonTransformView))]
    public class MovableObject : PickupBase, IInteractable
    {
        private Joint _joint;
        private Rigidbody _rigidbody;
        private Collider _collider;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public override void Pickup(Transform tr)
        {
            photonView.RPC(nameof(SetLayer), RpcTarget.AllBuffered, "Ignore Raycast");
            _joint = tr.GetComponent<Joint>();
            _joint.connectedBody = _rigidbody;
            _rigidbody.drag = 50;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
        }

        public override void Place(Transform tr)
        {
            photonView.RPC(nameof(SetLayer), RpcTarget.AllBuffered, "Default");
            _joint.connectedBody = null;
            _rigidbody.drag = 5;
        }

        public void Interact()
        {
            _rigidbody.useGravity = !_rigidbody.useGravity;
        }

        [PunRPC]
        public void SetLayer(string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}