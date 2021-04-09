using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourcesLoad{

    public static Dictionary<string, Sprite> LoadAssets(string path, out Dictionary<string, Sprite> dic)
    {
        dic = new Dictionary<string, Sprite>();
        Sprite[] sprite = Resources.LoadAll<Sprite>(path);
        for (int i = 0; i < sprite.Length; i++)
        {
            dic.Add(sprite[i].name, sprite[i]);
        }
        return dic;
    }

    public static List<Sprite> LoadAssets(string path, out List<Sprite> list)
    {
        list = new List<Sprite>();
        Sprite[] sprite = Resources.LoadAll<Sprite>(path);
        for (int i = 0; i < sprite.Length; i++)
        {
            list.Add(sprite[i]);
        }
        return list;
    }
}
