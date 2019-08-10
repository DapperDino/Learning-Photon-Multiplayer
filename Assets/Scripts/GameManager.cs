using Photon.Pun;
using UnityEngine.SceneManagement;

namespace PhotonLearning
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public override void OnLeftRoom() => SceneManager.LoadScene(0);
        public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    }
}
