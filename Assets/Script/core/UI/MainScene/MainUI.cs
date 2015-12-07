using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameProtocol.dto;

public class MainUI : MonoBehaviour {
   [SerializeField]
    private Transform createPanel;
    [SerializeField]
    private Text infoText;
    [SerializeField]
    private Transform timeDialogUI;
    private UserHandler userhandler;
    private MatchHandler matchHandler;
    void Awake() {
        userhandler = GameObject.Find("NetWork").AddComponent<UserHandler>();
        matchHandler = GameObject.Find("NetWork").AddComponent<MatchHandler>();
        if (userhandler == null || matchHandler == null)
            return;
        userhandler.OnGet += this.OnGet;
        userhandler.OnCreate += this.OnCreate;
        userhandler.Online += this.Online;
        matchHandler.OnGetMatch += this.OnGetMatch;
        userhandler.applyGet();
    }
    void Start() {
        transform.Find("btnqueue").GetComponent<Button>().onClick.AddListener(delegate()
        {
            onClick();
        });
    }

    private void onClick() {
        matchHandler.EnterMatch();//向服务器发起匹配对手请求；
    }
    void OnGet() {
        //显示用户信息
        if (GameData.user == null)
        {
            createPanel.gameObject.SetActive(true);
        }else
        {
            userhandler.applyonline();
        }
    }
    void OnCreate() {
        createPanel.gameObject.SetActive(false);
        userhandler.applyonline();//创建成功后申请上线
    }
    private void Online(GameProtocol.dto.UserModel model) {
        //显示面板信息
        infoText.text = model.name + "  LV " + model.level;
    }
    void OnGetMatch(UserModel model) {
        //匹配成功后返回
    }
}
