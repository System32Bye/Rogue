using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject go_BaseUi;




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
    }

    public void ClickExit()
    {
        Debug.Log("종료");
    }
}
