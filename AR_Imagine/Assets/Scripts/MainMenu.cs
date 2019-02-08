using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider loadingBar;

    GameObject aboutPanel;
    GameObject loadingPanel;
    GameObject continueBtn;

    AsyncOperation operation;

    void Start()
    {
        aboutPanel = transform.Find("AboutPanel").gameObject;
        loadingPanel = transform.Find("LoadingPanel").gameObject;
        continueBtn = loadingPanel.transform.Find("ContinueBtn").gameObject;
        print(continueBtn);
    }

    public void QuitApp()
    {
        print("quiting app");
        Application.Quit();
    }

    public void MoveToSolarSystem()
    {
        StartCoroutine(LoadSolarSystem());
    }

    IEnumerator LoadSolarSystem()
    {
        loadingPanel.SetActive(true);

        operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("SolarSystem");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.value = progress;

            yield return null;

            if(loadingBar.value == 1f)
                continueBtn.SetActive(true);
        }
        
       

    }

    public void AllowSceneActivation()
    {
        operation.allowSceneActivation = true;
    }

    public void ToggleAboutPanel()
    {
        if (!aboutPanel.activeSelf)
            aboutPanel.SetActive(true);
        else
            aboutPanel.SetActive(false);
    }

}
