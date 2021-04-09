using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelController : MonoBehaviour, IUIPanelShowHide {

    public static InventoryPanelController Instance;

    private InventoryPanelModel m_InventoryPanelModel;  //InventoryPanel的M层
    private InventoryPanelView m_InventoryPanelView;      //InventoryPanel的V层  

    private int slotNum = 27;                                                   //物品槽数量

    private List<GameObject> m_InventorySlotList;              //物品槽队列

    private void Awake()
    {
        Instance = this;
        FindAndInit();
    }

    void Start() {
        CreateAllSlot();
        CreateAllItem();
    }

    void OnDisable()
    {
        m_InventoryPanelModel.SaveInventoryToJson(m_InventorySlotList);
    }

    private void FindAndInit()
    {
        m_InventorySlotList = new List<GameObject>();

        m_InventoryPanelModel = gameObject.GetComponent<InventoryPanelModel>();
        m_InventoryPanelView = gameObject.GetComponent<InventoryPanelView>();
    }

    /// <summary>
    /// 创建所有物品槽
    /// </summary>
    private void CreateAllSlot()
    {
        for (int i = 0; i < slotNum; i++)
        {
            GameObject slot = Instantiate<GameObject>(m_InventoryPanelView.GetPrefabSlot(), m_InventoryPanelView.GetGridTransform());
            m_InventorySlotList.Add(slot);
        }
    }

    /// <summary>
    /// 创建所有物品
    /// </summary>
    private void CreateAllItem()
    {
        Dictionary<int, InventoryItem> itemDic = m_InventoryPanelModel.GetInventoryItemDic();
        int index = 0;
        foreach (KeyValuePair<int, InventoryItem> kvp in itemDic)
        {
            if (kvp.Value.ItemName != "")
            {
                GameObject item = Instantiate<GameObject>(m_InventoryPanelView.GetPrefabItem(), m_InventorySlotList[index].transform);
                item.GetComponent<InventoryItemController>().SetImageAndText(kvp.Value.ItemName, kvp.Value.ItemNum, kvp.Value.ItemId, float.Parse(kvp.Value.ItemBar));
                item.name = "InventoryItem";
            }
            index++;
        }
        index = 0;
    }

    /// <summary>
    /// 从合成槽内获取物品添加入物品槽中
    /// </summary>
    /// <param name="itemDic"></param>
    public void AddItemFromCraftingPanel(Dictionary<int, int> itemDic)
    {
        List<Transform> nullSlotList = new List<Transform>();
        for (int i = 0; i < m_InventorySlotList.Count; i++)
        {
            Transform itemTransform = m_InventorySlotList[i].transform.Find("InventoryItem");

            //如果当前物品槽无物品则放入nullSlotList中备用
            if (itemTransform == null)
            {
                nullSlotList.Add(m_InventorySlotList[i].transform);
                continue;
            }

            InventoryItemController itemController = itemTransform.GetComponent<InventoryItemController>();
            //判断物品槽内是否是合成槽内相关物品
            if (itemDic.ContainsKey(itemController.Id))
            {
                int num;
                itemDic.TryGetValue(itemController.Id, out num);
                itemDic[itemController.Id] -= num;
                itemController.AddItem(num);
            }
        }

        //当物品槽没有相关物品时创建新物品
        int index = 0;
        foreach (KeyValuePair<int, int> kvp in itemDic)
        {
            if (kvp.Value != 0)
            {
                CreateNewItem(nullSlotList[index], kvp.Key, kvp.Value);
                index++;
            }
        }
        index = 0;
    }

    /// <summary>
    /// 创建新物品
    /// </summary>
    private void CreateNewItem(Transform nullSlotTransform, int key, int value)
    {
        Dictionary<int, InventoryItem> itemDic = m_InventoryPanelModel.GetInventoryItemDic();
        InventoryItem temp = null;
        foreach (KeyValuePair<int, InventoryItem> kvp in itemDic)
        {
            if (kvp.Value.ItemId == key)
            {
                temp = kvp.Value;
                break;
            }
        }

        GameObject item = Instantiate<GameObject>(m_InventoryPanelView.GetPrefabItem(), nullSlotTransform);
        item.GetComponent<InventoryItemController>().SetImageAndText(temp.ItemName, value, key, 1);
        item.name = "InventoryItem";
    }

    public void SendInspectAndMaintainSlotDicForCraftingPanel(GameObject item)
    {
        CraftingPanelController.Instance.InspectAndMaintainSlotDic(item);
    }

    public void MaintainDicForToolBarPanel(GameObject item)
    {
        ToolBarPanelController.Instance.MaintainDic(item);
    }

    public void SetUI(bool inventoryState)
    {
        if (inventoryState)
        {
            m_InventoryPanelView.GetTransform().GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        }else
        {
            m_InventoryPanelView.GetTransform().GetComponent<RectTransform>().offsetMin = new Vector2(9999, 0);
        }
    }

    /// <summary>
    /// 拾取物品
    /// </summary>
    /// <param name="name"></param>
    public void AddItemFromWorld(string name)
    {
        List<Transform> nullSlotList = new List<Transform>();
        for (int i = 0; i < m_InventorySlotList.Count; i++)
        {
            Transform itemTransform = m_InventorySlotList[i].transform.Find("InventoryItem");

            //如果当前物品槽无物品则放入nullSlotList中备用
            if (itemTransform == null)
            {
                nullSlotList.Add(m_InventorySlotList[i].transform);
                continue;
            }

            InventoryItemController itemController = itemTransform.GetComponent<InventoryItemController>();
            //判断物品槽内是否是拾取到的物品
            if (itemController.GetComponent<Image>().sprite.name == name)
            {
                itemController.M_Text.text = (int.Parse(itemController.M_Text.text) + 1).ToString();
            }
        }
    }
}
