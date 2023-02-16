using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Team11.MainMenu {
    public class RoomListItem : MonoBehaviour {
        private RoomInfo _roomInfo;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        
        public void Initialize(RoomInfo roomInfo) {
            _roomInfo = roomInfo;
            _text.text = roomInfo.Name;
            _button.onClick.AddListener(() => {
                PhotonNetwork.JoinRoom(roomInfo.Name);
            });
        }

        public void Destroy() {
            Destroy(gameObject);
        }
    }
}