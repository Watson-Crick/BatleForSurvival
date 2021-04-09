using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTabController : MonoBehaviour {

    private Transform m_Transform;
    private Button m_Button;
    private GameObject m_Background;
    private Image m_Icon;

    private int index = -1;

    void Awake()
    {
        FindAndInit();
    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        m_Button = gameObject.GetComponent<Button>();
        m_Background = m_Transform.Find("TabItem_BG").gameObject;
        m_Icon = m_Transform.Find("TabItem_Icon").GetComponent<Image>();

        m_Button.onClick.AddListener(ButtonOnClick);
    }

    public void SetIndex(int index, Sprite sprite)
    {
        this.index = index;
        gameObject.name = "TabItem" + index;
        SetItemIcon(sprite);
    }

    public void SetNormalState()
    {
        if (m_Background.activeSelf == false)
        {
            m_Background.SetActive(true);
        }
    }

    public void SetActiveState()
    {
        m_Background.SetActive(false);
    }

    private void ButtonOnClick()
    {
        SendMessageUpwards("ResetTabsAndContents", index);
    }

    private void SetItemIcon(Sprite sprite)
    {
        m_Icon.sprite = sprite;
    }
}
