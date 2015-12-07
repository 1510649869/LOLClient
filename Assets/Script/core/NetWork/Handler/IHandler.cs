using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class IHandler:MonoBehaviour {
    public abstract int type { get; }
    public virtual void Start() {
        NetIO.Instance.RegisterHandler(type,this);
    }
    public virtual void OnDestory() {
        NetIO.Instance.UnRegisterHandler(type);
    }
    public abstract void MessageReceive(SocketModel model);
}
