using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelView : MonoBehaviour {

    private Transform m_Transform;
    private Transform grid_Transform;

    private GameObject prefab_Item;
    private GameObject prefab_Slot;

    public Transform GetTransform()
    {
        return m_Transform;
    }

    public Transform GetGridTransform()
    {
        return grid_Transform;
    }

    public GameObject GetPrefabItem()
    {
        return prefab_Item;
    }

    public GameObject GetPrefabSlot()
    {
        return prefab_Slot;
    }

	void Awake () {
        FindAndInit();
	}

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        grid_Transform = m_Transform.Find("Background/Grid").transform;

        prefab_Item = Resources.Load<GameObject>("Inventory/InventoryItem");
        prefab_Slot = Resources.Load<GameObject>("Inventory/InventorySlot");
    }
	
}
