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

    private Vector3 direction;

    private bool isAction;
    private bool isWalking;

    [SerializeField]
    private float walkTime;
    [SerializeField]
    private float waitTime;

    private float currentTime;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private CapsuleCollider capCol;
    //-----------------------------------------------------


    public enum CurrentState {
        Idle, Trace, Attack, Dead
    };
    public CurrentState curState = CurrentState.Idle;
    //public int playerDamage;

    private Transform _transform;
    private Transform playerTransform;
    private NavMeshAgent nvAgent;
    //private Animator animator;

    public float attackDelay;               //공격 딜레이
    public float traceDist = 3.2f;          //추적 거리
    public float attackDist = 0.7f;         //공격 거리
    private bool isDead = false;            //사망 확인


    // Use this for initialization
    private void Start () {
        //-----------------------------------------------------
        currentTime = waitTime;
        isAction = true;
        //-----------------------------------------------------
    }
    //-----------------------------------------------------

    private void Move() {
        if (isWalking)
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
    }

    private void Rotation() {
        if (isWalking) {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }

    private void Update() {
        Move();
        Rotation();
        ElapseTime();
        Nav();
    }

    private void Nav() {
        GameManager.instance.AddEnemyToLise(this);
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    private void ElapseTime() {
        if (isAction) {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                ResetState();
        }

    }

    private void ResetState() {
        isWalking = false;
        isAction = true;
        anim.SetBool("isTrace", isWalking);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction() {

        int _random = Random.Range(0, 2);

        if (_random == 0) {
            Wait();
        }
        else if (_random == 1) {
            TryWalk();
        }
    }

    private void Wait() {
        currentTime = waitTime;
        Debug.Log("대기");
    }

    private void TryWalk() {
        isWalking = true;
        anim.SetBool("isTrace", isWalking);
        currentTime = walkTime;
        Debug.Log("걷기");
    }

    //-----------------------------------------------------
    IEnumerator CheckState()
    {
        while (!isDead) {
            
            yield return new WaitForSeconds(0.1f);

            float dist = Vector3.Distance(playerTransform.position, _transform.position);

            if (dist < attackDist)
            {
                curState = CurrentState.Attack;
                yield return new WaitForSeconds(attackDelay);
            }
            else if (dist > attackDist&& dist < traceDist)
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
                    yield return new WaitForSeconds(attackDelay);
                    break;
            }
            yield return null;
        }
    }

    public void Damage(){
        hp--;

        if (hp <= 0){
            isDead = true;
            anim.SetTrigger("isDead");
            Destruction();
        }
    }
    private void Destruction() {
        capCol.enabled = false;
        Destroy(_enemy, destroyTime);
    }
}
