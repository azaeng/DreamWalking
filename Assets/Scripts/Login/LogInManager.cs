using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogInManager : MonoBehaviour
{
    public InputField id;
    public InputField password;
    public GameObject CreateUI;

    // 로그인 상태 변화 출력용 텍스트
    // public Text outputText;

    // Start is called before the first frame update
    void Start()
    {
        // FirebaseAuthManager의 상태 변경 이벤트에 콜백 추가 (현재 미사용)
        // FirebaseAuthManager.Instance.LoginState += OnChangedState;

        FirebaseAuthManager.Instance.Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CreateUI.activeSelf)
        {
            CreateUI.SetActive(false);
        }
    }

    // 로그인 상태 변화시 텍스트 업데이트
    //private void OnChangedState(bool sign)
    //{
    //    outputText.text = sign ? "로그인됨" : "로그아웃됨";
    //    outputText.text += FirebaseAuthManager.Instance.UserId;
    //}

    public void Create()
    {
        CreateUI.SetActive(!CreateUI.activeSelf); // 회원가입 화면으로 전환
    }

    public void Login()
    {
        FirebaseAuthManager.Instance.Login(id.text, password.text);

        // 로그인 성공 시 전환할 씬 지정
        SceneManager.LoadScene(1);
    }

    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }
}
