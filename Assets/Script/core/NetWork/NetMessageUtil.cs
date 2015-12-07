using UnityEngine;
using System.Collections;

public class NetMessageUtil : MonoBehaviour {
    IHandler login;
    void Start () {
        login = this.GetComponent<LoginHandler>();
        DontDestroyOnLoad(this.gameObject);
	}
	void Update () {
        while(NetIO.Instance.messages.Count>0)
        {
            SocketModel model = NetIO.Instance.messages[0];
            StartCoroutine("MessageReceive", model);
            NetIO.Instance.messages.RemoveAt(0);
        }
	}
    void MessageReceive(SocketModel model) {
        int type = model.Type;
        IHandler handler;
        NetIO.Instance.handlers.TryGetValue(type,out handler);
        if (handler != null)
            handler.MessageReceive(model);
    }
}
