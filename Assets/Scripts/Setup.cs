using System;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Team11.Interactions;
using Team11.Players;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Team11 {
    public class Setup : MonoBehaviourPunCallbacks {

        [SerializeField] private GameObject CharacterSelection;
        [SerializeField] private Torch[] Torches;
        [SerializeField] private MovableObject[] MovableObjects;
        [SerializeField] private Button LitoBtn;
        [SerializeField] private Button PolrBtn;
        [SerializeField] private Image Crosshair;
        [SerializeField] private TextMeshProUGUI MessageText;

        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private Button ResumeBtn;

        public PlayerBase P1;
        public PlayerBase P2;
        CharacterController characterController;

        private bool cursorLockedAndHidden = false;
        
        [SerializeField] private string MainMenuSceneName = "Proto Main Menu";


        private void Start() {
            if (!PhotonNetwork.OfflineMode && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
                if (!PhotonNetwork.IsMasterClient) {
                    SelectP2();
                    // photonView.RPC(nameof(SelectP2), RpcTarget.OthersBuffered);
                }
                // else {
                //     SelectP2();
                // }}
            }
        }

        public override void OnJoinedRoom() {
            if (PhotonNetwork.OfflineMode || !PhotonNetwork.IsConnected) {
                SelectP1();
            }
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer) {
            if (!PhotonNetwork.OfflineMode && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
                if (PhotonNetwork.IsMasterClient) {
                    SelectP1();
                    // photonView.RPC(nameof(SelectP2), RpcTarget.OthersBuffered);
                }
                // else {
                //     SelectP2();
                // }
            }
        }

        public void SelectP1() {
            // photonView.RPC(nameof(DisableLitoBtn), RpcTarget.OthersBuffered);
            // CharacterSelection.SetActive(false);
            P1.Select(Torches,playerSettings);
            ResumeBtn.onClick.AddListener(P1.Resume);
            SetupPlay();
        }
        
        public void SelectP2() {
            // photonView.RPC(nameof(DisablePolrBtn), RpcTarget.OthersBuffered);
            // CharacterSelection.SetActive(false);
            P2.Select(MovableObjects,playerSettings);
            ResumeBtn.onClick.AddListener(P2.Resume);
            SetupPlay();
        }

        private void SetupPlay() {
            ChangeCursorState(true);
            Crosshair.enabled = true;
            MessageText.gameObject.SetActive(false);
        }

        public void SetCamera(CinemachineVirtualCamera cam) => 
            playerSettings.vcam = cam;

#if UNITY_EDITOR
        [ContextMenu("GetPickups")]
        public void GetAllPickups()
        {
            Torches = FindObjectsOfType<Torch>();
            MovableObjects = FindObjectsOfType<MovableObject>();
            EditorUtility.SetDirty(this);
        }
#endif
        
        // [PunRPC]
        // private void DisableLitoBtn() {
        //     LitoBtn.interactable = false;
        // }
        //
        // [PunRPC]
        // private void DisablePolrBtn() {
        //     PolrBtn.interactable = false;
        // }

        private void ChangeCursorState(bool state) {
            cursorLockedAndHidden = state;
            Cursor.visible = !state;
            Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            MessageText.text = "Other player left the game";
            MessageText.gameObject.SetActive(true);
            Invoke(nameof(Quit), 5f);
        }

        public void CallResetLevel() {
            photonView.RPC(nameof(ResetLevel), RpcTarget.All);
        }

        [PunRPC]
        public void ResetLevel() {
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
        }
        
        public void Quit(){
            PhotonNetwork.LeaveRoom(false);
            PhotonNetwork.LoadLevel(MainMenuSceneName);
        }
    }
    
    [System.Serializable]
    public class PlayerSettings{
        public VignetteIntensity vignette;
        public CinemachineVirtualCamera vcam;
        public GameObject escapeMenu;
    }
}