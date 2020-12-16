using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 1.5f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public float playerFoodPoints = 100.0f;

    private Text levelText;
    private GameObject levelImage;
    public static int level = 0;
    private List<EnemyControll> enemies = new List<EnemyControll>();

    public static bool isPause = false;         //메뉴가 호출되면 true

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        ++level;

        InitGame();
    }

    void InitGame() {

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Floor " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage() {
        levelImage.SetActive(false);
    }

    public void GameOver() {
        if (level == 1)
        {
            levelText.text = "You died on the " + level + " floor.";
        }
        else
        {
            levelText.text = "You died on the " + level + " floors.";
        }
        levelImage.SetActive(true);
        enabled = false;
    }

    public void AddEnemyToLise(EnemyControll script)
    {
        enemies.Add(script);
    }

    public void ClickTitle()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene("TitleScene");
    }
}
