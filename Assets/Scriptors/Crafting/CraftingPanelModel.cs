using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class CraftingPanelModel : MonoBehaviour {

    private List<List<CraftingContentItem>> m_ContentItemList;
    private Dictionary<int, CraftingMapItem> m_MapItemDic;

    void Awake()
    {
        GetMapData("CraftingMapJsonData");
    }

    public List<List<CraftingContentItem>> GetContentData(string name)
    {
        m_ContentItemList = new List<List<CraftingContentItem>>();
        TextAsset text = Resources.Load<TextAsset>(name);

        JsonData jsondata = JsonMapper.ToObject(text.text);
        for (int i = 0; i < jsondata.Count; i++)
        {
            JsonData jd = jsondata[i]["Type"];
            List<CraftingContentItem> tempList = new List<CraftingContentItem>();
            for (int j = 0; j < jd.Count; j++)
            {
                CraftingContentItem itemData = JsonMapper.ToObject<CraftingContentItem>(jd[j].ToJson());
                tempList.Add(itemData);
            }
            m_ContentItemList.Add(tempList);
        }
        return m_ContentItemList;
    }

    private void GetMapData(string name)
    {
        m_MapItemDic = new Dictionary<int, CraftingMapItem>();
        TextAsset text = Resources.Load<TextAsset>(name);
        JsonData jd = JsonMapper.ToObject(text.text);
        for (int i = 0; i < jd.Count; i++)
        {
            int mapID = int.Parse(jd[i]["MapId"].ToString());
            string tempStr = jd[i]["MapContents"].ToString();
            string[] mapContents = tempStr.Split(',');
            string mapName = jd[i]["MapName"].ToString();
            m_MapItemDic.Add(mapID, new CraftingMapItem(mapID, mapContents, mapName));
        }
    }

    public CraftingMapItem GetMapDataById(int id)
    {
        CraftingMapItem temp = null;
        m_MapItemDic.TryGetValue(id, out temp);
        return temp;
    }
}
