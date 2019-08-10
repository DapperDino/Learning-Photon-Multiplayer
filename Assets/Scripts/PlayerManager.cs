using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotonLearning
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private float health = 1f;
        [Required] [SerializeField] private GameObject beams = null;
        [Required] [SerializeField] private GameObject playerUiPrefab = null;

        public static GameObject localPlayerInstance = null;

        private bool isFiring = false;

        public float Health => health;

        public override void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                PlayerManager.localPlayerInstance = gameObject;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                GetComponent<CameraWork>().OnStartFollowing();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;

            Instantiate(playerUiPrefab).GetComponentInChildren<PlayerUi>().SetTarget(this);
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (health <= 0f)
                {
                    GameManager.instance.LeaveRoom();
                    return;
                }

                ProcessInputs();
            }

            if (isFiring != beams.activeSelf)
            {
                beams.SetActive(isFiring);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine) { return; }

            if (!other.name.Contains("Beam")) { return; }

            health -= 0.1f;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine) { return; }

            if (!other.name.Contains("Beam")) { return; }

            health -= 0.1f * Time.deltaTime;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isFiring);
                stream.SendNext(health);
            }
            else
            {
                isFiring = (bool)stream.ReceiveNext();
                health = (float)stream.ReceiveNext();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadingMode)
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            Instantiate(playerUiPrefab).GetComponentInChildren<PlayerUi>().SetTarget(this);
        }

        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1")) { isFiring = true; }
            if (Input.GetButtonUp("Fire1")) { isFiring = false; }
        }
    }
}
