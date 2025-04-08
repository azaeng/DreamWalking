using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson.PunDemos
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonCharacter : MonoBehaviour
    {
        [SerializeField] float m_MovingTurnSpeed = 360;         // 이동 중 회전 속도
        [SerializeField] float m_StationaryTurnSpeed = 180;     // 정지 중 회전 속도
        [SerializeField] float m_JumpPower = 12f;                // 점프 힘
        [Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;  // 중력 배수
        [SerializeField] float m_RunCycleLegOffset = 0.2f;       // 달리는 애니메이션의 다리 움직임 오프셋
        [SerializeField] float m_MoveSpeedMultiplier = 1f;       // 이동 속도 배수
        [SerializeField] float m_AnimSpeedMultiplier = 1f;       // 애니메이션 속도 배수
        [SerializeField] float m_GroundCheckDistance = 0.1f;     // 땅 체크 거리

        Rigidbody m_Rigidbody;
        Animator m_Animator;
        bool m_IsGrounded;                // 땅에 닿았는지 여부
        float m_OrigGroundCheckDistance;
        const float k_Half = 0.5f;
        float m_TurnAmount;
        float m_ForwardAmount;
        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;
        bool m_Crouching;                 // 웅크리는 상태인지

        void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;

            // Rigidbody 회전을 고정
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            m_OrigGroundCheckDistance = m_GroundCheckDistance;
        }

        public void Move(Vector3 move, bool crouch, bool jump)
        {
            if (move.magnitude > 1f) move.Normalize();

            // 월드 좌표 기준의 방향을 로컬 좌표로 변환
            move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_TurnAmount = Mathf.Atan2(move.x, move.z);
            m_ForwardAmount = move.z;

            ApplyExtraTurnRotation();

            if (m_IsGrounded)
            {
                HandleGroundedMovement(crouch, jump);  // 지면 위 동작 처리
            }
            else
            {
                HandleAirborneMovement();              // 공중 동작 처리
            }

            ScaleCapsuleForCrouching(crouch);          // 웅크림 크기 조정
            PreventStandingInLowHeadroom();           // 좁은 공간에서 서는 것 방지
            UpdateAnimator(move);                      // 애니메이터 업데이트
        }

        void ScaleCapsuleForCrouching(bool crouch)
        {
            if (m_IsGrounded && crouch)
            {
                if (m_Crouching) return;
                m_Capsule.height /= 2f;
                m_Capsule.center /= 2f;
                m_Crouching = true;
            }
            else
            {
                // 위에 장애물이 있는지 확인
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                    return;
                }
                m_Capsule.height = m_CapsuleHeight;
                m_Capsule.center = m_CapsuleCenter;
                m_Crouching = false;
            }
        }

        void PreventStandingInLowHeadroom()
        {
            if (!m_Crouching)
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                }
            }
        }

        void UpdateAnimator(Vector3 move)
        {
            // 애니메이터에 파라미터 전달
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            m_Animator.SetBool("Crouch", m_Crouching);
            m_Animator.SetBool("OnGround", m_IsGrounded);

            if (!m_IsGrounded)
            {
                m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
            }

            // 점프 다리 위치 계산
            float runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
            if (m_IsGrounded)
            {
                m_Animator.SetFloat("JumpLeg", jumpLeg);
            }

            // 애니메이션 속도 설정
            if (m_IsGrounded && move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                m_Animator.speed = 1;
            }
        }

        void HandleAirborneMovement()
        {
            // 중력 보정 적용
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);

            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
        }

        void HandleGroundedMovement(bool crouch, bool jump)
        {
            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // 점프 실행
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                m_IsGrounded = false;
                m_Animator.applyRootMotion = false;
                m_GroundCheckDistance = 0.1f;
            }
        }

        void ApplyExtraTurnRotation()
        {
            // 캐릭터 회전 보정
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        }

        public void OnAnimatorMove()
        {
            // 루트 모션을 커스터마이즈하여 이동 적용
            if (m_IsGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
                v.y = m_Rigidbody.velocity.y; // y축 속도 유지
                m_Rigidbody.velocity = v;
            }
        }

        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // 씬 뷰에서 디버그용 선을 그림
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
            // 아래 방향으로 레이캐스트를 쏴서 땅에 닿았는지 체크
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                m_GroundNormal = hitInfo.normal;
                m_IsGrounded = true;
                m_Animator.applyRootMotion = true;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundNormal = Vector3.up;
                m_Animator.applyRootMotion = false;
            }
        }
    }
}
