using UnityEngine;
using System.Collections;

public class LoginHandler :IHandler{
    public override int type {
        get {
            return GameProtocol.Protocol.TYPE_LOGIN;
        }
    }
    public void login(GameProtocol.dto.AccountModel dto) {
        //NetIO.Instance.write((byte)type, 0, GameProtocol.LoginProtocol.LOGIN_CREQ, dto);
        this.Write((byte)type, 0, GameProtocol.LoginProtocol.LOGIN_CREQ, dto);
    }
    public void register(GameProtocol.dto.AccountModel dto) {
       // NetIO.Instance.write((byte)type, 0, GameProtocol.LoginProtocol.REG_CREQ, dto);
        this.Write((byte)type, 0, GameProtocol.LoginProtocol.REG_CREQ, dto);
        
    }
    public override void MessageReceive(SocketModel model) {
        switch(model.Command)
        {
            case GameProtocol.LoginProtocol.LOGIN_SRES:
                login((int)model.Message);
                break;
            case GameProtocol.LoginProtocol.REG_SRES:
                reg((int)model.Message);
                break;
        }
    }
    void login(int value) {
        switch(value)
        {
            case 0:
                //登陆成功
                if(OnLogin!=null)
                {
                    OnLogin(value);
                }
                break;
            case 1:
                //账号不存在
                PopWindowManager.AddMsg("账号不存在");
                break;
            case 2:
                //密码错误
                PopWindowManager.AddMsg("密码错误，从新输入");
                break;
            case 3:

                //账号已经登陆
                break;
        }
    }
    void reg(int value) {
        switch (value)
        {
            case 0:
                //注册成功
                if (OnReg != null)
                    OnReg(value);
                break;
            case 1:
                //账号存在
                PopWindowManager.AddMsg("账号存在");
                break;
            case 2:
                //账号不合法
                PopWindowManager.AddMsg("账号不合法");
                break;
            case 3:
                //密码不合法
                PopWindowManager.AddMsg("密码不合法");
                break;
        }
    }
    public event OnLoginEvent OnLogin;
    public event OnRegEvent OnReg;
}
