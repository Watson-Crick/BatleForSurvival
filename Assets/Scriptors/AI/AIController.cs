using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIController : MonoBehaviour {

    private Transform m_Transform;
    private Transform player_Transform;
    private PlayerController playerController;
    private Transform effectSetTransform;
    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;
    private GameObject fleshEffect;
    private AIManager parent_AIManager;

    public Transform M_Transform { get { return m_Transform; } }
    public NavMeshAgent M_NavMeshAgent { get { return m_NavMeshAgent; } }
    public Animator M_Animator { get { return m_Animator; } }

    private Vector3 startPos;           //起始位置
    private Transform[] patrolPos;
    private AIState aiState;
    private Vector3 targetPos;

    public Vector3 StartPos { get { return startPos; } }
    public Transform[] PatrolPos { set { patrolPos = value; } get { return patrolPos; } }
    public AIState AIState { set { aiState = value; } get { return aiState; } }
    public AIManager Parent_AIManager { set { parent_AIManager = value; } }

    private int life;
    private int demage;

    public int Life { set { life = value; } get { return life; } }
    public int Demage { set { demage = value; } get { return demage; } }

    private int index;

	protected virtual void Awake () {
        FindAndInit();
	}
	
	void Update () {
        BehavioralLogic();
	}

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        player_Transform = GameObject.Find("FPSController").transform;
        playerController = player_Transform.GetComponent<PlayerController>();
        effectSetTransform = GameObject.Find("TempObject/Effect_Flesh_Set").transform;
        m_NavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        m_Animator = gameObject.GetComponent<Animator>();
        fleshEffect = Resources.Load<GameObject>("Effects/Gun/Bullet Impact FX_Flesh");

        startPos = m_Transform.position;
        targetPos = startPos;

        index = 0;
    }

    /// <summary>
    /// AI行为逻辑
    /// </summary>
    private void BehavioralLogic()
    {
        if (aiState != AIState.DEATH)
        {
            Patrol();
            AIFollowPlayer();
            AIAttackPlayer();
        }
    }

    /// <summary>
    /// 巡逻行为
    /// </summary>
    private void Patrol()
    {
        //如果在行走或默认状态下,则另其巡逻
        if (aiState == AIState.WALK || aiState == AIState.IDLE)
        {
            targetPos = patrolPos[index % patrolPos.Length].position;
            m_NavMeshAgent.SetDestination(targetPos);
            if (Vector3.Distance(m_Transform.position, targetPos) < 3)
            {
                index++;
                ToggleAIState(AIState.IDLE);
            }
            else
            {
                ToggleAIState(AIState.WALK);
            }
        }
    }

    /// <summary>
    /// 跟随主角行为
    /// </summary>
    private void AIFollowPlayer()
    {
        if (aiState != AIState.ENTERATTACK)
        {
            if (Vector3.Distance(m_Transform.position, player_Transform.position) < 15)
            {
                ToggleAIState(AIState.ENTERRUN);
            }
            else
            {
                ToggleAIState(AIState.EXITRUN);
            }
        }
    }

    /// <summary>
    /// 攻击主角行为
    /// </summary>
    private void AIAttackPlayer()
    {
        if (aiState == AIState.ENTERRUN || aiState == AIState.ENTERATTACK)
        {
            if (playerController.IfDeath == false)
            {
                if (Vector3.Distance(m_Transform.position, player_Transform.position) < 2)
                {
                    ToggleAIState(AIState.ENTERATTACK);
                }
                else
                {
                    ToggleAIState(AIState.EXITATTACK);
                }
            }else
            {
                ToggleAIState(AIState.EXITATTACK);
                ToggleAIState(AIState.EXITRUN);
                Patrol();
            }

        }
    }

    /// <summary>
    /// 管理当前AI状态
    /// </summary>
    public void ToggleAIState(AIState state)
    {
        switch(state)
        {
            case AIState.IDLE:
                IdleState();
                break;
            case AIState.WALK:
                WalkState();
                break;
            case AIState.ENTERRUN:
                EnterRunState();
                break;
            case AIState.EXITRUN:
                ExitRunState();
                break;
            case AIState.ENTERATTACK:
                EnterAttackState();
                break;
            case AIState.EXITATTACK:
                ExitAttackState();
                break;
            case AIState.DEATH:
                DeathState();
                break;
        }
    }

    /// <summary>
    /// 行走状态
    /// </summary>
    private void WalkState()
    {
        m_Animator.SetBool("Walk", true);
        aiState = AIState.WALK;
    }

    /// <summary>
    /// 默认状态
    /// </summary>
    private void IdleState()
    {
        m_Animator.SetBool("Walk", false);
        aiState = AIState.IDLE;
    }

    /// <summary>
    /// 开始跑步状态
    /// </summary>
    private void EnterRunState()
    {
        m_Animator.SetBool("Run", true);
        m_NavMeshAgent.speed = 2;
        m_NavMeshAgent.SetDestination(player_Transform.position);
        aiState = AIState.ENTERRUN;
    }

    /// <summary>
    /// 结束跑步状态
    /// </summary>
    private void ExitRunState()
    {
        m_Animator.SetBool("Run", false);
        m_NavMeshAgent.speed = 0.8f;
        m_NavMeshAgent.SetDestination(targetPos);
        ToggleAIState(AIState.WALK);
    }

    /// <summary>
    /// 开始攻击状态
    /// </summary>
    private void EnterAttackState()
    {
        m_Animator.SetBool("Attack", true);
        m_NavMeshAgent.enabled = false;
        aiState = AIState.ENTERATTACK;
    }

    /// <summary>
    /// 结束攻击状态
    /// </summary>
    private void ExitAttackState()
    {
        m_Animator.SetBool("Attack", false);
        m_NavMeshAgent.enabled = true;
        ToggleAIState(AIState.ENTERRUN);
    }

    /// <summary>
    /// 死亡状态
    /// </summary>
    protected abstract void DeathState();

    /// <summary>
    /// 角色死亡
    /// </summary>
    protected IEnumerator Dead()
    {
        PlayDeathAudio();
        yield return new WaitForSeconds(3);
        SendMessageUpwards("AIDeath", gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// 头部受击
    /// </summary>
    public void HitHard(int att)
    {
        if (life > att)
        {
            life -= att;
        }else
        {
            life = 0;
            ToggleAIState(AIState.DEATH);
        }
        m_Animator.SetTrigger("HitHard");
        PlayHitAudio();
    }

    /// <summary>
    /// 其他部位受击
    /// </summary>
    public void HitNormal(int att)
    {
        if (life > att)
        {
            life -= att;
        }
        else
        {
            life = 0;
            ToggleAIState(AIState.DEATH);
        }
        m_Animator.SetTrigger("HitNormal");
        PlayHitAudio();
    }

    protected abstract void PlayAttackAudio();

    protected abstract void PlayDeathAudio();

    protected abstract void PlayHitAudio();

    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="hit"></param>
    public void PlayEffect(RaycastHit hit)
    {
        GameObject effect;
       if (parent_AIManager.Pool.Data())
        {
            effect = parent_AIManager.Pool.GetObject();
            effect.transform.position = hit.point;
            effect.transform.rotation = Quaternion.LookRotation(hit.normal);
        }
        else
        {
            effect = Instantiate(fleshEffect, hit.point, Quaternion.LookRotation(hit.normal), effectSetTransform);
        }
        StartCoroutine(DelayAddObjectPool(effect, 3));
    }

    /// <summary>
    /// 延迟一段时间后将物体加入对象池
    /// </summary>
    IEnumerator DelayAddObjectPool(GameObject temp, float time)
    {
        yield return new WaitForSeconds(time);
        parent_AIManager.Pool.AddObject(temp);
    }

    /// <summary>
    /// 攻击主角事件
    /// </summary>
    private void AttackPlayer()
    {
        PlayAttackAudio();
        playerController.CutPlayerLife(demage);
    }
}
