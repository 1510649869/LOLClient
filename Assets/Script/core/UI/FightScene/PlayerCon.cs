using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;

public class PlayerCon : MonoBehaviour {
    [HideInInspector]
    public FightPlayerModel data;

    protected Animator anim;

    protected NavMeshAgent agent;

    protected int state=AnimState.IDLE;
    void Start() {
        anim = transform.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void death() {
        //agent.CompleteOffMeshLink();
        agent.enabled = false;//不可再寻路
        transform.collider.enabled = false;
        state = AnimState.DEAED;
        anim.SetBool("death",true);//英雄死亡
    }
    public void move(Vector3 target) {
        agent.enabled = true;
        agent.ResetPath();
        agent.SetDestination(target);
        anim.SetBool("move", true);
        state = AnimState.RUN;
    }
    void Update() {
        //寻路完毕
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0 && !agent.pathPending)
        {
            state = AnimState.IDLE;
            anim.SetBool("move", false);
        }
        else
        {
            if (agent.isOnOffMeshLink)
            {
                agent.CompleteOffMeshLink();
            }
        }
    }
    public virtual void attacked() {

    }
    /// <summary>
    /// 申请攻击，攻击的目标
    /// </summary>
    /// <param name="target">攻击目标</param>
    public virtual void attack(Transform[] target) {

    }
    /// <summary>
    /// 服务器申请释放技能
    /// </summary>
    /// <param name="code">技能唯一标识符</param>
    /// <param name="target"></param>
    public virtual void skill(int code, Transform[] target, Vector3 ps) {

    }
    public virtual void skilled() {

    }
    /// <summary>
    /// 本地操作申请释放技能
    /// </summary>
    /// <param name="code"></param>
    /// <param name="target"></param>
    public virtual void baseSkill(int code, Transform[] target, Vector3 ps) {

    }
    public void init(FightPlayerModel data, int myTeam) {
        this.data = data;
        if (data.team != myTeam)
        {
            gameObject.layer = LayerMask.NameToLayer("enemy");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("friend");
        }
    }
}
