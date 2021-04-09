using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem{

    private int itemId;
    public int ItemId
    {
        set { itemId = value; }
        get { return itemId; }
    }

    private string itemName;
    public string ItemName
    {
        set { itemName = value; }
        get { return itemName; }
    }

    private int itemNum;
    public int ItemNum
    {
        set { itemNum = value; }
        get { return itemNum; }
    }

    private string itemBar;
    public string ItemBar
    {
        set { itemBar = value; }
        get { return itemBar; }
    }

    public InventoryItem() { }

    public InventoryItem(int id, string name, int num, float bar)
    {
        itemId = id;
        itemName = name;
        itemNum = num;
        itemBar = bar.ToString();
    }

    public override string ToString()
    {
        return string.Format("item id is {0}, name is {1}, num is {2}, bar is {3}", itemId, itemName, itemNum, itemBar);
    }
}
