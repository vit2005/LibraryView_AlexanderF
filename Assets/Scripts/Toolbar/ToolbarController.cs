using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    private static ToolbarController _instance;
    public static ToolbarController Instance => _instance;

    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] GameObject dontDestroyOnLoadParent;

    private Dictionary<ToolbarElementType, ToolbarElementBase> instances = new Dictionary<ToolbarElementType, ToolbarElementBase>();

    public void Awake()
    {
        _instance = this;

        foreach (var element in prefabs)
        {
            var instance = Instantiate(element, transform);
            var component = instance.GetComponent<ToolbarElementBase>();
            instances.Add(component.ToolbarElementType, component);
        }

        DontDestroyOnLoad(dontDestroyOnLoadParent);
    }

    public void Clear()
    {
        foreach (var element in instances.Values)
        {
            element.gameObject.SetActive(false);
        }
    }

    public Dictionary<ToolbarElementType, ToolbarElementBase> Show(ToolbarElementType buttons)
    {
        Clear();

        Dictionary<ToolbarElementType, ToolbarElementBase> result = new Dictionary<ToolbarElementType, ToolbarElementBase>();

        foreach (var type in Enum.GetValues(typeof(ToolbarElementType)))
        {
            ToolbarElementType t = (ToolbarElementType)type;

            if ((buttons & t) != 0 && instances.ContainsKey(t))
            {
                instances[t].gameObject.SetActive(true);
                result.Add(t, instances[t]);
            }
        }

        return result;
    }
}
