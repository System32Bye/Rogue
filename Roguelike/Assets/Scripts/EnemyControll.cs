using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControll : MonoBehaviour {

    public enum CurrentState {
        Idle, Trace, Attack, Dead
    };
    public CurrentState curState = CurrentState.Idle;

    private Transform _transform;
    private Transform playerTransform;
    private NavMeshAgent nvAgent;
    private Animator animator;
    

    //추적 거리
    public float traceDist = 3.5f;

    //공격 거리
    public float attackDist = 1.2f;

    //사망 확인
    private bool isDead = false;

	// Use this for initialization
	void Start () {
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //추적 대상의 위치를 성정하면 바로 추적
        //nvAgent.destination = playerTransform.position;

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
	}

    IEnumerator CheckState() {
        while (!isDead) {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(playerTransform.position, _transform.position);

            if (dist < attackDist) {
                curState = CurrentState.Attack;
            }
            else if (dist < traceDist) {
                curState = CurrentState.Trace;
            }
            else {
                curState = CurrentState.Idle;
            }
        }
    }

    IEnumerator CheckStateForAction() {
        
        while (!isDead) {
            switch (curState) {
                case CurrentState.Idle:
                    nvAgent.Stop();
                    animator.SetBool("isTrace", false);
                    break;
                case CurrentState.Trace:
                    nvAgent.destination = playerTransform.position*0.8f;
                    nvAgent.Resume();
                    animator.SetBool("isTrace", true);
                    break;
                case CurrentState.Attack:
                    animator.SetTrigger("monAttack");
                    break;
           }
            
            yield return null;
        }
    }

}
