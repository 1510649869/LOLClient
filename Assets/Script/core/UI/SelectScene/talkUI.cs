using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class talkUI : MonoBehaviour {
    [SerializeField]
    private InputField talkInput;
    [SerializeField]
    private Button sendBtn;
    [SerializeField]
    private Text context;
    SelectHandler select;
    void Start() {
        if(GameObject.Find("NetWork")!=null)
        {
            select = GameObject.Find("NetWork").GetComponent<SelectHandler>();
            select.Talk = showMsg;
        }
        sendBtn.onClick.AddListener(delegate
        {
            sendClick();
        });
    }
    void sendClick() {
        if(talkInput.text!=string.Empty)
        {
            select.pubtalk(talkInput.text);
            talkInput.text = string.Empty;
        }
    }
    void showMsg(string msg) {
        context.text += msg+"\n";
    }
}
