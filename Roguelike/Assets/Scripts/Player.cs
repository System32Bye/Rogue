using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : CheckingDead
{
    public float moveTime = 0.08f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 0.5f;
    public Text foodText;

    Vector3 lookDirection;
    bool IsWalk = false;

    private Animator animator;
    private float food;
    //---------------------------------------
    
    //public string weaponName;   //무기 이름
    public float range;         //공격 범위
    //public int damage;          //공격력
    public float attackDelay;   //공격 딜레이
    public float attackDelayA;  //공격 활성화 시점
    public float attackDelayB;  //공격 비활성화 시점

    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;
    //---------------------------------------

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;
        startingHealth = food;

        foodText.text = "Time: " + food;
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {

        int horizontal = 0;
        int vertical = 0;


        food -= Time.deltaTime;
        foodText.text = "Time: " + (food).ToString("0");
        if (food < 0f)
        {
            CheckIfGameOver();
        }

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

            this.transform.rotation = Quaternion.LookRotation(lookDirection);
            this.transform.Translate(Vector3.forward * moveTime);
        }
        else
        {
            IsWalk = false;
        }
        //---------------------------------------
        TryAttack();
        //---------------------------------------
    }
    //---------------------------------------
    private void TryAttack()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isAttack)
            {
                //코루틴 실행
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        animator.SetTrigger("playerAttack");

        yield return new WaitForSeconds(attackDelayA);
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(attackDelay - attackDelayA - attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                //충돌됨
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        Debug.DrawRay(transform.position, transform.forward * range, Color.blue, 0.3f);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range))
        {
            return true;
        }
        return false;
    }
    //---------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "Time: " + food + ("+" + pointsPerFood);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "Time: " + food + ("+" + pointsPerFood);
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "Time: " + (food).ToString("0") + "-" + loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
