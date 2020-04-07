using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControll : CheckingDead
{

    public enum CurrentState {
        Idle, Trace, Attack, Dead
    };
    public CurrentState curState = CurrentState.Idle;
    //public int playerDamage;

    private Transform _transform;
    private Transform playerTransform;
    private NavMeshAgent nvAgent;
    private Animator animator;

    public float attackDelay;               //공격 딜레이
    public float traceDist = 3.2f;          //추적 거리
    public float attackDist = 0.7f;         //공격 거리
    private bool isDead = false;            //사망 확인


    // Use this for initialization
    protected override void Start () {
        base.Start();

        GameManager.instance.AddEnemyToLise(this);
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());

    }

    IEnumerator CheckState()
    {
        if (dead)
        {
            isDead = true;
            curState = CurrentState.Dead;
            yield break;
        }

        while (!isDead) {
            
            yield return new WaitForSeconds(0.1f);

            float dist = Vector3.Distance(playerTransform.position, _transform.position);

            if (dist < attackDist)
            {
                curState = CurrentState.Attack;
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
                    animator.SetBool("isDead", true);
                    break;
            }
            yield break;
        }

        while (!isDead) {
            switch (curState) {
                case CurrentState.Idle:
                    nvAgent.isStopped = true;
                    animator.SetBool("isTrace", false);
                    break;
                case CurrentState.Trace:
                    nvAgent.destination = playerTransform.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("isTrace", true);
                    break;
                case CurrentState.Attack:
                    animator.SetTrigger("monAttack");
                    yield return new WaitForSeconds(attackDelay);
                    break;
            }
            yield return null;
        }
    }

    public void ChangeToMonDead() {

    }

}
