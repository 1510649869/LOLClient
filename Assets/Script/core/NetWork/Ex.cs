using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 自定义扩展方法
/// </summary>
public static class Ex {
    public static void Write(this MonoBehaviour momo, byte type, int area, int command, object message) {
        NetIO.Instance.write(type, area, command, message);
    }
}
