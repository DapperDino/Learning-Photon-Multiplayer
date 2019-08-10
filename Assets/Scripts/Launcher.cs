using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhotonLearning
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private byte maxPlayersPerRoom = 4;
        [Required] [SerializeField] private GameObject controlPanel = null;
        [Required] [SerializeField] private GameObject connectionPanel = null;

        private bool isConnecting = false;

        private const string GameVersion = "1";

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        public void Connect()
        {
            isConnecting = true;

            controlPanel.SetActive(false);
            connectionPanel.SetActive(true);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            connectionPanel.SetActive(false);
            controlPanel.SetActive(true);

            Debug.LogWarning($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No random rooms are available, creating a new room");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel("Scene_RoomFor1");
            }
        }
    }
}
