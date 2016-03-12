using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
public class NetIO {
    private static NetIO _instance;
    public static NetIO Instance {
        get {
            if (_instance == null)
                _instance = new NetIO();
            return _instance;
        }
    }
    private Socket socket;
    private string ip = "113.57.178.60";

    private int port = 6500;
    private byte[] readBuffer = new byte[1024];
    List<byte> cache = new List<byte>();//消息缓冲区
    private bool isReading = false;
    /// <summary>
    /// type类型与模块的注册
    /// </summary>
    public Dictionary<int, IHandler> handlers = new Dictionary<int, IHandler>();
    public List<SocketModel> messages = new List<SocketModel>();
    private NetIO() {
        try
        {
            IPAddress ipAddr = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //socket.Connect(ipAddr, port);//113.57.178.60/27.17.53.60
            socket.Connect("1462z254b3.iok.la", 15321);
            socket.BeginReceive(readBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, readBuffer);
        }
        catch (System.Exception ex)
        {
            string msg= String.Format("无法连接到服务器,{0}", ex.Message);
            PopWindowManager.AddMsg(msg);
        }
    }
    public void RegisterHandler(int type, IHandler handler) {
        if (!handlers.ContainsKey(type))
            handlers.Add(type, handler);
    }
    public void UnRegisterHandler(int type) {
        handlers.Remove(type);
    }
    private void ReceiveCallBack(IAsyncResult ar) {
        try
        {
            int length = socket.EndReceive(ar);//返回当前socket收到的消息长度
            byte[] message = new byte[length];
            Buffer.BlockCopy(readBuffer, 0, message, 0, length);
            cache.AddRange(message);//将接受到的消息添加至缓冲区
            if (!isReading)
            {
                isReading = true;
                OnData();
            }
        }
        catch (System.Exception ex)
        {

            Debug.Log("远程服务器断开连接.");
            PopWindowManager.AddMsg("远程服务器断开连接..");
            socket.Close();
        }
        socket.BeginReceive(readBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, readBuffer);
    }
    //写消息
    public void write(byte type,int area,int command,object message) {
        ByteArray arr = new ByteArray();
        arr.write(type);
        arr.write(area);
        arr.write(command);
        if(message!=null)
        {
            arr.write(SerializeUtil.encode(message));
        }
        byte[] result = arr.getBuff();
        ByteArray sendArr = new ByteArray();
        sendArr.write(arr.Length);
        sendArr.write(result);
        try
        {
            socket.Send(sendArr.getBuff());
        }catch(Exception e)
        {
            Debug.Log("网络错误，请从新连接" + e.Message);
            PopWindowManager.AddMsg("网络连接失败");
        }
        sendArr.Close();
        arr.Close();
        //return result;
    }
    void OnData() {
        byte[] result = lDecode(ref cache);
        if (result == null)
        {
            isReading = false;
            return;
        }
        SocketModel message = mDecode(result);
        if(message==null)
        {
            isReading = false;
            return;
        }
        //得到了消息进行消息处理
        messages.Add(message);
        OnData();
    }
    //消息长度解码
    private  byte[] lDecode(ref List<byte> cache) {
        if (cache.Count < 4)
        {
            isReading = false;
            return null;
        }
        MemoryStream ms = new MemoryStream(cache.ToArray());//创建内存流对象，并写入缓存数据
        BinaryReader br = new BinaryReader(ms);//二进制读取流
        int length = br.ReadInt32();//读取当前消息长度
        if (length > ms.Length - ms.Position)//传输的消息达不到包头给定的长度，即消息不够
        {
            return null;
        }
        byte[] result = br.ReadBytes(length);
        cache.Clear();
        //将读取剩余后数据写入缓存
        cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
        br.Close();
        ms.Close();
        return result;
    }
    //对消息的解码
    private SocketModel mDecode(byte[] value) {
        ByteArray ba = new ByteArray(value);
        SocketModel model = new SocketModel();
        byte type;
        int area;
        int command;
        ba.read(out type);
        ba.read(out area);
        ba.read(out command);
        model.Type = type;
        model.Area = area;
        model.Command = command;
        if (ba.Readnable)
        {
            byte[] message;
            ba.read(out message, ba.Length - ba.Postion);
            model.Message = SerializeUtil.decode(message);
        }
        ba.Close();
        return model;
    }
}
