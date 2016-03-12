using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameProtocol;
public class MatchHandler:IHandler {
    public override int type {
        get { return GameProtocol.Protocol.TYPE_MATCH; }
    }
    public void EnterMatch() {
        this.Write((byte)type, 0, MatchProtocol.ENTER_CREQ, null);
    }
    public void LeaveMatch() {
        this.Write((byte)type, 0, MatchProtocol.LEAVE_CREQ, null);
    }
    public override void MessageReceive(SocketModel model) {
       switch(model.Command)
       {
           case MatchProtocol.ENTER_SRES://返回匹配结果
               break;
           case MatchProtocol.LEAVE_SRES://返回取消匹配结果
               OnLeaveMatch();
               break;
           case MatchProtocol.ENTER_SELECT_BRO:
               //PopWindowManager.AddMsg("进入选人界面");
               OnLoadSelectSscene();
               break;
       }
    }
    //public event OnGetMatchEvent OnGetMatch;
    public OnLeaveMatchEvent OnLeaveMatch;
    public OnLoadSelectSceneEvent OnLoadSelectSscene;
}
