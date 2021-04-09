using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public sealed class JsonTools{

	public static List<T> LoadJson<T>(string path, List<T> list)
    {
        list = new List<T>();
        string text = File.ReadAllText(Application.dataPath + @"/Resources/" +  path);
        JsonData jd = JsonMapper.ToObject(text);
        for (int i = 0; i < jd.Count; i++)
        {
            T temp = JsonMapper.ToObject<T>(jd[i].ToJson());
            list.Add(temp);
        }
        return list;
    }
}
