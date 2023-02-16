using System;
using Photon.Pun;
using UnityEngine;

namespace Team11.MainMenu {
    public class OfflineTester : MonoBehaviourPunCallbacks {
        private void Start() {
            if (!PhotonNetwork.IsConnected) {
                PhotonNetwork.OfflineMode = true;
            }
        }
        
        public override void OnConnectedToMaster() {
            PhotonNetwork.CreateRoom("Testing Room");
        }
    }
}