using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public abstract class BaseUI : MonoBehaviour {
    private GameObject _mGameObject;
    List<Button> cacheBtn = new List<Button>();
    public GameObject mGameObject {
        get {
            if (_mGameObject == null)
                _mGameObject = this.gameObject;
            return _mGameObject;
        }
    }
    public abstract State mState{get;}
    //设置状态机
    private Dictionary<State, GameObject> stateDict = new Dictionary<State, GameObject>();
    void Awake() {
        OnAwake();
    }
    void Start() {
        UIInit();
        OnStart();
    }
    void Update() {
        OnUpdate();
    }
    //当前界面删除
    public void OnDestory() {
        OnDestroyFront();
        foreach (var o in cacheBtn)
        {
            Destroy(o.gameObject.GetComponent<EventTriggerListener>());
        }
        cacheBtn.Clear();
        cacheBtn = null;
        OnDestroyEnd();
    }
   protected void UIInit() {
        Button[] btns = transform.GetComponentsInChildren<Button>(true);
        for (int i = 0, len = btns.Length; i < len; i++)
        {
            EventTriggerListener.Get(btns[i].gameObject).onClick = OnClicked;
            cacheBtn.Add(btns[i]);
        }
        StateManager.Instance.AddState(mState,mGameObject);
    }
   protected void Show(object parames = null) {
       OnShowFront(parames);
       mGameObject.SetActive(true);//先显示信息
       OnShowEnd(parames);
   }
   protected void Hide() {
       OnHideFront();
       mGameObject.SetActive(false);
       OnHideEnd();
   }
   protected void onLogin(int value) {
       OnLogin(value);
   }
   protected void onReg(int value) {
       OnReg(value);
   }
   protected void SetNewState(State s) {
       StateManager.Instance.SetNewState(s);
   }
    #region 虚方法
    protected virtual void OnDestroyFront() { }
    protected virtual void OnDestroyEnd() { }
    protected virtual void OnClicked(GameObject _go) {}
    protected virtual void OnInit() {}
    protected virtual void OnAwake() {}
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnShowFront(object parames) {}
    protected virtual void OnShowEnd(object parames) {}
    protected virtual void OnHideFront() {}
    protected virtual void OnHideEnd() {}
    protected virtual void OnLogin(int value) { }
    protected virtual void OnReg(int value) { }
    #endregion
}
