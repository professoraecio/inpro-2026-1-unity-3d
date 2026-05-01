using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class SlimeIA : MonoBehaviour
{

    private GameManager _gm;
    private NavMeshAgent agent;
    private Vector3 destination;
    private int idWayPoint;
    private bool isWalk;

    private Animator anim;
    public int HP = 3;
    private bool isDie = false;
    public enemyState state;
    private bool isAlert = false;
    public const float idleWaitTime = 2f;
    //public const float patrolWaitTime = 4f;
    private int rand;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = transform.position;
        anim = GetComponent<Animator>();
        _gm = FindFirstObjectByType(typeof(GameManager)) as GameManager;
        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();
        if(agent.desiredVelocity.magnitude >= 0.1f)
            isWalk = true;
        else
            isWalk = false;
        anim.SetBool("isWalk",isWalk);
        anim.SetBool("isAlert",isAlert);
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
        {
            ChangeState(enemyState.FURY);
            anim.SetTrigger("GetHit");
        }
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
                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;
                StartCoroutine("IDLE");
            break;
            case enemyState.ALERT:
            break;
            case enemyState.EXPLORE:
            break;
            case enemyState.PATROL:
                agent.stoppingDistance = 0;
            break;
            case enemyState.FOLLOW:
            break;
            case enemyState.FURY:
                destination = _gm.player.position;
                agent.destination = destination;
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
                destination = transform.position;
                agent.destination = destination;
                StartCoroutine("IDLE");
            break;
            case enemyState.ALERT:
                isAlert = true;
                agent.stoppingDistance = 0;
                destination = transform.position;
                agent.destination = destination;
                StartCoroutine("ALERT");
            break;
            case enemyState.PATROL:
                idWayPoint = Random.Range(0,_gm.slimeWayPoints.Length);
                destination = _gm.slimeWayPoints[idWayPoint].position;
                agent.destination = destination;
                StartCoroutine("PATROL");
            break;
            case enemyState.FOLLOW:
                StartCoroutine("FOLLOW");
                StartCoroutine("ATTACK");
            break;
            case enemyState.FURY:
                destination = transform.position;
                agent.stoppingDistance = _gm.slimeDistanceToAttack;
                agent.destination = destination;
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
    IEnumerator PATROL()
    {
        yield return new WaitUntil(() => agent.remainingDistance <= 0);
        StayStill(30);
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        bool player = false;
        bool idle = false;
        bool patrol = false;
        if(other.gameObject.tag == "Player")
            player = true;
        if(state == enemyState.IDLE)
            idle = true;
        if(state == enemyState.PATROL)
            patrol = true;
        if(player && (idle || patrol))
            ChangeState(enemyState.ALERT);
    }
}
