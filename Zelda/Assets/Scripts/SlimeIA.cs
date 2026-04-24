using UnityEngine;
using System.Collections;
public class SlimeIA : MonoBehaviour
{
    private Animator anim;
    public int HP = 3;
    private bool isDie = false;
    public enemyState state;
    private bool isAlert = false;
    public const float idleWaitTime = 2f;
    public const float patrolWaitTime = 4f;
    private int rand;
    void Start()
    {
        anim = GetComponent<Animator>();
        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();
    }

    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    #region 
    void GetHit(int amount)
    {
        if(isDie){return;}
        HP -= amount;
        if(HP > 0)
            anim.SetTrigger("GetHit");
        else
        {
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }
    }
    void StateManager()
    {
        switch(state)
        {
            case enemyState.IDLE:
            break;
            case enemyState.ALERT:
            break;
            case enemyState.EXPLORE:
            break;
            case enemyState.PATROL:
            break;
            case enemyState.FOLLOW:
            break;
            case enemyState.FURY:
            break;
        }
    }
    void ChangeState(enemyState newState)
    {
        StopAllCoroutines();
        isAlert = false;
        switch(newState)
        {
            case enemyState.IDLE:
                StartCoroutine("IDLE");
            break;
            case enemyState.ALERT:
                isAlert = true;
                StartCoroutine("ALERT");
            break;
            case enemyState.PATROL:
                StartCoroutine("PATROL");
            break;
            case enemyState.FOLLOW:
                StartCoroutine("FOLLOW");
                StartCoroutine("ATTACK");
            break;
            case enemyState.FURY:
                
            break;
        }
    }
    int Rand()
    {
        rand = Random.Range(0,100);
        return rand;
    }
    void StayStill(int yes)
    {
        if(Rand() <= yes)
        {
            ChangeState(enemyState.IDLE);
        }
        else
        {
            ChangeState(enemyState.PATROL);
        }
    }
    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(idleWaitTime);
        StayStill(50);
    }
    #endregion
}
