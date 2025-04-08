using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson.PunDemos
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // 이 오브젝트에 연결된 ThirdPersonCharacter 참조
        private Transform m_Cam;                  // 씬에서 메인 카메라의 Transform 참조
        private Vector3 m_CamForward;             // 카메라의 현재 전방 방향
        private Vector3 m_Move;                   // 이동 벡터
        private bool m_Jump;                      // 점프 여부 (카메라 기준의 이동 방향을 계산해 사용자의 입력에 따라 결정됨)

        private void Start()
        {
            // 메인 카메라의 Transform을 가져옴
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "경고: 메인 카메라가 없습니다. ThirdPersonCharacter는 \"MainCamera\" 태그가 설정된 카메라가 필요합니다. 없을 경우 카메라 기준 이동이 적용되지 않습니다.", gameObject);
                // 이 경우, 캐릭터 기준의 상대적인 이동이 사용됩니다. 의도한 바는 아닐 수 있지만, 경고는 했습니다!
            }

            // ThirdPersonCharacter 컴포넌트를 가져옴 (RequireComponent 때문에 null이 될 수 없음)
            m_Character = GetComponent<ThirdPersonCharacter>();
        }

        private void Update()
        {
            // 점프 키가 눌렸는지 확인 (한 번만 체크)
            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }
        }

        // FixedUpdate는 물리 연산과 동기화되어 호출됨
        private void FixedUpdate()
        {
            // 입력 값 받기
            float h = Input.GetAxis("Horizontal"); // 좌우 이동
            float v = Input.GetAxis("Vertical");   // 앞뒤 이동
            bool crouch = Input.GetKey(KeyCode.C); // C키로 앉기 여부 판단

            // 캐릭터에 전달할 이동 방향 계산
            if (m_Cam != null)
            {
                // 카메라 기준의 상대적 이동 방향 계산
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // 메인 카메라가 없을 경우, 월드 기준 방향 사용
                m_Move = v * Vector3.forward + h * Vector3.right;
            }

#if !MOBILE_INPUT
            // 쉬프트 키를 누르면 걷기 속도로 이동
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // 캐릭터 컨트롤 스크립트에 모든 파라미터 전달
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false; // 점프 초기화
        }
    }
}
