using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPanelView : MonoBehaviour {

    private Transform m_Transform;
    private Transform tabs_Transform;
    private Transform contents_Transform;
    private Transform center_Transform;
    private Transform right_Transform;

    private GameObject prefab_TabItem;
    private GameObject prefab_Content;
    private GameObject prefab_ContentItem;
    private GameObject prefab_Slot;
    private GameObject prefab_CraftingItem;

    public Transform M_Transform { get { return m_Transform; } }
    public Transform Tabs_Transform { get { return tabs_Transform; } }
    public Transform Contents_Transform { get { return contents_Transform; } }
    public Transform Center_Transform { get { return center_Transform; } }
    public Transform Right_Transform { get { return right_Transform; } }

    public GameObject Prefab_TabItem { get { return prefab_TabItem; } }
    public GameObject Prefab_Content { get { return prefab_Content; } }
    public GameObject Prefab_ContentItem { get { return prefab_ContentItem; } }
    public GameObject Prefab_Slot { get { return prefab_Slot; } }
    public GameObject Prefab_CraftingItem { get { return prefab_CraftingItem; } }

    private List<Sprite> tabIconList;

    private Dictionary<string, Sprite> mapItemDic;

    public List<Sprite> GetTabItemList()
    {
        return tabIconList;
    }

    void Awake()
    {
        FindAndInit();
    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        tabs_Transform = m_Transform.Find("Background/Left/Tabs");
        contents_Transform = m_Transform.Find("Background/Left/Contents");
        center_Transform = m_Transform.Find("Background/Center");
        right_Transform = m_Transform.Find("Background/Right");

        prefab_TabItem = Resources.Load<GameObject>("Crafting/CraftingTabItem");
        prefab_Content = Resources.Load<GameObject>("Crafting/CraftingContent");
        prefab_ContentItem = Resources.Load<GameObject>("Crafting/CraftingContentItem");
        prefab_Slot = Resources.Load<GameObject>("Crafting/CraftingSlot");
        prefab_CraftingItem = Resources.Load<GameObject>("Inventory/InventoryItem");

        ResourcesLoad.LoadAssets("Textures/TabItem", out tabIconList);
        ResourcesLoad.LoadAssets("Textures/Item", out mapItemDic);
    }

    public Sprite ByNameGetMapSprite(string name)
    {
        Sprite temp = null;
        mapItemDic.TryGetValue(name, out temp);
        return temp;
    }

}
