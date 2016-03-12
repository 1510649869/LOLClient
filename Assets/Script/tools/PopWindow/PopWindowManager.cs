using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopWindowManager : MonoBehaviour {
    private static List<string> MsgList = new List<string>();
    public PopWindowUI windowUI;
    void Start() {
        //InvokeRepeating("Check", 0, 0.01f);
    }
    void Update() {
        if (MsgList.Count > 0)
        {
            string msg = MsgList[0];
            MsgList.RemoveAt(0);
            windowUI.Show(msg);
        }
    }
    public static void AddMsg(string msg) {
        MsgList.Add(msg);
    }

}
