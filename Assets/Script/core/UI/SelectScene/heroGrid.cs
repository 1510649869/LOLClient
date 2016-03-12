using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class heroGrid : MonoBehaviour {
    private Button btn;
    private Image hero;
    private int id = -1;

    private SelectHandler handler;
    void Awake() {
        handler = GameObject.Find("NetWork").GetComponent<SelectHandler>();
        hero = transform.Find("headBtn").GetComponent<Image>();
        btn = transform.Find("headBtn").GetComponent<Button>();
    }

    public void init(int id) {
        this.id = id;
        this.hero.sprite = Resources.Load<Sprite>("HeroIcon/"+id);
        btn.onClick.AddListener(delegate
        {
            handler.applySelect(id);
        });
    }
    public void enable() {
        btn.interactable = true;
    }
    public void disable() {
        btn.interactable = false;
    }
}
