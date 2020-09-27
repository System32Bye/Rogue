using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControll : MonoBehaviour
{
    //-----------------------------------------------------
    [SerializeField]
    private int hp;

    [SerializeField]
    private float destroyTime;

    [SerializeField]
    private GameObject _enemy;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float walkTime;
    [SerializeField]
    private float waitTime;

    protected RaycastHit hitCheck;
    public float range = 0.8f;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private CapsuleCollider capCol;
    //-----------------------------------------------------

    protected Vector3 destination;          //방향

    public enum CurrentState {
        Idle, Trace, Attack, Dead
    };
    public CurrentState curState = CurrentState.Idle;

    private Transform _transform;
    private Transform playerTransform;
    private NavMeshAgent nvAgent;


    //private Player currentPlayer;

    private bool playerInRange;
    private float timer;

    //private float timer = 0f;
    public float attackDelay = 2.3f;        //공격 딜레이
    public float traceDist = 3.2f;          //추적 거리
    public float attackDist = 0.7f;         //공격 거리
    private bool isDead = false;            //사망 확인

    public int attackDamage = 5;


    // Use this for initialization
    private void Start () {
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playerInRange = false;
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer >= attackDelay)
            MonAttack();

        if (!isDead) {
            Nav();
        }
    }
    private void Nav()
    {
        GameManager.instance.AddEnemyToLise(this);
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }
 
    protected void MonAttack()
    {
        timer = 0f;
        
        if (CheckObject())
        {
            anim.SetTrigger("monAttack");
            if (hitCheck.transform.tag == "Player")
            {
                hitCheck.transform.GetComponent<Player>().PlayerHit(attackDamage);
            }
        }
    }

    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitCheck, range))
        {
            return true;
        }
        return false;

    }
    //-----------------------------------------------------
    IEnumerator CheckState()
    {
        while (!isDead) {
            
            yield return new WaitForSeconds(2.5f);

            float dist = Vector3.Distance(playerTransform.position, _transform.position);

            //if ()
            if (dist < attackDist && attackDelay <= 0 || playerInRange)
            {
                curState = CurrentState.Attack;
            }
            else if (dist > attackDist && dist < traceDist)
            {
                curState = CurrentState.Trace;
            }
            else
            {
                curState = CurrentState.Idle;
            }
        }
    }

    IEnumerator CheckStateForAction() {

        if (isDead)
        {
            switch (curState)
            {
                case CurrentState.Dead:
                    nvAgent.isStopped = true;
                    anim.SetBool("isDead", true);
                    break;
            }
            yield break;
        }

        while (!isDead) {
            switch (curState) {
                case CurrentState.Idle:
                    nvAgent.isStopped = true;
                    anim.SetBool("isTrace", false);
                    break;
                case CurrentState.Trace:
                    nvAgent.destination = playerTransform.position;
                    nvAgent.isStopped = false;
                    anim.SetBool("isTrace", true);
                    break;
                case CurrentState.Attack:
                    anim.SetTrigger("monAttack");
                    MonAttack();
                    nvAgent.isStopped = true;
                    break;
            }
            yield return null;
        }
    }

    public void Damage(int _dmg, Vector3 _targetPos){
        if (!isDead) {
            hp -= _dmg;
            if (hp <= 0)
            {
                isDead = true;
                anim.SetTrigger("isDead");
                Destruction();
            }
        }
    }

    public void GunDamage(int _dmg){
        if (!isDead) {
            hp -= _dmg;
            if (hp <= 0)
            {
                isDead = true;
                anim.SetTrigger("isDead");
                Destruction();
            }
        }
    }

    private void Destruction() {
        capCol.enabled = false;
        Destroy(_enemy, destroyTime);
    }
    
}