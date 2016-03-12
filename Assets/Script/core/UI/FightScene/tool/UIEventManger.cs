using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class UIEventManger : MonoBehaviour,IPointerClickHandler{

    public void OnPointerClick(PointerEventData eventData) {
        switch(eventData.pointerId)
        {
            case PointerInputModule.kMouseLeftId:
                FightUI._instance.leftClick(eventData.position);
                break;
            case PointerInputModule.kMouseRightId:
                FightUI._instance.rightClick(eventData.position);
                break;
            //case PointerInputModule.kFakeTouchesId:
            //    FightUI._instance.rightClick(eventData.position);
            //    break;
        }
    }
    void Update() {
        Vector3 position = Input.mousePosition;
        if(position.x<10&&position.x>-10)
        {
            FightUI._instance.cameraHMove(1);
        }
        else if (position.x > Screen.width - 10&&position.x<Screen.width+10)
        {
            FightUI._instance.cameraHMove(-1);
        }
        else
        {
            FightUI._instance.cameraHMove(0);
        }
        if (position.y < 10&&position.y>-10)
        {
            //向下
            FightUI._instance.cameraVMove(-1);
        }
        else if (position.y > Screen.height - 10&&position.y<Screen.height+10)
        {
            FightUI._instance.cameraVMove(1);
        }
        else
        {
            FightUI._instance.cameraVMove(0);
        }
        //复原相机
        if (Input.GetKey(KeyCode.Space))
        {
            FightUI._instance.LookAt();
        }
    }
}
