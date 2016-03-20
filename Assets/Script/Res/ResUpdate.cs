using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
public class ResUpdate : MonoBehaviour {
    private static readonly string VERSION_FILE = "version.txt";
    private static readonly string LOCAL_RES_URL = "file://" + Application.dataPath + "/Res/";
    private static readonly string SERVER_RES_URL = "file:///F:/Res/";
    private static readonly string LOCAL_RES_PATH = Application.dataPath + "/Res/";

    private Dictionary<string, string> LocalResVersion;
    private Dictionary<string, string> ServerResVersion;
    private List<string> NeedDownfiles;

    private bool NeedUpdateLocalVesionfile = false;
    void Start() {
        LocalResVersion = new Dictionary<string, string>();
        ServerResVersion = new Dictionary<string, string>();
        NeedDownfiles = new List<string>();
        //加载服务器的version
        StartCoroutine(DownLoad(LOCAL_RES_URL + VERSION_FILE, delegate(WWW localVersion)
        {
            ParseVersionFile(localVersion.text, LocalResVersion);
            StartCoroutine(this.DownLoad(SERVER_RES_URL + VERSION_FILE, delegate(WWW serverVersion)
            {
                //保存服务端version  
                ParseVersionFile(serverVersion.text, ServerResVersion);
                //计算出需要重新加载的资源  
                CompareVersion();
                //加载需要更新的资源  
                DownLoadRes();
            })); 
        }));
    }
    void DownLoadRes() {
        if (NeedDownfiles.Count == 0)
        {
            UpdateLocalVersionFile();
            return;
        }
        string file = NeedDownfiles[0];
        NeedDownfiles.RemoveAt(0);
        StartCoroutine(this.DownLoad(SERVER_RES_URL + file, delegate(WWW w)
        {
            //将下载的资源替换本地就的资源  
            ReplaceLocaRes(file, w.bytes);
            DownLoadRes();
        })); 
    }
    void ReplaceLocaRes(string filename, byte[] data) {
        string filePath = LOCAL_RES_PATH + filename;
        System.IO.FileStream stream= new System.IO.FileStream(LOCAL_RES_PATH + filename, System.IO.FileMode.Create);
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
    }
    IEnumerator Show() {
        //WWW asset = new WWW(LOCAL_RES_URL + "image.assetbundle");
        foreach (var fileName in NeedDownfiles)
        {
            WWW asset = new WWW(LOCAL_RES_URL + fileName);
            yield return asset;
            AssetBundle bundle = asset.assetBundle;
            //GameObject image = Instantiate(bundle.Load("Image")) as GameObject;
            //image.transform.parent = GameObject.Find("Image").transform;
            //image.transform.localPosition = Vector3.zero;
            bundle.Unload(false);//删除镜像
        }

    }
    /// <summary>
    /// 更新本地配置
    /// </summary>
    void UpdateLocalVersionFile() {
        if (NeedUpdateLocalVesionfile)
        {
            StringBuilder versions = new StringBuilder();
            foreach (var item in ServerResVersion)
            {
                versions.Append(item.Key).Append(",").Append(item.Value).Append("\n");
            }
            System.IO.FileStream stream = new System.IO.FileStream(LOCAL_RES_PATH + VERSION_FILE, System.IO.FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }
        StartCoroutine(Show());
    }
    /// <summary>
    /// 版本比较
    /// </summary>
    void CompareVersion() {
        foreach (var version in ServerResVersion)
        {
            string fileName = version.Key;//需要热跟新加载的文件名
            string serverMD5 = version.Value;//版本号编码
            if (!LocalResVersion.ContainsKey(fileName))
            {
                NeedDownfiles.Add(fileName);//需要从远程服务器加载相关的资源
            }
            else
            {
                string localMD5;
                LocalResVersion.TryGetValue(fileName, out localMD5);
                if (!serverMD5.Equals(localMD5))
                {
                    NeedDownfiles.Add(fileName);
                }
            }
        }
        //同时更新本地的version.txt
        NeedUpdateLocalVesionfile = NeedDownfiles.Count > 0;
    }
    void ParseVersionFile(string content, Dictionary<string, string> dict) {
        if (content == null || content.Length == 0)
            return;
        string[] items = content.Split(new char[] { '\n' });
        foreach (var item in items)
        {
            string[] info = item.Split(new char[] { ',' });
            if (info != null && info.Length == 2)
            {
                dict.Add(info[0],info[1]);
            }
        }

    }
    IEnumerator DownLoad(string url,HandleFinishDownload finshFun) {
        WWW www = new WWW(url);
        yield return www;
        if (finshFun != null)
            finshFun(www);
        www.Dispose();
    }
    public delegate void HandleFinishDownload(WWW www);  
}
