﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
public class SerializeUtil {
    public static byte[] encode(object value) {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter bw = new BinaryFormatter();//二进制序列化对象
        //将obj对象序列化成二进制数据 写入到 内存流
        bw.Serialize(ms, value);
        byte[] result = new byte[ms.Length];
        Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
        ms.Close();
        return result;
    }
    public static object decode(byte[] value) {
        MemoryStream ms = new MemoryStream(value);
        BinaryFormatter bw = new BinaryFormatter();//二进制序列化对象
        //二进制数据反序列化为obj对象
        object result = bw.Deserialize(ms);
        ms.Close();
        return result;
    }
}
