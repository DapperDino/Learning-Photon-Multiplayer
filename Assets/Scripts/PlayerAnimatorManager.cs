using Photon.Pun;
using UnityEngine;

namespace PhotonLearning
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        [SerializeField] private float directionDampTime = 0.25f;

        private Animator animator = null;

        private static readonly int hashSpeed = Animator.StringToHash("Speed");
        private static readonly int hashDirection = Animator.StringToHash("Direction");
        private static readonly int hashJump = Animator.StringToHash("Jump");

        private void Start() => animator = GetComponent<Animator>();

        private void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected) { return; }

            Move();
        }

        private void Move()
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger(hashJump);
                }
            }

            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (verticalInput < 0) { verticalInput = 0; }

            animator.SetFloat(hashSpeed, Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
            animator.SetFloat(hashDirection, horizontalInput, directionDampTime, Time.deltaTime);
        }
    }
}
