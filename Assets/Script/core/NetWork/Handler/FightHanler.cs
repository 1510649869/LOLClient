using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.dto.fight;
using UnityEngine.UI;
public class FightHanler : IHandler {
    public override int type {
        get { return Protocol.TYPE_FIGHT; }
    }
    public FightRoomModel ROOM;//保存战斗房间的所以信息;
    public void applyEnter() {
        this.Write((byte)type, 0, FightProtocol.ENTER_CREQ, null);
    }
    public void applyMove(MoveDTO value) {
        this.Write((byte)type, 0, FightProtocol.MOVE_CREQ, value);
    }
    public void applyAttack(int id) {
        this.Write((byte)type, 0, FightProtocol.ATTACK_CREQ, id);
    }
    public void applyUpSkill(int value) {
        this.Write((byte)type, 0, FightProtocol.SKILL_UP_CREQ, value);
    }
    public override void MessageReceive(SocketModel model) {
       switch(model.Command)
       {
           case FightProtocol.START_BRO:
               start(model.GetMessage<FightRoomModel>());
               break;
           case FightProtocol.MOVE_BRO:
               move(model.GetMessage<MoveDTO>());
               break;
           case FightProtocol.ATTACK_BRO:
               attack(model.GetMessage<AttackDTO>());
               break;
           case FightProtocol.DAMAGE_BRO:
               damage(model.GetMessage<DamageDTO>());
               break;
           case FightProtocol.SKILL_UP_SRES:
               skillUP(model.GetMessage<FightSkill>());
               break;
           default:
               break;
       }
    }
    void skillUP(FightSkill skill) {
        FightScene._instance.skillUP(skill);
    }
    void damage(DamageDTO value) {
        FightScene._instance.damage(value);
    }
    void attack(AttackDTO dto) {
        FightScene._instance.attack(dto);
    }
    void move(MoveDTO value) {
        Vector3 target = new Vector3(value.x, value.y, value.z);
        FightScene._instance.move(value.userId, target);
    }
    void start(FightRoomModel model) {
        FightScene._instance.start(model);
    }

}
