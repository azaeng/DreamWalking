using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class PlayerData : MonoBehaviour
{
    public Text nicknameText;

    void Start()
    {
        Nick();
    }

    async void Nick()
    {
        string nickname = await FirebaseAuthManager.Instance.GetNickname();

        if (!string.IsNullOrEmpty(nickname))
        {
            nicknameText.text = nickname;
        }
        else
        {
            nicknameText.text = "none";
        }
    }
}
