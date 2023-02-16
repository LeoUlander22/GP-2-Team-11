using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

namespace Team11.MainMenu
{
    public class ConnectToServer : MonoBehaviourPunCallbacks {
        public Menu Mainmenu;
        public String gameScene;
        
        public bool offlineMode = false;
        
        private void Start() {
            if (offlineMode) {
                PhotonNetwork.OfflineMode = true;
            }
            else {
                // // LoadBalancingClient loadBalancingClient = PhotonNetwork.NetworkingClient;
                // PhotonNetwork.
                // PhotonNetwork.NetworkingClient.ConnectToNameServer();
                
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        // public override void OnRegionListReceived(RegionHandler regionHandler) {
        //     Debug.Log("Region List Received");
        //     // regionHandler.PingMinimumOfRegions()
        //     foreach (Region region in regionHandler.EnabledRegions) {
        //         Debug.Log(region.Code);
        //     }
        //     // PhotonNetwork.NetworkingClient.ConnectToRegionMaster()
        // }

        public override void OnConnectedToMaster() {
            if (offlineMode) {
                PhotonNetwork.CreateRoom("Testing Room");
            }
            else {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnJoinedLobby()
        {
            // ShowTextObj.SetActive(false);
            // RoomForm.SetActive(true);
            Mainmenu.gameObject.SetActive(true);
        }
        public void CreateRoom(string roomID){
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom(roomID,roomOptions,TypedLobby.Default);
            
            // RoomForm.SetActive(false);
            // ShowTextObj.SetActive(true);
            // ErrorTextObj.SetActive(false);
            // ShowText.text = "Loading";
        }
        public override void OnJoinedRoom(){

            Debug.Log("Joined Room");
            Mainmenu.RoomCreated();
            PhotonNetwork.LoadLevel(gameScene);
            // PhotonNetwork.
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            // OnJoinedLobby();
            // ErrorTextObj.SetActive(true);
            // ErrorText.text = message;
        }
    }
}
