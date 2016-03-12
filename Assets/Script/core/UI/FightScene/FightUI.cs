using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;
using UnityEngine.UI;
using System.Collections.Generic;


public class FightUI : MonoBehaviour {
    public static FightUI _instance;
    FightHanler fight;
    [SerializeField]
    Image headIcon;
    [SerializeField]
    Text LvTex;
    [SerializeField]
    skillGrid[] skills;
    [SerializeField]
    public Camera camera;

    public int skill=-1;

    public PlayerCon myHero;
    void Awake() {
        if (_instance == null)
            _instance = this;
        GameObject net = GameObject.Find("NetWork");
        if (net == null)
            return;
        if ((fight = net.GetComponent<FightHanler>()) == null)
        {
            fight = net.AddComponent<FightHanler>();
        }
    }
	void Start () {
        if (fight == null)
            return;
        fight.applyEnter();
	}

    public void refeshLevelUp() {
        int i = 0;
        foreach (FightSkill item in myHero.data.skills)
        {
            if (item.nextLevel <= myHero.data.level)
            {
                if (myHero.data.free > 0)
                {
                    skills[i].SetBtnState(true);
                }
                else
                {
                    skills[i].SetBtnState(false);
                }
            }
            else
            {
                skills[i].SetBtnState(false);
            }
            skills[i].SkillChange(item);
            skills[i].setMask(0);
            i++;
        }
    }

    /// <summary>
    /// 初始化本英雄面版
    /// </summary>
    /// <param name="model"></param>
    public void initView(FightPlayerModel model,GameObject go) {
        myHero = go.GetComponent<PlayerCon>();
        headIcon.sprite = Resources.Load<Sprite>("HeroIcon/"+model.code);
        LvTex.text = model.level.ToString();
        int i = 0;
        foreach(FightSkill item in model.skills)
        {
            skills[i].init(item);
            i++;
        }
    }
    public void LookAt() {
        camera.transform.position = myHero.transform.position + new Vector3(-18, 20, 0);
      // camera.transform.LookAt(myHero.transform.position);
    }
    private int cameraH;
    private int cameraV;
    public float cameraSpeed = 1f;
    public void cameraHMove(int dir) {
        if (cameraH != dir)
            cameraH = dir;
    }
    public void cameraVMove(int dir) {
        if (cameraV != dir)
            cameraV = dir;
    }
    void Update() {
        switch (cameraH)
        {
            case 1:
                if (Camera.main.transform.position.z < 166)
                {
                    float z=Camera.main.transform.position.z + (float)cameraH/2;
                    float x = 190 - z;
                    Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, z);
                }
                break;
            case -1:
                if (Camera.main.transform.position.z > 40)
                {
                    float z = Camera.main.transform.position.z + (float)cameraH / 2;
                    float x = 190 - z;
                    Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y,z);
                }
                break;
        }
        switch (cameraV)
        {
            case 1:
                if (Camera.main.transform.position.x < 150)
                {
                    float x = Camera.main.transform.position.x + (float)cameraV/2;
                    float z = 190 - x;
                    Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, z);
                }
                break;
            case -1:
                if (Camera.main.transform.position.x > 25)
                {
                    float x = Camera.main.transform.position.x + (float)cameraV / 2;
                    float z = 190 - x;
                    Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y, z);
                }
                break;
        }
    }
    /// <summary>
    /// 鼠标右键
    /// </summary>
    /// <param name="position"></param>
    public void leftClick(Vector2 position) {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 200);
        List<Transform> list = new List<Transform>();
        Vector3 tigger = Vector3.zero;
        for (int i = hits.Length - 1; i >= 0; i--)
        {
            RaycastHit item = hits[i];
            if (item.transform.gameObject.layer == LayerMask.NameToLayer("enemy"))//点击的是enenmy
            {
                PlayerCon con = item.transform.GetComponent<PlayerCon>();
                if (fight == null)
                    return;
                //CursorManager._instance.setAttack();
                if (Vector3.Distance(myHero.transform.position, item.transform.position) < con.data.aRange)
                {
                    //进行攻击
                    fight.applyAttack(con.data.id);
                }
                else
                {

                }
                tigger = item.point;
            }
            list.Add(item.transform);
            if (skill == -1)
            {

            }
        }
        if(skill!=-1)
        {
            //技能攻击
            myHero.baseSkill(skill, list.ToArray(), tigger);
        }
        skill = -1;
    }
    public GameObject testGo;
    public bool isTest = false;
    public void rightClick(Vector2 position) {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 200);
        for (int i = hits.Length - 1; i >= 0; i--)
        {
            RaycastHit item = hits[i];
            //if (item.transform.gameObject.layer == LayerMask.NameToLayer("enemy"))
            //{
            //    PlayerCon con = item.transform.GetComponent<PlayerCon>();
            //    if (Vector3.Distance(myHero.transform.position, item.transform.position) < con.data.aRange)
            //    {
            //        fight.applyAttack(con.data.id);
            //        return;
            //    }
            //    continue;
            //}
            //寻路地板
            if (item.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                if(isTest)
                {
                    testGo.GetComponent<PlayerCon>().move(item.point);
                }
                
                if (fight == null)
                    return;
                MoveDTO dto = new MoveDTO();
                dto.x = item.point.x;
                dto.y = item.point.y;
                dto.z = item.point.z;
                dto.userId = GameData.user.id;
                fight.applyMove(dto);
                return;
            }
        }
    }

}
