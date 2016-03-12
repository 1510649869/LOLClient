using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Login : BaseUI {
    private InputField accInput;
    private InputField passInput;
    private LoginHandler loginhandler;
    private Toggle toggle;
    protected override void OnAwake() {
        accInput = transform.Find("account/InputField").GetComponent<InputField>();
        passInput = transform.Find("password/InputField").GetComponent<InputField>();
        toggle = transform.Find("Toggle").GetComponent<Toggle>();
        loginhandler = GameObject.Find("NetWork").GetComponent<LoginHandler>();
        loginhandler.OnLogin += this.onLogin;
    }
    protected override void OnStart() {
        base.OnStart();
        NetIO io = NetIO.Instance;
    }
    protected override void OnClicked(GameObject _go) {
        if (_go.name.Equals("btnlogin"))
        {
            if(accInput.text.Length==0||passInput.text.Length==0)
            {
                PopWindowManager.AddMsg("账号密码不能为空");
                return;
            }
            else if(accInput.text.Length<2)
            {
                PopWindowManager.AddMsg("账号至少为2个字符");
                return;
            }
            else if (passInput.text.Length < 4)
            {
                PopWindowManager.AddMsg("密码至少为4个字符");
                return;
            }
            GameProtocol.dto.AccountModel model = new GameProtocol.dto.AccountModel();
            model.account = accInput.text;
            model.password = passInput.text;
            //登陆的时候像服务器发送消息
            loginhandler.login(model);
            if (toggle.isOn)
            {
                //保存账号密码
            }
            PopWindowManager.AddMsg("正在向服务器请求登陆...");       
        }
    }
    public void LoginShow()
    {
        base.Show();
    }
    public void LoginHide() {
        base.Hide();
    }
    protected override void OnHideFront() {
        accInput.text = "";
        passInput.text = "";
    }
    protected override void OnLogin(int value) {
        //页面跳转
        Application.LoadLevel(1);
    }
    protected override void OnDestroyFront() {
        loginhandler.OnLogin -= this.onLogin;
        loginhandler.OnReg -= this.onReg;
        base.OnDestroyFront();
    }
    public override State mState {
        get { return State.login; }
    }
}
