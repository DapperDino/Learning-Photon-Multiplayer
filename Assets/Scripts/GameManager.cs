using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotonLearning
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public override void OnLeftRoom() => SceneManager.LoadScene(0);
        public void LeaveRoom() => PhotonNetwork.LeaveRoom();

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player({newPlayer.NickName}) has entered the room");

            if (PhotonNetwork.IsMasterClient) { LoadArena(); }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"Player({otherPlayer.NickName}) has left the room");

            if (PhotonNetwork.IsMasterClient) { LoadArena(); }
        }

        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Can't load level unless we are the Master Client");
            }

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            Debug.Log($"Loading Level : {playerCount}");

            PhotonNetwork.LoadLevel($"Scene_RoomFor{playerCount}");
        }
    }
}
