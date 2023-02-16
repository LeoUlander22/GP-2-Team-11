using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using IntroAssignment;
using Photon.Pun;
using Team11.Health;
using UnityEngine.InputSystem;

namespace Team11.Players {
    public class PlayerBase : MonoBehaviourPunCallbacks {
        [SerializeField] private List<Behaviour> ToEnable;
        [SerializeField] private CharacterController Controller;
        [SerializeField] private Transform PlayerCameraRoot;
        [SerializeField] private GameObject playerModel;
        
        private InputScheme _inputs;
        
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerHealth health;
        private GameObject EscapeMenu;
        bool isPaused = false;

        public void Select(MonoBehaviourPunCallbacks[] ItemsToOwn,PlayerSettings playerSettings) {
            foreach (var behaviour in ToEnable) {
                behaviour.enabled = true;
            }

            foreach (var item in ItemsToOwn) {
                item.photonView.RequestOwnership();
            }

            Controller.enabled = true;
            playerSettings.vcam.Follow = PlayerCameraRoot;
            playerSettings.vignette.SetPlayerHealth(health);
            
            photonView.RequestOwnership();

            EscapeMenu = playerSettings.escapeMenu;
            _inputs = new();
            _inputs.UI.Menu.performed += Menu;
            _inputs.UI.Menu.Enable();
            playerModel.layer = LayerMask.NameToLayer("Invisible");
            
            //Cheats
            DebugCommand revive = new DebugCommand("revive", "Revive the player", "revive",() => {
                health.Revive();
            });
            DebugController.instance.commandList.Add(revive);
            DebugCommand<bool> invincible = new DebugCommand<bool>("invincible", "Make the player invincible", "invincible <true/false>", (bool value) => {
                Debug.Log(value);
                health.Invincibility(value);
            });
            DebugController.instance.commandList.Add(invincible);
        }

        public void Resume() {
            Menu(new InputAction.CallbackContext());
        }
        public void Menu(InputAction.CallbackContext _) {
            isPaused = !isPaused;
            EscapeMenu.SetActive(isPaused);
            playerInput.enabled = !isPaused;
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}