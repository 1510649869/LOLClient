using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum State {
    login,
    reg,
    none
}
public class StateManager : MonoBehaviour {
    private static StateManager _instance;
    public static StateManager Instance {
        get {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                go.name = "StateMachine";
                _instance=go.AddComponent<StateManager>();
            }
            return _instance;
        }
    }
    private State currentState = State.login;//当前状态;
    private State newState=State.login;//下一状态
    private Dictionary<State, GameObject> stateDict = new Dictionary<State, GameObject>();
    public void AddState(State s,GameObject go) {
        if(!stateDict.ContainsKey(s)&&s!=State.none&go!=null)
        {
            stateDict.Add(s,go);//注册状态
        }
    }
    public void SetNewState(State s) {
        newState = s;
    }
    void Update() {
        if(stateDict.Count>0)
        {
            if (currentState != newState)
            {
                if (currentState != State.none)
                {
                    stateDict[currentState].SetActive(false);
                }
                if (newState != State.none)
                {
                    stateDict[newState].SetActive(true);
                }
                currentState = newState;
            }
        }   
    }
}
