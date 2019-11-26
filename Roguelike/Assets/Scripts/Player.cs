using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    Vector3 lookDirection;
    bool IsWalk = false;

    private Animator animator;
    private int food;

    // Use this for initialization
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update() {
        if (!GameManager.instance.playersTurn) return;
        
        int horizontal = 0;
        int vertical = 0;

        animator.SetBool("IsWalk", IsWalk);

        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {

            IsWalk = true;

            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");
            lookDirection = vertical * Vector3.forward + horizontal * Vector3.right;

            //Vector3 dir = new Vector3(horizontal*0.1f, 0, vertical*0.1f);
            this.transform.rotation = Quaternion.LookRotation(lookDirection);
            this.transform.Translate(Vector3.forward * moveTime);
            //this.transform.Translate(dir);
        }
        else {
            IsWalk = false;
        }
        if (horizontal != 0)
            vertical = 0;
/*
        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);*/
    }

    protected override void AttemptMove<T>(int xDir, int zDir) {
        food--;

        base.AttemptMove<T>(xDir, zDir);

        RaycastHit hit;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
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
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component) {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver() {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
