using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI Textprogress;
    public bool isLoad;

    private void Start()
    {
        isLoad = false;
    }
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
        loadingScreen.SetActive(true);
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            Textprogress.text = (int)progress * 100f + "%";
            yield return null;
        }
    }
    public void ResumeButton(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
        loadingScreen.SetActive(true);
        isLoad = true;
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
