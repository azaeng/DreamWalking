using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class buttonScript : MonoBehaviour
{
    [SerializeField] Button btnCasual, btnRank, btnSpecial;
    void Start()
    {
        btnCasual.onClick.AddListener(()=>{
            Debug.Log("Clicked Casual");
        });

        btnRank.onClick.AddListener(()=>{
            Debug.Log("Clicked Rank");
        });

        btnSpecial.onClick.AddListener(()=>{
            Debug.Log("Clicked Speical");
        });
    }

    // Update is called once per frame
    void Update()
    {
        // ESC 키를 눌렀는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 전환할 Scene 이름 입력 (예: "MainMenu")
            SceneManager.LoadScene("Lobby");
        }
    }
    
}