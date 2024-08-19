using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadUtil : MonoBehaviour
{
    public static IEnumerator DownloadFile(string url, System.Action<string> onComplete)
    {
        if (!url.StartsWith("http:"))
        {
            // Load from Resources
            string resourcePath = url.Replace("resources:", "").TrimStart('/'); // Removing the "resources:" prefix and any starting slashes
            TextAsset asset = Resources.Load<TextAsset>(resourcePath);
            if (asset != null)
            {
                onComplete?.Invoke(asset.text);
            }
            else
            {
                Debug.LogError($"Error: Could not find local resource at {resourcePath}");
            }
            yield break; // Ends the coroutine here for local resources.
        }

        UnityWebRequest request = UnityWebRequest.Get(url);
        // request.certificateHandler = new AcceptAllCertificatesHandler(); // Add this line

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            onComplete?.Invoke(request.downloadHandler.text);
        }
    }
}
