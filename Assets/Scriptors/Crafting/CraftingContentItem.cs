using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingContentItem{

    private int itemID;
    public int ItemID
    {
        set { itemID = value; }
        get { return itemID; }
    }

    private string itemName;
    public string ItemName
    {
        set { itemName = value; }
        get { return itemName; }
    }

    public CraftingContentItem() { }

    public CraftingContentItem(int id, string name)
    {
        itemID = id;
        itemName = name;
    }

    public override string ToString()
    {
        return string.Format("name is {0}, id is {1}", itemName, itemID);
    }
}
