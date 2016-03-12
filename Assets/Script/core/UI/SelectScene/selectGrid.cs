using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameProtocol.dto;
public class selectGrid : MonoBehaviour {
    private Image img;
    private Image head;
    private Text tex;
    void Awake() {
        img = this.GetComponent<Image>();
        head = transform.Find("headIcon").GetComponent<Image>();
        tex = transform.Find("nameText").GetComponent<Text>();
    }
    public void refesh(SelectModel model) {
        if(model.userID==GameData.user.id)
        {
            tex.color = Color.green;
        }
        else
        {
            tex.color = Color.white;
        }
        tex.text = "等待进入";
        if(model.enter)
        {
            tex.text = model.name;
            if(model.hero==-1)
            {
                head.sprite = Resources.Load<Sprite>("HeroIcon/-1");
            }
            else
            {
                head.sprite = Resources.Load<Sprite>("HeroIcon/"+model.hero);
                if (model.ready)
                {
                    ready();
                }
                else
                {
                    img.color = Color.white;
                }
            }
        }
        else
        {
            head.sprite = Resources.Load<Sprite>("HeroIcon/nil");
            tex.text = "等待进入";
        }
    }
    private void ready() {
        img.color = Color.red;
    }
}
