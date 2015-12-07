using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopWindowUI : BaseUI {
    private Text msgText;
    public void Show(string msg) {
        base.Show(msg);
    }
    protected override void OnStart() {
        base.OnStart();
        msgText = transform.Find("message").GetComponent<Text>();
        this.gameObject.SetActive(false);
    }
    protected override void OnClicked(GameObject _go) {
        //base.OnClicked(_go);
        if (_go.name.Equals("btnsure"))
        {
            base.Hide();
        }
    }
    protected override void OnShowFront(object parames) {
        msgText.text = parames.ToString();//显示之后
    }
    //在object显示之后 显示类容
    protected override void OnShowEnd(object parames) {
        //Debug.Log("其他信息");
    }
    protected override void OnHideFront() {
        msgText.text = "";
    }

    public override State mState {
        get { return State.none; }
    }
}
