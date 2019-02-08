using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInformation : MonoBehaviour
{
    Image info;
    GameObject buttonPanel;

    void Start()
    {
        info = FindObjectOfType<Canvas>().transform.Find(("Info" + name).ToString()).GetComponent<Image>();
        buttonPanel = FindObjectOfType<Canvas>().transform.Find("HUD/Background/ButtonPanel").gameObject;
    }
    
    public void ShowInfo()
    {
        info.gameObject.SetActive(true);
        ARController.InfoVisible = true;
        buttonPanel.SetActive(false);
    }

    public void CloseInfo()
    {
        info.gameObject.SetActive(false);
        ARController.InfoVisible = false;
        buttonPanel.SetActive(true);
    }
}
