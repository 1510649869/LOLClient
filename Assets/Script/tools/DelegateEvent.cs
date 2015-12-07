using GameProtocol.dto;
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