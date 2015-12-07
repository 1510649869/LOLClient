using UnityEngine;
using System.Collections;

public class TopBtns : BaseUI {
    public Transform regPanel;
    public Transform loginPanel;
    private RegUI regUI;
    private Login loginUI;
    protected override void OnStart() {
        base.OnStart();
        regUI = regPanel.GetComponent<RegUI>();
    }
    protected override void OnClicked(GameObject _go) {
        //base.OnClicked(_go);
        if(_go.name.Equals("button_zczh"))
        {
            //点击了注册账号
            base.SetNewState(State.reg);
        }
    }
    public override State mState {
        get { return State.none; }
    }
}
