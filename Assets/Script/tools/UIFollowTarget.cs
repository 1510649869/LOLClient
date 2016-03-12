using UnityEngine;
using System.Collections;

public class UIFollowTarget : MonoBehaviour{
    public Transform target;
    Transform mTrans;

    void Awake() {
        mTrans = transform;
        //gameCamera = GameObject.Find("004Main Camera").camera;
    }

    void Update() {
        //Vector3 pos=gameCamera.WorldToScreenPoint
        if(target!=null)
        {

            Vector3 pos =Camera.main.WorldToScreenPoint(target.position);
            transform.position = pos;
        }
       
    }

}
