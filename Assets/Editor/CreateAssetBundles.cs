using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("AssetBundle/Build AssetBundles")]
    static void BuildeAllAssetBundle()
    {
        string streamPath = Application.streamingAssetsPath;
        if (Directory.Exists(streamPath))
        {
            Directory.Delete(streamPath,true);
        }
        Directory.CreateDirectory(streamPath);
        AssetDatabase.Refresh();

        BuildPipeline.BuildAssetBundles(streamPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
