using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControll : MonoBehaviour {

    public enum CurrentState {
        Idle, Trace, Attack, Dead
    };
    public CurrentState curState = CurrentState.Idle;
    public int playerDamage;

    private Transform _transform;
    private Transform playerTransform;
    private NavMeshAgent nvAgent;
    private Animator animator;
    
    //추적 거리
    public float traceDist = 3.2f;

    //공격 거리
    public float attackDist = 0.7f;

    //사망 확인
    private bool isDead = false;

	// Use this for initialization
	void Start () {
        GameManager.instance.AddEnemyToLise(this);
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
	}

    IEnumerator CheckState() {
        while (!isDead) {
            yield return new WaitForSeconds(0.1f);

            float dist = Vector3.Distance(playerTransform.position, _transform.position);

            if (dist < attackDist)
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
                    nvAgent.isStopped = true;
                    animator.SetTrigger("monAttack");
                    break;
           }
            
            yield return null;
        }
    }

}
