using UnityEditor;
using UnityEngine;

public class CreateAssetBundle
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        // Створює AssetBundles у папці "Assets/AssetBundles"
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
        Debug.Log("AssetBundles зібрані!");
    }
}
