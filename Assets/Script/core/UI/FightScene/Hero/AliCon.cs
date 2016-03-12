using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;
using GameProtocol;

public class AliCon : PlayerCon {
    private Transform[] list;

    /// <summary>
    /// 设置播放动画
    /// </summary>
    /// <param name="target"></param>
    public override void attack(Transform[] target) {
        //agent.CompleteOffMeshLink();
        if (state == AnimState.RUN)
        {
            agent.CompleteOffMeshLink();
            agent.ResetPath();
        }
        agent.enabled = false;
        this.list = target;
        transform.LookAt(target[0]);
        state = AnimState.ATTACK;
        anim.SetBool("move", false);
        anim.SetTrigger("Attack");
        
    }
    /// <summary>
    /// 动画事件添加
    /// </summary>
    public override void attacked() {
        //用于显示特效
        state = AnimState.IDLE;
        //遍历攻击目标
        foreach(Transform item in list)
        {
            //向服务器发送伤害
            DamageDTO dto = new DamageDTO();
            dto.userId = data.id;
            dto.skill = -1;
            dto.target = new int[][] { new int[] { item.GetComponent<PlayerCon>().data.id } };
            this.Write(Protocol.TYPE_FIGHT, 0, FightProtocol.DAMAGE_CREQ, dto);
        }
    }
    /// <summary>
    /// 技能释放完
    /// </summary>
    /// <param name="code"></param>
    /// <param name="target"></param>
    /// <param name="ps"></param>
    public override void baseSkill(int code, Transform[] target, Vector3 ps) {
        //base.baseSkill(code, target, ps);
    }

    public override void skill(int code, Transform[] target, Vector3 ps) {
        //base.skill(code, target, ps);
    }
    public override void skilled() {
        //base.skilled();
    }
    public void OnMouseEnter() {
        
    }
    public void OnMouseExit() {

    }
}
