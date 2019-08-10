using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonLearning
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        private const string playerNamePrefKey = "PlayerName";

        private void Start() => SetUpInputField();

        private void SetUpInputField()
        {
            string defaultName = string.Empty;

            TMP_InputField inputField = GetComponent<TMP_InputField>();

            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }

            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return; }

            PhotonNetwork.NickName = name;

            PlayerPrefs.SetString(playerNamePrefKey, name);
        }
    }
}
