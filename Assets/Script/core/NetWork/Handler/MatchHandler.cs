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
           case MatchProtocol.ENTER_SRES:
               break;
           case MatchProtocol.LEAVE_SRES:
               break;
       }
    }
    public event OnGetMatchEvent OnGetMatch;
    public event OnLeaveMatchEvent OnLeaveMatch;
}
