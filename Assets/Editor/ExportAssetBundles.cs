using UnityEngine;
using UnityEditor;

public class ExportAssetBundles
{
    //在Unity编辑器中添加菜单
    [MenuItem("Assets/Build AssetBundle From Selection")]
    static void ExportResourceRGB2()
    {
        // 打开保存面板，获得用户选择的路径
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");

        if (path.Length != 0)
        {
            // 选择的要保存的对象
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //打包
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);
        }
    }

    [MenuItem("Assets/Save Scene")]
    static void ExportScene()
    {
          // 打开保存面板，获得用户选择的路径
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");

        if (path.Length != 0)
        {
            // 选择的要保存的对象
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            string[] scenes = {"Assets/scene1.unity"};
            //打包
            BuildPipeline.BuildPlayer(scenes,path,BuildTarget.StandaloneWindows,BuildOptions.BuildAdditionalStreamedScenes);
        }
    }
}