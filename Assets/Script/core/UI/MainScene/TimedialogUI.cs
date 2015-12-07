using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimedialogUI : MonoBehaviour {
    private Text timeText;
    private Text contentText;
    private int timer=0;
    private bool isStart = false;
    string[] point = { "。", "。。", "。。。" };
    void Start() {
        timeText=transform.Find("timer").GetComponent<Text>();
        contentText = transform.Find("Image/Text").GetComponent<Text>();
        transform.Find("btncancel").GetComponent<Button>().onClick.AddListener(delegate()
        {
            //向服务器请求取消匹配
            //OnHide();
        });
    }
    void Update() {
        if(isStart)
        {
            timeText.text = "正在为你匹配对手。";
            timeText.text = "正在为你匹配对手.。";
            timeText.text = "正在为你匹配对手。。。";
            timer += (int)Time.deltaTime;
            timeText.text = timer.ToString();
        }
    }
    public void ShowTimer() {
        isStart = true;
        this.gameObject.SetActive(true);
    }
    public void OnHide() {
        isStart = false;
        timer = 0;
        this.gameObject.SetActive(false);
    }
}
