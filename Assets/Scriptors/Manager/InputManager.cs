using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    private bool isGun = false;

    private GunControllerBase gunController;
    private GameObject gunStar;

    private GameObject nowPanel;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FindAndInit();
        InventoryPanelController.Instance.SetUI(false);
        BuildPanelController.Instance.ShowOrHide(false);
    }

    private void  FindAndInit()
    {
        gunStar = GameObject.Find("Canvas/MainPanel/GunStar");
        nowPanel = null;
    }

    void Update ()
    {
        InventoryPanelKeyControl();
        ToolBarPanelKeyControl();
    }

    /// <summary>
    /// 背包面板按键控制
    /// </summary>
    private void InventoryPanelKeyControl()
    {
        if (ToolBarPanelController.Instance.CurrentToolModel != null)
        {
            gunController = ToolBarPanelController.Instance.CurrentToolModel.GetComponent<GunControllerBase>();
            isGun = true;
        }
        if (Input.GetKeyDown(GameConst.InventoryPanelKey))
        {
            if (nowPanel == InventoryPanelController.Instance.gameObject)
            {
                //关闭背包
                nowPanel = null;
                BuildPanelController.Instance.ClosePanel();
                CloseInventory();
            }
            else
            {
                //开启背包
                if (nowPanel != InventoryPanelController.Instance.gameObject && nowPanel != null)
                {
                    nowPanel.SetActive(false);
                }
                nowPanel = InventoryPanelController.Instance.gameObject;
                OpenInventory();
            }
        }
    }

    /// <summary>
    /// 关闭背包
    /// </summary>
    private void CloseInventory()
    {
        if (isGun && gunController != null)
        {
            gunController.enabled = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gunStar.SetActive(true);
        ToolBarPanelController.Instance.InventoryState(false);
        InventoryPanelController.Instance.SetUI(false);
    }

    /// <summary>
    /// 开启背包
    /// </summary>
    private void OpenInventory()
    {
        if (isGun && gunController != null)
        {
            gunController.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gunStar.SetActive(false);
        ToolBarPanelController.Instance.InventoryState(true);
        InventoryPanelController.Instance.SetUI(true);
    }

    /// <summary>
    /// 激活BuildPanel
    /// </summary>
    public void BuildPanelActive()
    {
        if (nowPanel != BuildPanelController.Instance.gameObject && nowPanel != null)
        {
            nowPanel.SetActive(false);
            CloseInventory();
        }
        nowPanel = BuildPanelController.Instance.gameObject;
        BuildPanelController.Instance.ShowOrHide(true);
    }

    /// <summary>
    /// 冻结BuildPanel
    /// </summary>
    public void BuildPanelFreeze()
    {
        if (nowPanel == BuildPanelController.Instance.gameObject)
        {
            nowPanel = null;
        }
        BuildPanelController.Instance.ClosePanel();
        BuildPanelController.Instance.ShowOrHide(false);
    }

    /// <summary>
    /// 工具栏面板按键控制
    /// </summary>
    private void ToolBarPanelKeyControl()
    {
        if (nowPanel == null)
        {
            ToolBarKeyControl(GameConst.ToolBarPanelKey_1, 1);
            ToolBarKeyControl(GameConst.ToolBarPanelKey_2, 2);
            ToolBarKeyControl(GameConst.ToolBarPanelKey_3, 3);
            ToolBarKeyControl(GameConst.ToolBarPanelKey_4, 4);
            ToolBarKeyControl(GameConst.ToolBarPanelKey_5, 5);
            ToolBarKeyControl(GameConst.ToolBarPanelKey_6, 6);
            ToolBarKeyControl(GameConst.ToolBarPanelKey_7, 7);
            ToolBarKeyControl(GameConst.ToolBarPanelKey_8, 8);
            ToolBarKeyControl(GameConst.ToolBarPanelKey_9, 9);
        }
    }

    private void ToolBarKeyControl(KeyCode keycode, int keyNum)
    {
        if (Input.GetKeyDown(keycode))
        {
            ToolBarPanelController.Instance.InputKeyStateControl(keyNum - 1);
        }
    }

}
