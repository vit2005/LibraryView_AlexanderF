using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TestDownloadAsset : MonoBehaviour
{
    [SerializeField] private StagesManager manager;

    private string url = "https://www.dropbox.com/scl/fi/3kaxml7sswgjryibfglcv/stagesconfigassetbundle?rlkey=ep4pgpxl2t3scb4gva8a25fqj&st=q1pm1lzy&dl=1"; // URL до твого ассет-файлу

    void Start()
    {
        StartCoroutine(DownloadAndLoadStagesConfig());
    }

    IEnumerator DownloadAndLoadStagesConfig()
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

            if (bundle != null)
            {
                StagesConfig stagesConfig = bundle.LoadAsset<StagesConfig>("StagesConfig");

                if (stagesConfig != null)
                {
                    Debug.Log("StagesConfig loaded successfully!");
                    manager.Init(stagesConfig);
                }

                //bundle.Unload(false);
            }
        }
        else
        {
            Debug.LogError("AssetBundle loading error: " + request.error);
        }
    }
}
