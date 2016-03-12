using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;
using System.Collections.Generic;

public class FightScene : MonoBehaviour {
    public static FightScene _instance;

    public Transform deathMask;
    private Dictionary<int, GameObject> teamOne = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> teamTwo = new Dictionary<int, GameObject>();
    private Dictionary<int, PlayerCon> models = new Dictionary<int, PlayerCon>();
    private Dictionary<int,GameObject> gohpDic=new Dictionary<int,GameObject>();
    [SerializeField]
    GameObject[] teamOnePoints;
    [SerializeField]
    GameObject[] teamTwoPoints;
    [SerializeField]
    Transform HP;

    void Awake() {
        if (_instance == null)
            _instance = this;
    }
	void Start () {
	
	}
	void Update () {
	
	}
    //加载英雄
    GameObject LoadPlayer(int code,GameObject point) {
        GameObject player;
        player = Instantiate(Resources.Load("Prefab/Player/" + code),point.transform.position,point.transform.rotation) as GameObject;
        return player;
    }
    //加载heroHP
    GameObject LoadHP(GameObject go, FightPlayerModel player) {
        GameObject hp = Instantiate(Resources.Load("Prefab/HpBar")) as GameObject;
        hp.transform.parent =HP;
        HpBar hpBar = hp.GetComponent<HpBar>();
        hpBar.SetTaget(go.transform.Find("headhp"));
        hpBar.refesh(player);
        return hp;
    }
    public void start(FightRoomModel model) {
        int MyTeam = -1;
        foreach (AbsFightModel item in model.teamOne)
        {
            if(item.id==GameData.user.id)
            {
                MyTeam = item.team;
            }
        }
        foreach (AbsFightModel item in model.teamTwo)
        {
            if (item.id == GameData.user.id)
            {
                MyTeam = item.team;
            }
        }
        int team1 = 0;
        foreach (AbsFightModel item in model.teamOne)
        {
            GameObject go=null;
            GameObject hp = null;
            PlayerCon pc=null;
            if (item.type == ModelType.HUMAN)
            {
                //实例化英雄
                go = LoadPlayer(item.code, teamOnePoints[team1]);
                pc = go.GetComponent<PlayerCon>();
                pc.init((FightPlayerModel)item, MyTeam);
                hp = LoadHP(go, (FightPlayerModel)item);
                gohpDic.Add(item.id, hp);
                if (item.id == GameData.user.id)
                {
                    FightUI._instance.initView((FightPlayerModel)item, go);
                    FightUI._instance.LookAt();
                }
                team1++;
            }
            else if(item.type==ModelType.BUILD)
            {
                //实例化防御塔
                FightBuildModel build = (FightBuildModel)item;
                go = Instantiate(Resources.Load("Prefab/Build/1-" + item.code)) as GameObject;
                go.transform.position = new Vector3(build.defaultVec.x, build.defaultVec.y, build.defaultVec.z);
                pc = go.GetComponent<PlayerCon>();
               
            }
            this.teamOne.Add(item.id, go);
            this.models.Add(item.id, pc);
        }
        int team2 = 0;
        foreach (AbsFightModel item in model.teamTwo)
        {
            GameObject go;
            PlayerCon pc;
            GameObject hp = null;
            if (item.type == ModelType.HUMAN)
            {
                //实例化英雄
                go = LoadPlayer(item.code, teamTwoPoints[team2]);
                pc = go.GetComponent<PlayerCon>();
                pc.init((FightPlayerModel)item, MyTeam);
                hp = LoadHP(go,(FightPlayerModel)item);
                gohpDic.Add(item.id, hp);
                if (item.id == GameData.user.id)
                {
                    FightUI._instance.initView((FightPlayerModel)item, go);
                    FightUI._instance.LookAt();
                }
                team2++;
            }
            else
            {
                FightBuildModel build = (FightBuildModel)item;
                go = Instantiate(Resources.Load("Prefab/build/2-" + item.code)) as GameObject;
                go.transform.position = new Vector3(build.defaultVec.x, build.defaultVec.y, build.defaultVec.z);
                pc = go.GetComponent<PlayerCon>();
            }
            this.teamTwo.Add(item.id, go);
            this.models.Add(item.id, pc);
        }
    }
    public void move(int id, Vector3 target) {
        models[id].move(target);
        Vector3 pos = target;
        pos.y = models[id].transform.position.y;
        models[id].transform.LookAt(pos);
    }

    public void attack(AttackDTO dto) {
        PlayerCon obj = models[dto.userID];
        PlayerCon target = models[dto.targetID];
        obj.attack(new Transform[] { target.transform });
    }
    public void damage(DamageDTO value) {
        foreach(int[] item in value.target)
        {
            PlayerCon pc = models[item[0]];
            pc.data.hp -= item[1];
            //掉血效果
            Debug.Log("受到伤害掉血");
            //更新ui
            gohpDic[pc.data.id].GetComponent<HpBar>().refesh(pc.data);
            if(pc.data.id==GameData.user.id)
            {
                //如果是自己受到伤害

            }
            if(item[2]==0)//hp没有了
            {
                if (item[0] >= 0)
                {
                    //pc.gameObject.SetActive(false);
                    //gohpDic[pc.data.id].SetActive(false);
                    
                    //models.Remove(pc.data.id);//将该英雄从列表中移除
                    pc.death();
                    //血条处理
                    Destroy(gohpDic[pc.data.id]);
                    gohpDic.Remove(pc.data.id);
                    //gohpDic.Remove();
                    if (pc.data.id == GameData.user.id)
                    {
                        //s设置-->死亡
                        deathMask.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Destroy(pc.gameObject);
                }
            }

        }
    }

    public void skillUP(FightSkill skill) {
        for (int i = 0; i < FightUI._instance.myHero.data.skills.Length; i++)
        {
            if (FightUI._instance.myHero.data.skills[i].code ==skill.code)
            {
                FightUI._instance.myHero.data.free -= 1;
                FightUI._instance.myHero.data.skills[i] = skill;
                FightUI._instance.refeshLevelUp();
            }
        }
    }
}
