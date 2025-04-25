using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions; // 정규식 사용을 위한 네임스페이스 추가

using Firebase.Database;


public class JoinManager : MonoBehaviour
{
    [SerializeField] private InputField idInputField;
    [SerializeField] private InputField passwordInputField;
    [SerializeField] InputField confirmPasswordInputField;
    [SerializeField] Text warningText;

    public void Create()
    {
        FirebaseAuthManager.Instance.Init();

        string id = idInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        // 입력값 유효성 검사

    }
}
