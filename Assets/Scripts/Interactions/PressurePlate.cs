using System.Collections.Generic;
using Photon.Pun;
using Team11.Interactions;
using Team11.Players;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviourPunCallbacks
{
    List<int> objectsOnPlate = new();
    public UnityEvent OnPress;
    public UnityEvent OnRelease;

    protected virtual void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerBase>() == null && other.GetComponent<MovableObject>() == null) return;
        int objectID = other.GetComponent<PhotonView>().ViewID;
        if (!PhotonNetwork.GetPhotonView(objectID).IsMine) return;
        photonView.RPC(nameof(ObjectEntered), RpcTarget.All, objectID);
    }

    [PunRPC]
    protected virtual void ObjectEntered(int objectID) {
        if (objectsOnPlate.Count == 0) {
            OnPress?.Invoke();
            Debug.Log("Press");
        }
        objectsOnPlate.Add(objectID);
    }

    protected virtual void OnTriggerExit(Collider other) {
        int objectID = other.gameObject.GetPhotonView().ViewID;
        if (!PhotonNetwork.GetPhotonView(objectID).IsMine) return;
        if (!objectsOnPlate.Contains(objectID)) return;
        photonView.RPC(nameof(ObjectExit), RpcTarget.All, objectID);
    }
    
    [PunRPC]
    protected virtual void ObjectExit(int objectID) {
        objectsOnPlate.Remove(objectID);
        if(objectsOnPlate.Count == 0) {
            OnRelease?.Invoke();
            Debug.Log("Release");
        }
        
    }
}
