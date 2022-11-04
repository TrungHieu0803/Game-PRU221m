using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField]
    private TextMeshProUGUI killedEnemies;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateKill()
    {

        killedEnemies.text = (Int32.Parse(killedEnemies.text) + 1).ToString();
    }





}
