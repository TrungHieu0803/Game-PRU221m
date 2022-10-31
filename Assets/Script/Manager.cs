using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public void PlayGameButton()
    {
        Application.LoadLevel("SC Pixel Art Top Down - Basic");

    }
    public void QuitGameButton()
    {
        Application.Quit();
    }
}
