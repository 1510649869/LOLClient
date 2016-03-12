using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
    [SerializeField]
    Text nameTex;
    [SerializeField]
    Image hp;

    UIFollowTarget follow;
    void Awake() {
        follow = this.GetComponent<UIFollowTarget>();
    }
    public void SetTaget(Transform go) {
        follow.target=go;
    }
    //刷新角色头顶
    public void refesh(FightPlayerModel model) {
        nameTex.text = model.name;
        this.GetComponent<Slider>().value = (float)model.hp / model.maxHp;
        if (model.id == GameData.user.id)
        {
            hp.color = Color.green;
        }
        else
        {
            hp.color = Color.red;
        }
    }
}
