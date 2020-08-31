using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene("GameScene");
    }

    public void ClickTutorial()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene("TutorialScene");
    }

    public void ClickExit()
    {
        Debug.Log("종료");
        Application.Quit();
    }
}
