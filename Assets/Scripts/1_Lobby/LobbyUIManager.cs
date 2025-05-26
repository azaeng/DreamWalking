using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    public GameObject ShopUI;
    public GameObject EnforceUI;
    public GameObject StorageUI;
    public GameObject GameModeUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ShopUI.activeSelf)
        {
            ShopUI.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Escape) && EnforceUI.activeSelf)
        {
            EnforceUI.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Escape) && StorageUI.activeSelf)
        {
            StorageUI.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GameModeUI.activeSelf)
        {
            GameModeUI.SetActive(false);
        }
    }

    public void Shop()
    {
        ShopUI.SetActive(!ShopUI.activeSelf);
    }
    public void Enforce()
    {
        EnforceUI.SetActive(!EnforceUI.activeSelf);
    }
    public void Storage()
    {
        StorageUI.SetActive(!StorageUI.activeSelf);
    }
    public void Game()
    {
        GameModeUI.SetActive(!GameModeUI.activeSelf);
    }
}
