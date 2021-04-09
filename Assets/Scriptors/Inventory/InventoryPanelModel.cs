using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class InventoryPanelModel : MonoBehaviour {

    private List<InventoryItem> m_InventoryItemList;
    private Dictionary<int, InventoryItem> m_InventoryItemDic;

	void Awake () {
        GetData();
    }

    private void GetData()
    {
        m_InventoryItemList = JsonTools.LoadJson<InventoryItem>("InventoryJsonData.txt", m_InventoryItemList);
    }

    public void SaveInventoryToJson(List<GameObject> list)
    {
        List<InventoryItem> itemList = new List<InventoryItem>();
        for (int i = 0; i < list.Count; i++)
        {
            Transform itemTransform = list[i].transform.Find("InventoryItem");

            InventoryItem item;
            if (itemTransform != null)
            {
                InventoryItemController itemController = itemTransform.GetComponent<InventoryItemController>();
                item = new InventoryItem(itemController.Id, itemController.M_Image.sprite.name, itemController.Num, itemController.Bar);
            }
            else
            {
                item = new InventoryItem(0, "", 0, 0);
            }
            itemList.Add(item);
        }

        string str = JsonMapper.ToJson(itemList); 

        File.Delete(Application.dataPath + @"/Resources/InventoryJsonData.txt");
        StreamWriter sw = new StreamWriter(Application.dataPath + @"/Resources/InventoryJsonData.txt");
        sw.Write(str);
        sw.Close();
    }

    public Dictionary<int, InventoryItem> GetInventoryItemDic()
    {
        m_InventoryItemDic = new Dictionary<int, InventoryItem>();
        for (int i = 0, j = 0; i < 27; i++, j++)
        {
            if (j < m_InventoryItemList.Count)
            {
                m_InventoryItemDic.Add(i, m_InventoryItemList[j]);
            }
        }
        return m_InventoryItemDic;
    }
	
}
