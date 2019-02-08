using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDSolarSystem : MonoBehaviour
{
    [SerializeField] GameObject solarSystem;
    [SerializeField] GameObject DropdownPanel;
    [SerializeField] GameObject DropdownButton;

    Transform dropDownImg;
    Slider scaleSlider;

    void Start()
    {
        dropDownImg = DropdownButton.transform.Find("Image");
        scaleSlider = DropdownPanel.transform.Find("ScaleSlider").GetComponent<Slider>();
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ToggleDropdownMenu()
    {
        if (!solarSystem.activeSelf)
            return;

        if (!DropdownPanel.activeSelf)
        {
            DropdownPanel.SetActive(true);
            ARController.DropdownVisible = true;
            dropDownImg.rotation = new Quaternion(180, 0, 0, 0);
        }

        else
        {
            DropdownPanel.SetActive(false);
            ARController.DropdownVisible = false;
            dropDownImg.rotation = new Quaternion(0, 0, 0, 0);
        }
            
    }

    public void AdjustScaleSlider(float scale)
    {
        scaleSlider.value = scale;
    }
}
