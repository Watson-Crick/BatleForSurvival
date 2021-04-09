using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarPanelController : MonoBehaviour {

    public static ToolBarPanelController Instance;

    private ToolBarPanelView m_ToolBarPanelView;
    private ToolBarPanelModel m_ToolBarPanelModel;

    private List<GameObject> slotList;          //工具栏列表
    private Dictionary<GameObject, GameObject> toolBarDic;          //工具栏与模型的字典,用于两者产生关联

    private GameObject currentToolModel;            //当前工具模型,用于工具模型切换
    private Transform inventoryItemTransform;           //当前工具栏物品的Transform组件

    private int slotNum;            //工具栏数量
    private int mouseIndex;         //鼠标索引
    private bool canMouseScroll;    //鼠标是否可以滚动

    public int MouseIndex { set { mouseIndex = value; } get { return mouseIndex; } }
    public bool CanMouseScroll {  set { canMouseScroll = value; } }
    public GameObject CurrentToolModel { get { return currentToolModel; } }

    void Awake()
    {
        Instance = this;
    }

    void Start () {
        FindAndInit();
        CreateAllSlot();
    }

    private void Update()
    {
        if (canMouseScroll)
        {
            MouseScrollWheelStateControl();
        }
    }

    private void FindAndInit()
    {
        m_ToolBarPanelView = gameObject.GetComponent<ToolBarPanelView>();
        m_ToolBarPanelModel = gameObject.GetComponent<ToolBarPanelModel>();

        slotList = new List<GameObject>();
        toolBarDic = new Dictionary<GameObject, GameObject>();

        currentToolModel = null;
        inventoryItemTransform = null;

        slotNum = 9;
        mouseIndex = 0;
        canMouseScroll = true;

        m_ToolBarPanelView.GunStar.SetActive(false);
    }

    /// <summary>
    /// 创建所有工具槽
    /// </summary>
    private void CreateAllSlot()
    {
        for (int i = 0; i < slotNum; i++)
        {
            GameObject go = Instantiate<GameObject>(m_ToolBarPanelView.Prefab_ToolBarSlot, m_ToolBarPanelView.Grid_Transform);
            go.transform.GetComponent<ToolBarSlotController>().Init(m_ToolBarPanelView.Prefab_ToolBarSlot.name + i.ToString(), i + 1);

            slotList.Add(go);
        }

        slotList[mouseIndex].GetComponent<ToolBarSlotController>().ShowBG(true);
    }

    /// <summary>
    /// 鼠标滚轮控制工具槽
    /// </summary>
    private void MouseScrollWheelStateControl()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //滚轮向后滚
            if (mouseIndex < slotNum - 1)
            {
                slotList[mouseIndex].GetComponent<ToolBarSlotController>().ShowBG(false);
                mouseIndex++;
                slotList[mouseIndex].GetComponent<ToolBarSlotController>().ShowBG(true);
            }
            StartCoroutine(CallGunFactory(0.5f));
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //滚轮向前滚
            if (mouseIndex > 0)
            {
                slotList[mouseIndex].GetComponent<ToolBarSlotController>().ShowBG(false);
                mouseIndex--;
                slotList[mouseIndex].GetComponent<ToolBarSlotController>().ShowBG(true);
            }
            StartCoroutine(CallGunFactory(0.5f));
        }
    }

    /// <summary>
    /// 输入按键控制工具槽
    /// </summary>
    public void InputKeyStateControl(int keyNum)
    {
        slotList[mouseIndex].GetComponent<ToolBarSlotController>().ShowBG(false);
        mouseIndex = keyNum;
        slotList[mouseIndex].GetComponent<ToolBarSlotController>().ShowBG(true);
        StartCoroutine(CallGunFactory(0.5f));
    }

    /// <summary>
    /// 调用工厂类创建模型
    /// </summary>
    public IEnumerator CallGunFactory(float time)
    {
        inventoryItemTransform = slotList[mouseIndex].transform.Find("InventoryItem");
        //判断工具栏内是否有物品
        if (inventoryItemTransform != null)
        {

            //切换模型需将当前工具模型设为隐藏
            if (currentToolModel != null && currentToolModel.activeSelf == true)
            {
                if (currentToolModel.tag == "CollectTool")
                {
                    currentToolModel.GetComponent<StoneHatchetController>().M_StoneHatchetView.M_Animator.SetTrigger("Holster");
                }else
                {
                    currentToolModel.GetComponent<GunControllerBase>().M_GunViewBase.M_Animator.SetTrigger("Holster");
                }
                m_ToolBarPanelView.GunStar.SetActive(false);
                yield return new WaitForSeconds(time);
            }

            GameObject tempItem = inventoryItemTransform.gameObject;
            GameObject tempTool = null;
            toolBarDic.TryGetValue(tempItem, out tempTool);
            //判断字典中物品是否有对应模型
            if (tempTool == null)
            {
                //没有对应模型则生成对应模型并将两者加入字典
                tempTool = GunFactory.Instance.CreateGun(tempItem.GetComponent<Image>().sprite.name, tempItem);
                toolBarDic.Add(tempItem, tempTool);
            }else
            {
                //有对应模型则将对应模型显示并且设为当前工具模型
                tempTool.SetActive(true);
                m_ToolBarPanelView.GunStar.SetActive(true);
            }
            currentToolModel = tempTool;
        }else if (inventoryItemTransform == null && currentToolModel != null)         //有物品切换到无物品
        {
            if (currentToolModel.tag == "CollectTool")
            {
                currentToolModel.GetComponent<StoneHatchetController>().M_StoneHatchetView.M_Animator.SetTrigger("Holster");
            }
            else
            {
                currentToolModel.GetComponent<GunControllerBase>().M_GunViewBase.M_Animator.SetTrigger("Holster");
            }
            m_ToolBarPanelView.GunStar.SetActive(false);
            currentToolModel = null;
        }
        PlayerController.Instance.BuildEnd(currentToolModel);
    }

    /// <summary>
    /// 维护字典
    /// </summary>
    public void MaintainDic(GameObject temp)
    {
        toolBarDic.Remove(temp);
    }

    /// <summary>
    /// 获取字典内数据
    /// </summary>
    private GameObject GetDicData(GameObject key)
    {
        GameObject value = null;
        toolBarDic.TryGetValue(key, out value);
        if(value == null)
        {
            StartCoroutine(CallGunFactory(0));
            toolBarDic.TryGetValue(key, out value);
        }
        return value;
    }

    /// <summary>
    /// 背包开关控制
    /// </summary>
    public void InventoryState(bool state)
    {
        inventoryItemTransform = slotList[mouseIndex].transform.Find("InventoryItem");
        if (state)          //背包打开
        {
            if (inventoryItemTransform != null)
            {
                GetDicData(inventoryItemTransform.gameObject).SetActive(false);
            }
        }else           //背包关闭
        {
            if (inventoryItemTransform != null)
            {
                GetDicData(inventoryItemTransform.gameObject).SetActive(true);
            }
        }
    }
}
