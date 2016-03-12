using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour {
    public static CursorManager _instance;
    public Texture2D cursor_attack;
    public Texture2D cursor_normal;

    private Vector2 hotspot = Vector2.zero;
    private CursorMode mode = CursorMode.ForceSoftware;
    void Awake() {
        _instance = this;
    }
    void Start() {
        setNomal();
    }
    public void setAttack() {
        Cursor.SetCursor(cursor_attack, hotspot, mode);
    }
    public void setNomal() {
        Cursor.SetCursor(cursor_normal, hotspot, mode);
    }

}
