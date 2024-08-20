using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineRun : MonoBehaviour
{
    [SerializeField] private StagesManager manager;
    [SerializeField] private StagesConfig config;

    void Start()
    {
        manager.Init(config);
    }
}
