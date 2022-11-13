using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class StartMenuController : MonoBehaviour
{
    public static StartMenuController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI Textprogress;
    public GameObject resumeBtn;
    public GameObject confirmMenu;
    public bool isLoad;
    private bool isSavedDataExist;
    private string path;
    private void Start()
    {
        path = $"{Application.persistentDataPath}";
        isLoad = false;
        isSavedDataExist = File.Exists(Path.Combine(path, "player.json"));
        if (!isSavedDataExist)
        {
            resumeBtn.SetActive(false);
        }

    }
    public void LoadLevel(int sceneIndex)
    {
        if (isSavedDataExist)
        {
            confirmMenu.SetActive(true);
        }
        else
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
            loadingScreen.SetActive(true);
        }
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

    public void CheckConfirm(bool isOk)
    {
        if (isOk)
        {
            File.Delete(Path.Combine(path, "player.json"));
            File.Delete(Path.Combine(path, "bullets.json"));
            File.Delete(Path.Combine(path, "enemies.json"));
            File.Delete(Path.Combine(path, "weapons.json"));
            File.Delete(Path.Combine(path, "ammoes.json"));
            StartCoroutine(LoadAsynchronously(1));
            loadingScreen.SetActive(true);
        }
        else
        {
            confirmMenu.SetActive(false);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
