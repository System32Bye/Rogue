using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{

    public int playerDamage;


    private Animator animator;
    private Transform target;
    private bool skipMove;


    protected override void Start()
    {
        GameManager.instance.AddEnemyToLise(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }


    protected override void AttemptMove<T>(int xDir, int zDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, zDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int zDir = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            zDir = target.position.z > transform.position.z ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, zDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAttack");
        hitPlayer.LoseFood(playerDamage);
    }
}
