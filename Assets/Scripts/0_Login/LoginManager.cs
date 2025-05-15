using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField Id;
    public InputField Pwd;
    public GameObject CreateUI;
    public Text warningText;
    private bool LoginDone;

    void Start()
    {
        FirebaseAuthManager.Instance.Init();
        LoginDone = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CreateUI.activeSelf)
        {
            CreateUI.SetActive(false);
        }
        if(LoginDone)
        {
            LoginDone = false;
            SceneManager.LoadScene(1);
        }
    }

    public void Create()
    {
        CreateUI.SetActive(!CreateUI.activeSelf); // 회원가입 화면으로 전환
    }

    public void Login()
    {
        FirebaseAuthManager.Instance.Login(Id.text, Pwd.text,
            onSuccess: () => {LoginDone = true;},
            onFailure: (error) => {LoginDone = false;}
        );
    }


    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }
}
