using UnityEngine;
using System.Collections;
using GameProtocol;
using GameProtocol.dto;

public class UserHandler : IHandler {
    public override int type {
        get {
            return Protocol.TYPE_USER;
        }
    }
    /// <summary>
    /// 向服务器申请获取用户信息
    /// </summary>
    public void applyGet() {
        this.Write((byte)type, 0, UserProtocol.INFO_CREQ, null);
    }
    /// <summary>
    /// 像服务器申请创建用户
    /// </summary>
    /// <param name="name"></param>
    public void applyCreate(string name) {
        this.Write((byte)type, 0, UserProtocol.CREAT_CREQ, name);
    }
    public void applyonline() {
        this.Write((byte)type, 0, UserProtocol.ONLINE_CREQ, null);
    }
    public override void MessageReceive(SocketModel model) {
       switch(model.Command)
       {
           case UserProtocol.CREAT_SRES:
               create((bool)model.Message);
               break;
           case UserProtocol.INFO_SRES:
               get((UserModel)model.Message);
               break;
           case UserProtocol.ONLINE_SRES:
               online((UserModel)model.Message);
               break;
       }
    }
    void create(bool message) {
        if(message)
        {
            OnCreate();//创建成功
        }else
        {
            PopWindowManager.AddMsg("创建角色失败");
        }
    }
    void get(UserModel user) {
        Debug.Log(user);
        if (user == null)
        {
            GameData.user = null;//创建角
        }
        else
            GameData.user = user;//不创建角色
        OnGet();
    }
    void online(UserModel user) {
        if (user == null)
        {
            PopWindowManager.AddMsg("上线失败");
        }
        else
        {
            PopWindowManager.AddMsg("用户上线成功");
            Online(user);
        }
    }
    public event OnGetEvent OnGet;
    public event OnCreateEvent OnCreate;
    public event OnLineEvent Online;
  
}