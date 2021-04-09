using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private RectTransform m_RectTransform;  //自身RectTransform组件
    private Transform self_Parent;                     //父物体Transform组件
    private Transform parent;                             //拖拽中临时父物体Transform组件 
    private Transform target_Transform;           //目标物体Transform组件 

    private CanvasGroup m_CanvasGroup;      //自身Canvas组件

    private Image m_Image;                              //自身Image组件
    private Text m_Text;                                    //自身Text组件
    private Image bar_Image;                            //耐久条Image组件

    public Image M_Image { set { m_Image = value; } get { return m_Image; } }
    public Text M_Text { set { m_Text = value; } get { return m_Text; } }

    private int id;                                                //物品id信息
    public int Id { set { id = value; } get { return id; } }
    private int num;                                            //物品数量信息
    public int Num
    {
        set
        {
            num = value;
            m_Text.text = num.ToString();
        }
        get { return num; }
    }
    private float bar;                                               //物品是否存在耐久
    public float Bar { set { bar = value; } get { return bar; } }

    private bool isDrag;                                      //是否在拖拽中的状态判断
    private bool isBreakToCrafting;                   //是否被拆分到合成槽的状态判断

	void Awake () {
        FindAndInit();
    }

    void Update()
    {
        DragItemOperating();
    }

    private void FindAndInit()
    {
        m_RectTransform = gameObject.GetComponent<RectTransform>();
        target_Transform = null;
        self_Parent = m_RectTransform.parent;
        parent = GameObject.Find("Canvas").transform;

        m_CanvasGroup = m_RectTransform.GetComponent<CanvasGroup>();

        m_Image = gameObject.GetComponent<Image>();
        m_Text = m_RectTransform.Find("Text").GetComponent<Text>();
        bar_Image = m_RectTransform.Find("Bar").GetComponent<Image>();

        id = -1;
        num = -1;

        isDrag = false;
        isBreakToCrafting = false;
    }

    /// <summary>
    /// 设置物品信息
    /// </summary>
    public void SetImageAndText(string imageName, int textData, int id, float bar)
    {
        m_Image.sprite = Resources.Load<Sprite>("Textures/Item/" + imageName);
        m_Text.text = textData.ToString();
        this.id = id;
        bar_Image.fillAmount = bar;
        this.bar = bar;
        num = textData;
        BarOrText();
    }

    /// <summary>
    /// 显示耐久条/数量
    /// </summary>
    private void BarOrText()
    {
        if (bar == 0)
        {
            bar_Image.gameObject.SetActive(false);
        }else if (bar > 0)
        {
            m_Text.gameObject.SetActive(false);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        m_RectTransform.SetParent(parent);
        m_CanvasGroup.blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        GameObject target = eventData.pointerEnter;

        #region  当拖拽位置发生改变且其位于一下三个标签物体的范围内时重置target_Transform组件信息
        if (target != null)
        {
            if (target.tag == "InventorySlot" || target.tag == "CraftingSlot" || target.tag == "InventoryItem")
            {
                target_Transform = target.transform;
            }else
            {
                target_Transform = null;
            }
        }
        #endregion

        Vector3 pos = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(m_RectTransform, eventData.position, eventData.enterEventCamera, out pos);
        m_RectTransform.position = pos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        GameObject target = eventData.pointerEnter;

        DragItemLogic(target);

        m_CanvasGroup.blocksRaycasts = true;
        m_RectTransform.localPosition = Vector3.zero;

        isDrag = false;
    }

    /// <summary>
    /// 重置图片尺寸
    /// </summary>
    private void ResetWidthAndHeight(RectTransform rectTransform, float width, float height)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    /// <summary>
    /// 物品拖拽逻辑
    /// </summary>
    /// <param name="target"></param>
    private void DragItemLogic(GameObject target)
    {
        //判断拖拽目标位置是否是非UI部分
        if (target != null)
        {
            Transform targetTransform = target.transform;

            #region  判断拖拽位置是否为Inventory槽
            if (target.tag == "InventorySlot")
            {
                m_RectTransform.SetParent(targetTransform);
                ResetWidthAndHeight(m_RectTransform, 85, 85);
            }
            else
            {
                m_RectTransform.SetParent(self_Parent);
            }
            #endregion

            #region  物体交换
            if (target.tag == "InventoryItem")
            {
                //物品id相同则进行合并,不同就交换
                if (target.GetComponent<InventoryItemController>().id == id)
                {
                    MergeItem();

                    if (target_Transform.parent.tag == "CraftingSlot")
                    {
                        InventoryPanelController.Instance.SendInspectAndMaintainSlotDicForCraftingPanel(target);
                    }
                }
                else
                {
                    m_RectTransform.SetParent(targetTransform.parent);
                    targetTransform.SetParent(self_Parent);
                    targetTransform.localPosition = Vector3.zero;
                    target.GetComponent<InventoryItemController>().self_Parent = self_Parent;
                    self_Parent = m_RectTransform.parent;
                }
            }
            #endregion

            #region  判断拖拽位置是否为合成槽
            if (target.tag == "CraftingSlot")
            {
                CraftingSlotController targetController = target.GetComponent<CraftingSlotController>();
                //判断合成槽内是否可以加入物品
                if (targetController.IsOpen == true)
                {
                    //判断合成槽内物品与物品槽内物品是否相同
                    if (targetController.Id == id)
                    {
                        m_RectTransform.SetParent(targetTransform);
                        ResetWidthAndHeight(m_RectTransform, 75, 75);
                        self_Parent = m_RectTransform.parent;
                        InventoryPanelController.Instance.SendInspectAndMaintainSlotDicForCraftingPanel(gameObject);
                    }
                    else
                    {
                        m_RectTransform.SetParent(self_Parent);
                    }
                }
                else
                {
                    m_RectTransform.SetParent(self_Parent);
                }
            }
            #endregion
        }
        else
        {
            m_RectTransform.SetParent(self_Parent);
        }
    }

    /// <summary>
    /// 物品拖拽时按鼠标右键进行拆分,添加以及合并操作
    /// </summary>
    private void DragItemOperating()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isDrag)
            {
                if (target_Transform != null)
                {
                    if (target_Transform.tag == "InventoryItem")
                    {
                        if (target_Transform.GetComponent<InventoryItemController>().id == id)
                        {
                            AddItem();
                        }
                    }

                    //判断拖拽位置是否为合成槽
                    if (target_Transform.tag == "CraftingSlot")
                    {
                        CraftingSlotController targetController = target_Transform.GetComponent<CraftingSlotController>();
                        //判断合成槽内是否可以加入物品
                        if (targetController.IsOpen == true)
                        {
                            //判断合成槽内物品与物品槽内物品是否相同
                            if (targetController.Id == id)
                            {
                                isBreakToCrafting = true;
                                BreakItem();
                            }
                        }
                    }

                    if (target_Transform.tag == "InventorySlot")
                    {
                        BreakItem();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 拆分物品
    /// </summary>
    private void BreakItem()
    {
        //创建物品
        GameObject temp = Instantiate<GameObject>(gameObject, target_Transform);
        temp.name = "InventoryItem";
        //鼠标不动时target_Transform不会更新导致bug
        target_Transform = temp.transform;

        //物品属性信息重置
        InventoryItemController tempController = temp.GetComponent<InventoryItemController>();
        tempController.Num = 1;
        tempController.id = id;
        Num -= 1;

        //物品位置,大小等信息重置
        RectTransform tempTransform = temp.GetComponent<RectTransform>();
        tempTransform.localPosition = Vector3.zero;
        tempTransform.localScale = Vector3.one;
        if (isBreakToCrafting)
        {
            ResetWidthAndHeight(tempTransform, 75, 75);
            InventoryPanelController.Instance.SendInspectAndMaintainSlotDicForCraftingPanel(temp);
        }

        //启用射线检查
        temp.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// 合并物品
    /// </summary>
    private void MergeItem()
    {
        InventoryItemController targetController = target_Transform.GetComponent<InventoryItemController>();
        targetController.Num += Num;
        Destroy(gameObject);
    }

    /// <summary>
    /// 对目标物品进行添加
    /// </summary>
    private void AddItem()
    {
        InventoryItemController targetController = target_Transform.GetComponent<InventoryItemController>();
        targetController.Num += 1;
        Num -= 1;
        if (target_Transform.parent.tag == "CraftingSlot")
        {
            InventoryPanelController.Instance.SendInspectAndMaintainSlotDicForCraftingPanel(target_Transform.gameObject);
        }
    }

    /// <summary>
    /// 对自身进行添加
    /// </summary>
    /// <param name="num"></param>
    public void AddItem(int num)
    {
        Num += num;
    }

    public void UpdateUI(float fill)
    {
        bar_Image.fillAmount = fill;
        bar = fill;
        if (fill == 0)
        {
            InventoryPanelController.Instance.MaintainDicForToolBarPanel(gameObject);
            Destroy(gameObject);
        }
    }
}
