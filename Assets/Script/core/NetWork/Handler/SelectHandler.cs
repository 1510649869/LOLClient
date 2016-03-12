using UnityEngine;
using System.Collections;
using GameProtocol;
using GameProtocol.dto;
public class SelectHandler :IHandler {
    public override int type {
        get { return GameProtocol.Protocol.TYPE_SELECT; }
    }
    public SelectRoomDTO ROOM=null;

    //选择模块向服务器申请进入
    public void applyEnter() {
        this.Write((byte)type, 0, SelectProtocol.ENTER_CREQ, null);
    }
    public void applySelect(int hero) {
        this.Write((byte)type, 0, SelectProtocol.SELECT_CREQ, hero);
    }
    public void pubtalk(string message) {
        this.Write((byte)type, 0, SelectProtocol.TALK_CREQ, message);
    }
    public void applyReady() {
        this.Write((byte)type, 0, SelectProtocol.READY_CREQ, null);
    }
    //接收服务器消息
    public override void MessageReceive(SocketModel model) {
        switch(model.Command)
        {
            case SelectProtocol.DESTORY_BRO:
                Application.LoadLevel(1);
                break;
            case SelectProtocol.ENTER_SRES:
                enter(model.GetMessage<SelectRoomDTO>());
                break;
            case SelectProtocol.ENTER_EXBRO:
                enter(model.GetMessage<int>());
                break;
            case SelectProtocol.SELECT_SRES:
                PopWindowManager.AddMsg("选择英雄失败");
                break;
            case SelectProtocol.SELECT_BRO:
                select(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.TALK_BRO:
                talk(model.GetMessage<string>());
                break;
            case SelectProtocol.READY_BRO:
                ready(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.FIGHT_BRO:
                Application.LoadLevel(3);
                break;
        }
    }
    void ready(SelectModel model) {
        //如果是自己准备
        if(model.userID==GameData.user.id)
        {
            //
            selected();
        }
        if (ROOM == null)
            return;
        int id = model.userID;
        //用户在队伍1;
        foreach (SelectModel item in ROOM.teamOne)
        {
            if (id == item.userID)
            {
                item.hero = model.hero;
                item.ready = model.ready;
                break;
            }
        }
        foreach (SelectModel item in ROOM.teamTwo)
        {
            if (id == item.userID)
            {
                item.hero = model.hero;
                item.ready = model.ready;
                break;
            }
        }
        //TODO 刷新UI界面
        if(refeshSelectUI!=null)
           refeshSelectUI(ROOM);
    }
    void select(SelectModel model) {
        if (ROOM == null)
            return;
        int id = model.userID;
        foreach(SelectModel item in ROOM.teamOne)
        {
            if(id==item.userID)
            {
                item.hero = model.hero;
                break;
            }
        }
        foreach (SelectModel item in ROOM.teamTwo)
        {
            if (id == item.userID)
            {
                item.hero = model.hero;
                break;
            }
        }
        //TODO 刷新UI界面
        if (refeshSelectUI != null)
            refeshSelectUI(ROOM);
    }
    void talk(string message) {
        //UI面板添加聊天消息;
        if(Talk!=null)
        {
            Talk(message);
        }
    }

    //处理自己进入
    void enter(SelectRoomDTO room) {
        ROOM = room;
        //处理UI
        if (refeshSelectUI != null)
            refeshSelectUI(ROOM);
    }

    //处理其他玩家进入
    void enter(int id) {
        if (ROOM == null)
            return;
        foreach (SelectModel item in ROOM.teamOne)
        {
            if (id == item.userID)
            {
                item.enter = true;
                break;
            }
        }
        foreach (SelectModel item in ROOM.teamTwo)
        {
            if (id == item.userID)
            {
                item.enter = true;
                break;
            }
        }
        //TODO 刷新UI
        if (refeshSelectUI != null)
            refeshSelectUI(ROOM);
    }
    public RefeshSelectUIEvent refeshSelectUI;
    public selectedEvent selected;
    public TalkEvent Talk;
}