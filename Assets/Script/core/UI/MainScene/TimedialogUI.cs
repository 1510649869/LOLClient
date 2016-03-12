using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimedialogUI : MonoBehaviour {
    private Text timeText;
    private Text contentText;
    private float timer = 0;
    private bool isStart = false;
    string[] point = { ".", "..", "...","....",".....","......","......." };
    int index=0;

    private MatchHandler matchHandler;
    void Start() {
        timeText=transform.Find("timer").GetComponent<Text>();
        contentText = transform.Find("Image/Text").GetComponent<Text>();
        InvokeRepeating("showtip", 0, 0.4f);
        matchHandler = GameObject.Find("NetWork").GetComponent<MatchHandler>();
        matchHandler.OnLeaveMatch = this.OnHide;//向该模块注册取消匹配返回的处理函数
        transform.Find("btncancel").GetComponent<Button>().onClick.AddListener(delegate()
        {
            //向服务器请求取消匹配
            matchHandler.LeaveMatch();
        });
    }
    void Update() {
        if(isStart)
        {
            timer += Time.deltaTime;
            //Debug.Log(Time.deltaTime);
            int time = (int)timer;
            timeText.text = getTime(time);
        }
    }
    //Enumerable
    //IEnumerator
    //1h=60分=3600s  
    string getTime(int time) {
        int s = time % 60;
        int h = time / 3600; 
        int m = time / 60-h*60;
        return convert(h) + ":" + convert(m) +":"+ convert(s);
    }
    string convert(int temp) {
        if (temp < 10)
        {
            return "0" + temp;
        }
        else
            return "" + temp;
    }
    void showtip() {
        if(isStart)
        {
            contentText.text = "正在为你匹配对手" + point[index];
            index += 1;
            if (index > 6)
            {
                index = 0;
            }
        }
    }
    public void OnHide() {
        isStart = false;
        timer = 0;
        this.gameObject.SetActive(false);
    }
    public void ShowTimer() {
        this.gameObject.SetActive(true);
        this.isStart = true;
    }

}
