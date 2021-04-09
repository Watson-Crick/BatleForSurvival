using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarPanelView : MonoBehaviour {

    private Transform m_Transform;
    private Transform grid_Transform;

    private GameObject prefab_ToolBarSlot;
    private GameObject gunStar;         //准星UI

    public Transform M_Transform { get { return m_Transform; } }
    public Transform Grid_Transform { get { return grid_Transform; } }

    public GameObject Prefab_ToolBarSlot { get { return prefab_ToolBarSlot; } }
    public GameObject GunStar { get { return gunStar; } }

	void Awake () {
        FindAndInit();
	}

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        grid_Transform = m_Transform.Find("Background/Grid");

        prefab_ToolBarSlot = Resources.Load<GameObject>("ToolBar/ToolBarSlot");
        gunStar = GameObject.Find("Canvas/MainPanel/GunStar");
    }
}
