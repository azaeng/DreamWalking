using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class JoinManager : MonoBehaviour
{
    public GameObject CreateUI;

    [SerializeField] private InputField IdIF;
    [SerializeField] private InputField PwdIF;
    [SerializeField] private InputField confirmPwdIF;
    [SerializeField] private InputField NickIF;
    [SerializeField] private Text warningText;

    public async void Create()
    {
        string Id = IdIF.text;
        string Pwd = PwdIF.text;
        string confirmPwd = confirmPwdIF.text;
        string Nick = NickIF.text;

        if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Pwd) || string.IsNullOrEmpty(confirmPwd) || string.IsNullOrEmpty(Nick))
        {
            ShowWarning("모든 항목을 입력해주세요.");
            return;
        }

        if (!IsValidEmail(Id))
        {
            ShowWarning("이메일 형식을 확인해주세요. 예: ~~~@gmail.com");
            return;
        }

        if (!IsValidPassword(Pwd))
        {
            ShowWarning("비밀번호는 7자 이상이어야 합니다.");
            return;
        }

        if (Pwd != confirmPwd)
        {
            ShowWarning("비밀번호가 일치하지 않습니다.");
            return;
        }

        // 🔍 닉네임 중복 체크
        bool isDuplicate = await FirebaseAuthManager.Instance.IsNicknameDuplicate(Nick);
        if (isDuplicate)
        {
            ShowWarning("해당 닉네임은 중복됩니다.");
            return;
        }

        // 회원가입 및 닉네임 저장
        FirebaseAuthManager.Instance.Create(Id, Pwd, Nick, OnSuccess, OnFailure);
    }

    private void OnSuccess()
    {
        warningText.text = "회원가입에 성공하셨습니다.";
        warningText.color = Color.green;
        CreateUI.SetActive(false);
    }

    private void OnFailure(string error)
    {
        ShowWarning(error);
    }

    private void ShowWarning(string message)
    {
        warningText.text = message;
        warningText.color = Color.red;
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]{1,}@gmail.com$";
        return Regex.IsMatch(email, pattern);
    }

    private bool IsValidPassword(string password)
    {
        return password.Length >= 7;
    }
}
