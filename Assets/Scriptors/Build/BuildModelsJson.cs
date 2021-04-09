using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;
using System.Text;

public class BuildModelsJson : MonoBehaviour {

    private Transform m_Transform;
    private MaterialModelBase[] allBuildModelBase;
    private List<BuildItem> modelList;
    private List<BuildItem> jsonList;
    private List<GameObject> buildItemList;
    private GameObject prefab_Model;

    private string jsonPath = null;

	void Start () {
        FindAndInit();
        JsonToObject();
    }

    private void OnDisable()
    {
        ObjectToJson();
    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        modelList = new List<BuildItem>();
        jsonList = new List<BuildItem>();
        buildItemList = new List<GameObject>();

        jsonPath = Application.dataPath + @"/Resources/ModelsJson.txt";
    }

    private void ObjectToJson()
    {
        allBuildModelBase = m_Transform.GetComponentsInChildren<MaterialModelBase>();

        for (int i = 0; i < allBuildModelBase.Length; i++)
        {
            string name = allBuildModelBase[i].gameObject.name;
            Vector3 pos = allBuildModelBase[i].transform.position;
            Quaternion rot = allBuildModelBase[i].transform.rotation;
            BuildItem item = new BuildItem(name, Math.Round(pos.x, 2).ToString(), Math.Round(pos.y, 2).ToString(), Math.Round(pos.z, 2).ToString()
                , Math.Round(rot.x, 2).ToString(), Math.Round(rot.y, 2).ToString(), Math.Round(rot.z, 2).ToString(), Math.Round(rot.w, 2).ToString());
            modelList.Add(item);
        }

        string str = JsonMapper.ToJson(modelList);
        File.Delete(jsonPath.ToString());

        StreamWriter sw = new StreamWriter(jsonPath);
        sw.Write(str);
        sw.Close();
    }

    private void JsonToObject()
    {
        string textAsset = File.ReadAllText(jsonPath);
        JsonData jsonData = JsonMapper.ToObject(textAsset);

        for (int i = 0; i < jsonData.Count; i++)
        {
            BuildItem item = JsonMapper.ToObject<BuildItem>(jsonData[i].ToJson());
            jsonList.Add(item);
        }

        for (int i = 0; i < jsonList.Count; i++)
        {
            Vector3 pos = new Vector3(float.Parse(jsonList[i].PosX), float.Parse(jsonList[i].PosY), float.Parse(jsonList[i].PosZ));
            Quaternion rot = new Quaternion(float.Parse(jsonList[i].RotX), float.Parse(jsonList[i].RotY), float.Parse(jsonList[i].RotZ), float.Parse(jsonList[i].RotW));

            prefab_Model = Resources.Load<GameObject>("Build/Model/" + jsonList[i].Name);
            GameObject go = Instantiate(prefab_Model, pos, rot, m_Transform);
            go.GetComponent<MaterialModelBase>().Normal();
            go.name = jsonList[i].Name;
            buildItemList.Add(go);
            if (go.name == "Door" || go.name == "Window" || go.name == "Ceiling_Light")
            {
                go.transform.SetParent(buildItemList[i - 1].transform);
            }
        }
    }
}