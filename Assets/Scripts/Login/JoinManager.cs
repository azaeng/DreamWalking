
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions; // 이메일 유효성 검사를 위한 정규표현식

public class JoinManager : MonoBehaviour
{
    public GameObject CreateUI;
    
    [SerializeField] private InputField idInputField;
    [SerializeField] private InputField passwordInputField;
    [SerializeField] private InputField confirmPasswordInputField;
    [SerializeField] private Text warningText;

    public void Create()
    {
        // FirebaseAuthManager.Instance.Init(); // 이미 로그인 창에서 초기화됨

        string id = idInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        // 입력값 유효성 검사
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            ShowWarning("모든 항목을 입력해주세요.");
            return;
        }

        if (!IsValidEmail(id))
        {
            ShowWarning("이메일 형식을 확인해주세요. 예: ~~~@gmail.com");
            return;
        }

        if (!IsValidPassword(password))
        {
            ShowWarning("비밀번호는 7자 이상이어야 합니다.");
            return;
        }

        if (password != confirmPassword)
        {
            ShowWarning("비밀번호가 일치하지 않습니다.");
            return;
        }

        // 모든 유효성 검사 통과 → 회원가입 시도
        FirebaseAuthManager.Instance.Create(id, password);
        warningText.text = "회원가입에 성공하셨습니다.";
        warningText.color = Color.green;

        CreateUI.SetActive(!CreateUI.activeSelf);
    }

    // 경고 메시지를 빨간색으로 출력
    private void ShowWarning(string message)
    {
        warningText.text = message;
        warningText.color = Color.red;
    }

    // 이메일 형식 검사: @ 앞 최소 1자, .com으로 끝나는지 확인
    private bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]{1,}@gmail.com$";
        return Regex.IsMatch(email, pattern);
    }

    // 비밀번호 길이 검사
    private bool IsValidPassword(string password)
    {
        return password.Length >= 7;
    }
}
