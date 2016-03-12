using GameProtocol.dto;
using GameProtocol.dto.fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void OnLoginEvent(int value);
public delegate void OnRegEvent(int value);
public delegate void OnGetEvent();
public delegate void OnCreateEvent();
public delegate void OnLineEvent(UserModel model);
public delegate void OnGetMatchEvent(UserModel model);
public delegate void OnLeaveMatchEvent();
public delegate void OnLoadSelectSceneEvent();
public delegate void RefeshSelectUIEvent(SelectRoomDTO room);
public delegate void selectedEvent();
public delegate void TalkEvent(string msg);
public delegate void InitViewEvent(FightPlayerModel model);
public class DelegateEvent {

}