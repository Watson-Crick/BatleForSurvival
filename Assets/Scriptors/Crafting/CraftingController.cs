using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour {

    private Transform m_Transform;
    private Transform m_SlotTransform;

    private Image m_Image;

    private Button m_ButtonOne;
    private Button m_ButtonAll;

	void Awake () {
        FindAndInit();
        InitButton();

    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        m_SlotTransform = m_Transform.Find("GoodItemSlot");

        m_Image = m_Transform.Find("GoodItemSlot/GoodItem").GetComponent<Image>();

        m_ButtonOne = m_Transform.Find("CraftingButton").GetComponent<Button>();
        m_ButtonAll = m_Transform.Find("CraftingAllButton").GetComponent<Button>();

        m_ButtonOne.onClick.AddListener(ClickButtonOne);
        m_ButtonAll.onClick.AddListener(ClickButtonAll);

        m_Image.gameObject.SetActive(false);
    }

    public void SetSprite(string name)
    {
        m_Image.gameObject.SetActive(true);
        Sprite sprite = Resources.Load<Sprite>("Textures/Item/" + name);
        m_Image.sprite = sprite;
    }

    /// <summary>
    /// 初始化按钮
    /// </summary>
    public void InitButton()
    {
        m_ButtonOne.interactable = false;
        m_ButtonOne.transform.Find("Text").GetComponent<Text>().color = Color.black;
        m_ButtonAll.interactable = false;
        m_ButtonAll.transform.Find("Text").GetComponent<Text>().color = Color.black;
    }

    public void ActiveButton()
    {
        m_ButtonOne.interactable = true;
        m_ButtonOne.transform.Find("Text").GetComponent<Text>().color = Color.white;
        m_ButtonAll.interactable = true;
        m_ButtonAll.transform.Find("Text").GetComponent<Text>().color = Color.white;
    }

    private void ClickButtonOne()
    {
        CraftingPanelController.Instance.CreateCraftingItem(m_SlotTransform, 1);
    }

    private void ClickButtonAll()
    {
        int num = CraftingPanelController.Instance.FindMinCraftingNum();
        CraftingPanelController.Instance.CreateCraftingItem(m_SlotTransform, num);
    }
}
