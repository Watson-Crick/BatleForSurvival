using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingContentController : MonoBehaviour {

    private Transform grid_Transform;
    private CraftingContentItemController current;

    private List<GameObject> itemList;

    private int index = -1;

	void Awake () {
        FindAndInit();
	}

    private void FindAndInit()
    {
        grid_Transform = gameObject.transform.Find("Grid");
        current = null;

        itemList = new List<GameObject>();
    }

    /// <summary>
    /// 设置当前Content选项卡的信息
    /// </summary>
    public void SetIndex(int index, GameObject prefab, List<CraftingContentItem> contentItemList)
    {
        this.index = index;
        gameObject.name = "Content" + index;
        CreateAllContentItems(contentItemList, prefab);
    }

    /// <summary>
    /// 创建Content选项卡的内容
    /// </summary>
    private void CreateAllContentItems(List<CraftingContentItem> contentItemList, GameObject prefab)
    {
        for (int i = 0; i < contentItemList.Count; i++)
        {
            GameObject item = Instantiate<GameObject>(prefab, grid_Transform);
            item.GetComponent<CraftingContentItemController>().SetText(contentItemList[i]);
            itemList.Add(item);
        }
    }

    /// <summary>
    /// 重置当前选项卡的显示状态
    /// </summary>
    private void ResetContentItem(CraftingContentItemController newContentItem)
    {
        if (current == newContentItem) return;
        if (current != null)
        {
            current.NormalBGState();
            newContentItem.ActiveBGState();
        }
        else
        {
            newContentItem.ActiveBGState();
        }
        current = newContentItem;
    }

    /// <summary>
    /// 当Content选项卡切换时重置之前选项卡内容的显示状态
    /// </summary>
    public void ChangeContent()
    {
        if (current != null)
        {
            current.NormalBGState();
            current = null;
        }
    }
}
