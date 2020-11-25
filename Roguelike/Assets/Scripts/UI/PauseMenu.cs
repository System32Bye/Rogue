using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject go_BaseUi;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.isPause)
            {
                CallMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUi.SetActive(true);

        Time.timeScale = 0f;
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUi.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ClickReset()
    {
        Debug.Log("리셋");
        SceneManager.LoadScene("GameScene");
        GameManager.isPause = false;
        GameManager.level = 0;
        Player.food = 100;
        Time.timeScale = 1f;
    }

    public void ClickTitle()
    {
        Debug.Log("타이틀");
        SceneManager.LoadScene("TitleScene");
        GameManager.isPause = false;
        GameManager.level = 0;
        Player.food = 100;
        Time.timeScale = 1f;
    }

    public void ClickExit()
    {
        Debug.Log("종료");
        Application.Quit();
    }
}
