using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public float moveTime = 0.08f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    Vector3 lookDirection;
    bool IsWalk = false;

    private Animator animator;
    private int food;

    // Use this for initialization
    private void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Time: " + food;
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update() {
        
        int horizontal = 0;
        int vertical = 0;

        animator.SetBool("IsWalk", IsWalk);

        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow)) {

            IsWalk = true;

            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");
            lookDirection = vertical * Vector3.forward + horizontal * Vector3.right;
            
            this.transform.rotation = Quaternion.LookRotation(lookDirection);
            this.transform.Translate(Vector3.forward * moveTime);
        }
        else {
            IsWalk = false;
        }

    }


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "Time: " + food + "+" + pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "Time: " + food + "+" + pointsPerFood;
            other.gameObject.SetActive(false);
        }
    }
    
    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text= "Time: " + food + "-" + loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver() {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
