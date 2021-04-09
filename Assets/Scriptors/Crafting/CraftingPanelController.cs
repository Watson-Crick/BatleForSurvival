using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPanelController : MonoBehaviour {

    public static CraftingPanelController Instance;

    private CraftingPanelView m_CraftingPanelView;        //CraftingPanel的V层
    private CraftingPanelModel m_CraftingPanelModel;    //CraftingPanel的M层

    private List<GameObject> tabList;                                 //标题队列
    private List<GameObject> contentList;                          //内容队列 
    private List<GameObject> slotList;                                //槽队列

    private int slotNum;                                                         //合成槽数量
    private int currentTabIndex;                                            //当前标题索引
    private int currentMapIndex;                                           //当前合成图谱索引
    private int mapMaterialsNum;                                         //合成图谱所需材料数量(槽数)

    Dictionary<int, int> itemDic;                                           //当前合成槽内物品信息字典<物品Id, 物品数量>   
    Dictionary<Transform, int> slotDic;                                //当前存放物品的合成槽字典<合成槽Transform组件, 物品数量>

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FindAndInit();
        CreateAllTabsAndContents();
        CreateAllSlot();

        ResetTabsAndContents(0);
    }

    private void FindAndInit()
    {
        tabList =new List<GameObject>();
        contentList = new List<GameObject>();
        slotList = new List<GameObject>();

        slotNum = 25;
        currentTabIndex = -1;
        currentMapIndex = -1;
        mapMaterialsNum = 0;

        m_CraftingPanelModel = gameObject.GetComponent<CraftingPanelModel>();
        m_CraftingPanelView = gameObject.GetComponent<CraftingPanelView>();

        itemDic = new Dictionary<int, int>();
        slotDic = new Dictionary<Transform, int>();
    }

    /// <summary>
    /// 创建Tab和Content选项卡
    /// </summary>
    private void CreateAllTabsAndContents()
    {
        for (int i = 0; i < m_CraftingPanelView.GetTabItemList().Count; i++)
        {
            GameObject item = Instantiate<GameObject>(m_CraftingPanelView.Prefab_TabItem, m_CraftingPanelView.Tabs_Transform);
            item.GetComponent<CraftingTabController>().SetIndex(i, m_CraftingPanelView.GetTabItemList()[i]);
            tabList.Add(item);
            GameObject content = Instantiate<GameObject>(m_CraftingPanelView.Prefab_Content, m_CraftingPanelView.Contents_Transform);
            content.GetComponent<CraftingContentController>().SetIndex(i, m_CraftingPanelView.Prefab_ContentItem, m_CraftingPanelModel.GetContentData("CraftingContentsJsonData")[i]);
            contentList.Add(content);
        }
    }

    /// <summary>
    /// 创建合成图谱槽
    /// </summary>
    private void CreateAllSlot()
    {
        for (int i = 0; i < slotNum; i++)
        {
            GameObject slot = Instantiate<GameObject>(m_CraftingPanelView.Prefab_Slot, m_CraftingPanelView.Center_Transform);
            slot.name = "Slot" + i;
            slotList.Add(slot);
        }
    }

    /// <summary>
    /// 更新当前Tab和Content选项卡
    /// </summary>
    private void ResetTabsAndContents(int index)
    {
        if (currentTabIndex == index) return;
        for (int i = 0; i < tabList.Count; i++)
        {
            tabList[i].GetComponent<CraftingTabController>().SetNormalState();
            contentList[i].GetComponent<CraftingContentController>().ChangeContent();
            contentList[i].SetActive(false);
        }
        tabList[index].GetComponent<CraftingTabController>().SetActiveState();
        contentList[index].SetActive(true);
        currentTabIndex = index;
    }

    /// <summary>
    /// 创建合成图谱
    /// </summary>
    private void CreateSlotContent(int id)
    {
        mapMaterialsNum = 0;
        if (currentMapIndex == id) return;
        CraftingMapItem mapItem = m_CraftingPanelModel.GetMapDataById(id);

        for (int j = 0; j < mapItem.MapContents.Length; j++)
        {
            if (mapItem.MapContents[j] != "0")
            {
                mapMaterialsNum++;
                Sprite sprite = m_CraftingPanelView.ByNameGetMapSprite(mapItem.MapContents[j]);
                slotList[j].GetComponent<CraftingSlotController>().InitSprite(sprite, mapItem.MapContents[j]);
            }
        }
        currentMapIndex = id;

        m_CraftingPanelView.Right_Transform.GetComponent<CraftingController>().SetSprite(mapItem.MapName);
    }

    /// <summary>
    /// 重置合成图谱内容
    /// </summary>
    private void ResetSlotContent()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].GetComponent<CraftingSlotController>().ResetSprite();
        }

        ReturnItemToInventory();
    }

    /// <summary>
    /// 将合成图谱内物品归还到物品槽内
    /// </summary>
    private void ReturnItemToInventory()
    {

        for (int i = 0; i < slotList.Count; i++)
        {
            Transform temp = slotList[i].transform.Find("InventoryItem");
            //如果合成槽内有未回到物品槽的物品,则将其添加进itemDic中
            if (temp != null)
            {
                InventoryItemController tempController = temp.GetComponent<InventoryItemController>();
                if (itemDic.ContainsKey(tempController.Id))
                {
                    itemDic[tempController.Id] += tempController.Num;
                }else
                {
                    itemDic.Add(tempController.Id, tempController.Num);
                }
                Destroy(temp.gameObject);
            }
        }

        //如果合成结果槽内有物品则将其放入背包内,当前该功能有问题,Json文件不是很完整,留待以后Json文件完整后处理
        Transform craftingItemTransform = m_CraftingPanelView.Right_Transform.Find("GoodItemSlot/InventoryItem");
        if (craftingItemTransform != null)
        {
            InventoryItemController craftingItemController = craftingItemTransform.GetComponent<InventoryItemController>();
            itemDic.Add(craftingItemController.Id, craftingItemController.Num);
            Destroy(craftingItemTransform.gameObject);
        }

        InventoryPanelController.Instance.AddItemFromCraftingPanel(itemDic);

        itemDic.Clear();
        slotDic.Clear();
    }

    /// <summary>
    /// 检查并维护当前槽内物品字典
    /// </summary>
    public void InspectAndMaintainSlotDic(GameObject item)
    {
        if (slotDic.ContainsKey(item.transform.parent))
        {
            slotDic[item.transform.parent] = item.transform.parent.Find("InventoryItem").GetComponent<InventoryItemController>().Num;
        }else
        {
            slotDic.Add(item.transform.parent, item.transform.parent.Find("InventoryItem").GetComponent<InventoryItemController>().Num);
        }

        if (slotDic.Count == mapMaterialsNum)
        {
            m_CraftingPanelView.Right_Transform.GetComponent<CraftingController>().ActiveButton();
        }
    }

    /// <summary>
    /// 寻找合成槽内最少的物品数量
    /// </summary>
    public int FindMinCraftingNum()
    {
        int num = 64;
        foreach (KeyValuePair<Transform, int> kvp in slotDic)
        {
            if (kvp.Value < num)
            {
                num = kvp.Value;
            }
        }

        return num;
    }

    /// <summary>
    /// 合成物品
    /// </summary>
    public void CreateCraftingItem(Transform tempTransform, int num)
    {
        Transform craftingItemTransform = m_CraftingPanelView.Right_Transform.Find("GoodItemSlot/InventoryItem");
        if (craftingItemTransform == null)
        {
            CraftingMapItem craftingMapItem = m_CraftingPanelModel.GetMapDataById(currentMapIndex);

            GameObject craftingItem = Instantiate<GameObject>(m_CraftingPanelView.Prefab_CraftingItem, tempTransform);
            craftingItem.name = "InventoryItem";
            craftingItem.GetComponent<InventoryItemController>().SetImageAndText(craftingMapItem.MapName, num, craftingMapItem.MapId, 1);
            craftingItem.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 130);
            craftingItem.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 130);
        }else
        {
            craftingItemTransform.GetComponent<InventoryItemController>().Num += num;
        }

        ReduceSlotItem(num);
    }

    /// <summary>
    /// 减少槽内物品
    /// </summary>
    private void ReduceSlotItem(int num)
    {
        //通过将字典内物品全部提取出来,然后判断那些物品该被删除哪些该被保留
        //然后清空字典,再将该被保留的物体重新存入字典
        List<Transform> deleteList = new List<Transform>();
        List<Transform> saveList = new List<Transform>();
        List<int> numList = new List<int>();

        foreach (KeyValuePair<Transform, int> kvp in slotDic)
        {
            int result = slotDic[kvp.Key] - num;
            if (result == 0)
            {
                deleteList.Add(kvp.Key);
            }else
            {
                saveList.Add(kvp.Key);
                numList.Add(result);
            }
        }

        slotDic.Clear();

        if (deleteList.Count > 0)
        {
            m_CraftingPanelView.Right_Transform.GetComponent<CraftingController>().InitButton();
        }

        for (int i = 0; i < deleteList.Count; i++)
        {
            Destroy(deleteList[i].Find("InventoryItem").gameObject);
        }

        for (int i = 0; i < saveList.Count; i++)
        {
            saveList[i].Find("InventoryItem").GetComponent<InventoryItemController>().Num = numList[i];
            slotDic.Add(saveList[i], numList[i]);
        }

        deleteList.Clear();
        saveList.Clear();
        numList.Clear();
    }

    /// <summary>
    /// 输出合成槽字典数据用于测试
    /// </summary>
    private void PrintSlotDicData<T1, T2>(Dictionary<T1,T2> dic)
    {
        foreach (KeyValuePair<T1, T2> kvp in dic)
        {
            Debug.Log(kvp.Key + " has " + kvp.Value + " item");
        }
    }
}
