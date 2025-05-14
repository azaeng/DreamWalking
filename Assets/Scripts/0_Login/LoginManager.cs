using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField Id;
    public InputField Pwd;
    public GameObject CreateUI;

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

    public void Create()
    {
        CreateUI.SetActive(!CreateUI.activeSelf); // 회원가입 화면으로 전환
    }

    public void Login()
    {
        FirebaseAuthManager.Instance.Login(Id.text, Pwd.text);

        // 로그인 성공 시 전환할 씬 지정
        SceneManager.LoadScene(1);
    }

    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }
}
