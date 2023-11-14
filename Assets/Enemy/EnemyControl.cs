using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControl : EnemyFSM
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    //Current state that the NPC is reaching
    public FSMState curState;

    //Speed of the enemy
    public float curSpeed;

    //Whether the NPC is destroyed or not
    private bool bDead;
    private int health;

    public GameObject player;
    protected float distance;

    protected override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        //anim.SetBool("isRunning", true);
        curState = FSMState.Patrol;
        curSpeed = 5.0f;
        bDead = false;
        //elapsedTime = 0.0f;
        health = 100;
       
    }

    //Update each frame
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        //Update the time
        // elapsedTime += Time.deltaTime;

        //Go to dead state is no health left
        if (health <= 0)
            curState = FSMState.Dead;
    }

    protected void UpdatePatrolState()
    {
        Debug.Log("In patrol state");
        distance = playerDistance();
        Debug.Log("Dist is " + distance);
        if (distance > 10.0f)
        {
            curState = FSMState.Patrol;
            Vector2 point = currentPoint.position - transform.position;
            rb.velocity = new Vector2(curSpeed, 0);
            if (transform.position.x == pointB.transform.position.x)
            {
                flip();
                rb.velocity = new Vector2(curSpeed, 0);
            }
            else if(transform.position.x == pointA.transform.position.x)
            {
                flip();
                rb.velocity = new Vector2(-curSpeed, 0);
            }


        }
       // else if (distance < 10.0f && distance >= 5.0f)
        //    curState = FSMState.Chase;
        //else if (distance < 5.0f)
         //   curState = FSMState.Attack;


        //transform.position += transform.right * curSpeed * Time.deltaTime;
    }

    protected void UpdateChaseState()
    {
        distance = playerDistance();
        if (distance > 10.0f)
            curState = FSMState.Patrol;
        else if (distance < 5.0f)
            curState = FSMState.Chase;


    }
    protected void UpdateAttackState()
    {
        distance = playerDistance();
    }
    protected void UpdateDeadState()
    {
        curState = FSMState.Dead;

    }

    protected float playerDistance()
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        return dist;
    }
    protected void jump()
    {

    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = -1;
    }
}
