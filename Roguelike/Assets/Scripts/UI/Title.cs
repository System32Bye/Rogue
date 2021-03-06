﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void ClickStart()
    {
        Debug.Log("로딩");
        GameManager.level = 0;
        Player.food = 100;
        SceneManager.LoadScene("GameScene");
    }

    public void ClickTutorial()
    {
        Debug.Log("로딩");
        GameManager.level = 0;
        Player.food = 100;
        SceneManager.LoadScene("TutorialScene");
    }

    public void ClickExit()
    {
        Debug.Log("종료");
        Application.Quit();
    }
}
