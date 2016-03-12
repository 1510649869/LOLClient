using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameProtocol.dto;
using UnityEngine.UI;
public class SelectUI : MonoBehaviour {
    private SelectHandler selectHandler;

    public GameObject heroHead;
    public GameObject herosUI;

    private Dictionary<int, heroGrid> gridMap = new Dictionary<int, heroGrid>();

    [SerializeField]
    private selectGrid[] left;
    [SerializeField]
    private selectGrid[] right;
    [SerializeField]
    private Button ready;
    void Awake() {
        if (GameObject.Find("NetWork") == null)
            return;
        selectHandler = GameObject.Find("NetWork").GetComponent<SelectHandler>();
        if(selectHandler==null)
        {
            selectHandler = GameObject.Find("NetWork").AddComponent<SelectHandler>();
        }
    }
    void Start() {
        if (selectHandler == null)
            return;
        selectHandler.applyEnter();
        initHeroList();
        selectHandler.refeshSelectUI = refeshView;
        selectHandler.selected = selected;
        ready.onClick.AddListener(delegate {
            selectHandler.applyReady();
        });
    }
    void initHeroList() {
        if (GameData.user == null)
            return;
        int index = 0;
        foreach (int item in GameData.user.heroList)
        {
            GameObject go = Instantiate(heroHead) as GameObject;
            heroGrid grid = go.GetComponent<heroGrid>();
            grid.init(item);
            go.transform.parent = herosUI.transform;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(35 + 70 * (index % 8) - 280, -35 - (index / 8) * 70 + 111);
            gridMap.Add(item, grid);
            index++;
        }
    }
    void selected() {
        ready.interactable = false;
    }
    void refeshList(SelectRoomDTO room) {
        if(!ready.interactable)
        {
            foreach(heroGrid item in gridMap.Values)
            {
                item.disable();
            }
            return;
        }
        List<int> selectedHero = new List<int>();
        int team = room.getTeam(GameData.user.id);
        if(team==1)
        {
            foreach (SelectModel item in room.teamOne)
            {
                if (item.hero != -1)
                    selectedHero.Add(item.hero);
            }
        }
        else if(team==2)
        {
            foreach (SelectModel item in room.teamTwo)
            {
                if (item.hero != -1)
                    selectedHero.Add(item.hero);
            }
        }

        foreach(int item in gridMap.Keys)
        {
            if(selectedHero.Contains(item))
            {
                gridMap[item].disable();
            }
            else
            {
                gridMap[item].enable();
            }
        }
        
    }
    void refeshView(SelectRoomDTO room) {
        int team = room.getTeam(GameData.user.id);
        if(team==1)
        {
          for(int i=0;i<room.teamOne.Length;i++)
          {
              left[i].refesh(room.teamOne[i]);
          }
          for (int i = 0; i < room.teamTwo.Length; i++)
          {
              right[i].refesh(room.teamTwo[i]);
          }

        }
        else if(team==2)
        {
            for (int i = 0; i < room.teamTwo.Length; i++)
            {
                left[i].refesh(room.teamTwo[i]);
            }
            for (int i = 0; i < room.teamOne.Length; i++)
            {
                right[i].refesh(room.teamOne[i]);
            }
        }
        refeshList(room);

    }

}
