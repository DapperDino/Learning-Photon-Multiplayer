using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonLearning
{
    public class PlayerUi : MonoBehaviour
    {
        [SerializeField] private Vector3 screenOffset = new Vector3(0f, 30f, 0f);
        [Required] [SerializeField] private TextMeshProUGUI playerNameText = null;
        [Required] [SerializeField] private Slider playerHealthSlider = null;

        private PlayerManager target = null;
        private Transform targetTransform = null;
        private Renderer targetRenderer = null;
        private CanvasGroup canvasGroup = null;
        private Vector3 targetPosition = Vector3.zero;
        private float characterControllerHeight = 0f;

        private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

        public void SetTarget(PlayerManager target)
        {
            if (target == null)
            {
                Debug.LogError("Missing player manager target");
                return;
            }

            targetTransform = target.transform;
            targetRenderer = target.GetComponent<Renderer>();

            var characterController = target.GetComponent<CharacterController>();
            characterControllerHeight = characterController.height;

            this.target = target;

            playerNameText.text = target.photonView.Owner.NickName;
        }

        private void Update()
        {
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
            }
        }

        private void LateUpdate()
        {
            if (targetRenderer != null)
            {
                canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }
    }
}
