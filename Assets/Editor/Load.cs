using System;
using UnityEngine;
using System.Collections;

public class Load: MonoBehaviour
{
    void OnGUI()
    {
        if(GUILayout.Button("All"))
        {
            StartCoroutine(LoadALLGameobject("file://" + Application.dataPath +"/StreamingAssets"+ "/ALL.assetbundle")); 
        }
    }
    private IEnumerator LoadALLGameobject(string path)
    {
        WWW bundle = new WWW(path);
 
		 yield return bundle;
 
		 //通过Prefab的名称把他们都读取出来
		 GameObject  obj0 =  (GameObject)bundle.assetBundle.Load("Cube 1");
         GameObject  obj1 =  (GameObject) bundle.assetBundle.Load("Sphere");

		 //加载到游戏中	
		 yield return Instantiate(obj0);
		 yield return Instantiate(obj1);
		 bundle.assetBundle.Unload(false);

	 }
}