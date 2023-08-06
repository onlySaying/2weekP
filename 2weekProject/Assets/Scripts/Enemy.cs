using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        ready, move, attack, damaged, death, back, patrol
    }
    //state 0 : ready, 1: move, 2 : attack, 3 : damaged, 4 : death
    EnemyState state;

    Transform player;

    float findDistance = 8f;
    float attackDistance = 2f;
    float moveDistance = 10f;

    float moveSpeed = 3f;

    [SerializeField]Slider hpUI;
    float currTime = 0f;
    float attackDelayTime = 0.4f;
    float damagedDelayTime = 2f;
    float dieDelayTime = 1f;
    float idleDelayTime = 3f;

    Vector3 originPos;

    float curHP = 0;
    float maxHP = 3;

    NavMeshAgent navi;
    //경로를 나타낼 변수
    LineRenderer lr;

    [SerializeField]Animator anim;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        originPos= transform.position;
        curHP = maxHP;

        //네비메시 에이전트 콤포넌트 가져오기
        navi = GetComponent<NavMeshAgent>();
        //라인렌더러 가져오기
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    { 
        CheckState();
    }

    void CheckState()
    {
        if (state == EnemyState.ready)
        {
            UpadateReady();
        }
        else if (state == EnemyState.move)
        {
            UpdateMove();
        }
        else if (state == EnemyState.attack)
        {
            UpdateAttack();
        }
        else if (state == EnemyState.damaged)
        {
            //UpdateDamaged();
        }
        else if (state == EnemyState.death)
        {
            UpdateDeath();
        }
        else if (state == EnemyState.back)
        {
            UpdateBack();
        }
        else if(state == EnemyState.patrol)
        {
            UpdatePatrol();
        }
    }

    void UpadateReady()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if(dist < findDistance) 
        {
            ChangeState(EnemyState.move);
        }
        else
        {
            //시간을 흐르게 한다
            currTime += Time.deltaTime;
            if(currTime > idleDelayTime)
            {
                ChangeState(EnemyState.patrol);
                currTime = 0;
            }
        }
    }

    void UpdatePatrol()
    { 
        if(navi.remainingDistance < 0.1f)
        {
            ChangeState(EnemyState.ready);
        }
    }
    void UpdateMove()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < attackDistance)
        {
            ResetNavi();
            ChangeState(EnemyState.attack);
           
        }
        else if(dist > findDistance)
        {
            ChangeState(EnemyState.back);
        }
        else 
        {
            /*   Vector3 dir = player.position - transform.position;
               dir.Normalize();
               transform.position += dir * moveSpeed * Time.deltaTime;
            */
            SetDestination(player.position);
        }
    }

    void UpdateAttack()
    {
        float distByOrigin = Vector3.Distance(transform.position, originPos);

        if(distByOrigin > moveDistance)
        {
            state = EnemyState.back;
            return;
        }
        float dist = Vector3.Distance(transform.position, player.position);
        if(dist < attackDistance)
        {
            currTime += Time.deltaTime;
            if (currTime > attackDelayTime)
            {
                anim.SetTrigger("Attack");
                PlayerMove pm = player.GetComponent<PlayerMove>();
                pm.OnDamaged();
                currTime = 0;
            }
        }
        else
        {
           ChangeState(EnemyState.move);
            currTime= 0;
        }
       
    }

    public void OnDamamaged()
    {
        if (state == EnemyState.death) { return; }
        curHP--;
        StopAllCoroutines();
        ResetNavi();

        float ratio = curHP / maxHP;
        hpUI.value = ratio;
        if (curHP > 0)
        {
            ChangeState(EnemyState.damaged);
            StartCoroutine(DamagedProcess());
        }
        else 
        {
            ChangeState(EnemyState.death);
        }
    }

    void UpdateDamaged()
    {
        currTime += Time.deltaTime;
        if(currTime > damagedDelayTime)
        {
            state = EnemyState.ready;
            currTime= 0;
        }
    }

    IEnumerator DamagedProcess()
    {
        yield return new WaitForSeconds(damagedDelayTime);
        state = EnemyState.ready;
        anim.SetTrigger("Idle");

    }
    void UpdateDeath() 
    {
        currTime += Time.deltaTime;
        if(currTime > dieDelayTime)
        {
            navi.enabled = false;
            GetComponent<CapsuleCollider>().enabled= false;
            transform.position = Vector3.down * moveSpeed * Time.deltaTime;
            if (transform.position.y < -2)
            {
                Destroy(gameObject);
            }
        }
    }

    void UpdateBack()
    {
        float dist = Vector3.Distance(originPos, player.position);
        if(dist < 20f) 
        {
            ResetNavi();
        }
        else 
        {
            /*Vector3 dir = originPos - transform.position;
            dir.Normalize();
            transform.position += dir * moveSpeed * Time.deltaTime;*/
            SetDestination(originPos);
        }
    }

    void SetDestination(Vector3 pos)
    {
        navi.destination = pos;
        // 목적지 경로 그리기
        lr.positionCount = navi.path.corners.Length;
        lr.SetPositions(navi.path.corners);
    }

    void ResetNavi()
    {
        navi.isStopped = true;
        navi.ResetPath();
        lr.positionCount = 0;
    }

    void ChangeState(EnemyState s)
    {
        state = s;
        if(state == EnemyState.ready) 
        {
            ResetNavi();
            ChangeState(EnemyState.ready);
            anim.SetTrigger("Idle");
        }
        else if(state == EnemyState.move) 
        {
            anim.SetTrigger("Move");
        }
        else if(state == EnemyState.attack)
        {
            state = EnemyState.attack;
        }
        else if(state == EnemyState.damaged)
        {
            anim.SetTrigger("Damaged");
        }
        else if(state == EnemyState.back)
        {
            state = EnemyState.back;
        }
        else if(state == EnemyState.death) 
        {
            anim.SetTrigger("Dead");
        }
        else if(state == EnemyState.patrol)
        {
            //만약 시간이 지나면 랜덤한 값을 구한뒤 랜덤한 위치를 patrol 한다
            //랜덤한 x,z 값을 구한다(-20, 20) 사이
            float randX = Random.Range(-20f, 20f);
            float randZ = Random.Range(-20f, 20f);
            Vector3 pos = originPos;
            pos.x += randX;
            pos.z += randZ;
            SetDestination(pos);
            state = EnemyState.patrol;
            anim.SetTrigger("Move");
        }
    }

}
