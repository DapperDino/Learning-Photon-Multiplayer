using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PhotonLearning
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [Required] [SerializeField] private GameObject beams = null;

        private bool isFiring = false;

        private void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }

            if (isFiring != beams.activeSelf)
            {
                beams.SetActive(isFiring);
            }
        }

        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1")) { isFiring = true; }
            if (Input.GetButtonUp("Fire1")) { isFiring = false; }
        }
    }
}
