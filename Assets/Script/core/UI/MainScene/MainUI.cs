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
    private TimedialogUI timeDialogUI;

    private UserHandler userhandler;
    private MatchHandler matchHandler;

    void Awake() {
        userhandler=GameObject.Find("NetWork").GetComponent<UserHandler>();
        matchHandler = GameObject.Find("NetWork").GetComponent<MatchHandler>();
        if (userhandler == null)
        {
            userhandler = GameObject.Find("NetWork").AddComponent<UserHandler>();
        }
        if(matchHandler==null)
        {
            matchHandler = GameObject.Find("NetWork").AddComponent<MatchHandler>();
        }
        if (userhandler == null || matchHandler == null)
            return;
        userhandler.OnGet = this.OnGet;
        userhandler.OnCreate = this.OnCreate;
        userhandler.Online = this.Online;
        matchHandler.OnLoadSelectSscene = this.OnLoadSelectScene;
        userhandler.applyGet();
    }
    void Start() {
        transform.Find("btnqueue").GetComponent<Button>().onClick.AddListener(delegate()
        {
            onClick();
        });
    }

    private void onClick() {
        matchHandler.EnterMatch();//向服务器发起匹配对手请求;
        timeDialogUI.ShowTimer();
    }
    void OnGet() {
        //显示用户信息
        if (GameData.user == null)
        {
            createPanel.gameObject.SetActive(true);
        }else 
        {
            if(!GameData.isOnline)
            {
                userhandler.applyonline();
                GameData.isOnline = true;
            }
            
        }
    }
    void OnCreate() {
        createPanel.gameObject.SetActive(false);
        userhandler.applyonline();//创建成功后申请上线
    }
    private void Online(GameProtocol.dto.UserModel model) {
        GameData.user = model;
        //显示面板信息..服务器上线成功后返回后进行的执行
        infoText.text = model.name + "  LV " + model.level;
    }
    void OnLoadSelectScene() {
        timeDialogUI.OnHide();
        PopWindowManager.AddMsg("匹配成功,进入选择界面");
        //Application.LoadLevel(2);
        Application.LoadLevelAsync(2);
    }

    public void OnDestroy() {

    }
    
}
