using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameProtocol.dto.fight;
public class skillGrid : MonoBehaviour {
    [SerializeField]
    GameObject SkillInfoPanel;
    [SerializeField]
    Button btn;
    [SerializeField]
    Image mask;
    [SerializeField]
    Image skillIcon;
    [SerializeField]
    Button leveUpBtn;
    private bool sclied=false;
    private FightSkill skill;
    private float maxTime=0;
    private float currentTime = 0;
    void Start() {
        leveUpBtn.onClick.AddListener(leveUp);
        btn.onClick.AddListener(pointClick);
    }

    private void pointClick() {
        SkillInfoPanel.transform.Find("Text").GetComponent<Text>().text = "";
        SkillInfoPanel.gameObject.SetActive(false);
        //申请技能攻击
        FightUI._instance.skill = skill.code;
       // throw new System.NotImplementedException();
    }
    public void init(FightSkill skill) {
        this.skill = skill;
        Sprite sp = Resources.Load<Sprite>("SkillIcon/" + skill.code);
        skillIcon.sprite = sp;
        //mask.fillAmount = 0;
        btn.interactable = false;
        mask.gameObject.SetActive(true);
    }
    void Update() {
        if(sclied)
        {
            currentTime-=Time.deltaTime;
            if(currentTime<=0)
            {
                currentTime=0;
                mask.gameObject.SetActive(false);
                sclied=false;
                btn.interactable = true;
            }
            mask.fillAmount = currentTime / maxTime;
        }
    }
    //点击技能后进行的攻击
    public void setMask(float time) {
        if(time==0)
        {
            if(!sclied&&skill.level>0)
            {
                mask.gameObject.SetActive(false);
                btn.interactable = true;
            }
            else
            {
                mask.gameObject.SetActive(true);
                btn.interactable = false;
                return;
            }
        }
        maxTime = time;
        currentTime = time;
        mask.gameObject.SetActive(true);
        btn.interactable = false;
        sclied = true;
    }

    public void pointEnter() {
        //显示技能信息
        SkillInfoPanel.gameObject.SetActive(true);
        if(skill!=null)
          SkillInfoPanel.transform.Find("Text").GetComponent<Text>().text = skill.info;
    }
    public void pointExit() {
        SkillInfoPanel.transform.Find("Text").GetComponent<Text>().text = "";
        SkillInfoPanel.gameObject.SetActive(false);
        //Debug.Log("33");
    }
    //点击技能之后
    public void SetBtnState(bool state) {
        leveUpBtn.interactable = state;
    }
    public void leveUp() {
        //向服务器申请升级技能;
        this.Write((byte)GameProtocol.Protocol.TYPE_FIGHT, 0, GameProtocol.FightProtocol.SKILL_UP_CREQ, skill.code);
    }

    public void SkillChange(FightSkill item) {
        this.skill = item;
    }
}
