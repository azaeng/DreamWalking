using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject EnforceUI;

    void Start()
    {
        shopUI.SetActive(false);
        EnforceUI.SetActive(false);
    }

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
    }

    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }
    public void ToggleEnforce()
    {
        EnforceUI.SetActive(!EnforceUI.activeSelf);
    }
}
