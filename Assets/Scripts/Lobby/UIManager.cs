using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject EnforceUI;
    public GameObject GameModeUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && shopUI.activeSelf)
        {
            shopUI.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Escape) && EnforceUI.activeSelf)
        {
            EnforceUI.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Escape) && EnforceUI.activeSelf)
        {
            GameModeUI.SetActive(false);
        }
    }

    public void Shop()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }
    public void Enforce()
    {
        EnforceUI.SetActive(!EnforceUI.activeSelf);
    }
    public void Game()
    {
        GameModeUI.SetActive(!GameModeUI.activeSelf);
    }
}
