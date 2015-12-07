using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RegUI : BaseUI {
    private InputField mAccInput;
    private InputField mPassInput;
    private InputField mPassInput0;
    private LoginHandler loginhandler;
    protected override void OnStart() {
        base.OnStart();
        loginhandler = GameObject.Find("NetWork").GetComponent<LoginHandler>();
        mAccInput = transform.Find("regaccount/InputField").GetComponent<InputField>();
        mPassInput = transform.Find("regpassword/InputField").GetComponent<InputField>();
        mPassInput0 = transform.Find("regpassword0/InputField").GetComponent<InputField>();
        loginhandler.OnReg += this.onReg;
        gameObject.SetActive(false);
    }
    protected override void OnClicked(GameObject _go) {
        if(_go.name.Equals("BtnReg"))
        {
            if(mAccInput.text.Length==0||mPassInput.text.Length==0||mPassInput0.text.Length==0)
            {
                PopWindowManager.AddMsg("输入信息不能为空");
            }
            else if(mAccInput.text.Length<2||mPassInput.text.Length<4)
            {
                PopWindowManager.AddMsg("注册信息不合法");
            }
            else if(!mPassInput.text.Equals(mPassInput0.text))
            {
                PopWindowManager.AddMsg("密码不匹配");
            }
            GameProtocol.dto.AccountModel model = new GameProtocol.dto.AccountModel();
            model.account = mAccInput.text;
            model.password = mPassInput.text;
            loginhandler.register(model);
            PopWindowManager.AddMsg("正在向服务器请求注册...");
        }
        else if (_go.name.Equals("BtnCancelReg"))
        {
            //base.Hide();
            base.SetNewState(State.login);
        }
    }
    public void RegShow() {
        base.Show();
    }
    protected override void OnShowFront(object parames) {
        //界面显示之前需要做的工作
    }
    protected override void OnHideFront() {
        mAccInput.text="";
        mPassInput.text = "";
        mPassInput0.text = "";
    }
    protected override void OnReg(int value) {
        Application.LoadLevel(1);   
    }
    public override State mState {
        get { return State.reg; }
    }
}
