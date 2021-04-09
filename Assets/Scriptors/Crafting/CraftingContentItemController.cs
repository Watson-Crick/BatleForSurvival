using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingContentItemController : MonoBehaviour {

    private Transform m_Transform;
    private Text m_Text;
    private GameObject m_BG;
    private Button m_Button;

    private int id;

    void Awake()
    {
        FindAndInit();
    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        m_Text = m_Transform.Find("Text").GetComponent<Text>();
        m_BG = m_Transform.Find("ContentItem_BG").gameObject;
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClick);
    }

    /// <summary>
    /// 激活对应Content时,创建合成图谱
    /// </summary>
    public void ActiveBGState()
    {
        m_BG.SetActive(true);
        SendMessageUpwards("CreateSlotContent", id);
    }

    /// <summary>
    /// Content的默认状态,此时需重置合成图谱
    /// </summary>
    public void NormalBGState()
    {
        m_BG.SetActive(false);
        SendMessageUpwards("ResetSlotContent");
    }

    /// <summary>
    /// 按下按钮则重置Content的显示状态
    /// </summary>
    private void ButtonOnClick()
    {
        SendMessageUpwards("ResetContentItem", this);
    }

    public void SetText(CraftingContentItem contentItem)
    {
        m_Text.text = contentItem.ItemName;
        id = contentItem.ItemID;
        gameObject.name = "ContentItem" + id.ToString();
        NormalBGState();
    }
	
}
