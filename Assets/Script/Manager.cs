using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public void PlayGameButton()
    {
        SceneManager.LoadScene("SC All Props");

    }
    public void QuitGameButton()
    {
        Application.Quit();
    }
}
