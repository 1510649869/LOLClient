﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class creatUI : MonoBehaviour {

    private InputField nameInput;
    private UserHandler userhandler;
	void Awake () {
        nameInput = transform.Find("createbg/InputField").GetComponent<InputField>();
        userhandler = GameObject.Find("NetWork").AddComponent<UserHandler>();
        transform.Find("createbg/btncreate").GetComponent<Button>().onClick.AddListener(delegate() {
            if (nameInput.text == null || nameInput.text.Length < 3)
            {
                PopWindowManager.AddMsg("名称不合法");
                return;
            }
            userhandler.applyCreate(nameInput.text);
        });
	}
}
