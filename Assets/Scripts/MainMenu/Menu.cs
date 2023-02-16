using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Team11.MainMenu {
    public class Menu : MonoBehaviour {
        public GameObject MainMenu;
        public GameObject CreateARoomMenu;
        public GameObject JoinARoomMenu;
        public TMP_InputField RoomNameInputField;
        
        public ConnectToServer connectToServer;

        private void Start() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void GoToCreateARoom() {
            MainMenu.SetActive(false);
            CreateARoomMenu.SetActive(true);
        }
        
        public void GoToJoinARoom() {
            MainMenu.SetActive(false);
            JoinARoomMenu.SetActive(true);
        }
        
        public void GoToMainMenu() {
            MainMenu.SetActive(true);
            CreateARoomMenu.SetActive(false);
            JoinARoomMenu.SetActive(false);
        }

        public void CreateRoom() {
            string roomName = RoomNameInputField.text;
            if (string.IsNullOrEmpty(roomName)) return;
            connectToServer.CreateRoom(RoomNameInputField.text);
        }

        public void RoomCreated() {
            CreateARoomMenu.SetActive(false);
        }
    }
}