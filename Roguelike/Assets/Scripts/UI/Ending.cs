using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public void ClickPlayAgain()
    {
        Debug.Log("로딩");
        GameManager.level = 0;
        Player.food = 100;
        SceneManager.LoadScene("GameScene");
    }

    public void ClickTitle()
    {
        Debug.Log("로딩");
        GameManager.level = 0;
        Player.food = 100;
        SceneManager.LoadScene("TitleScene");
    }

    public void ClickExit()
    {
        Debug.Log("종료");
        Application.Quit();
    }
}
