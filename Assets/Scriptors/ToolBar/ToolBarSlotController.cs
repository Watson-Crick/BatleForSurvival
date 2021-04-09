using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarSlotController : MonoBehaviour {

    private Transform m_Transform;

    private Text m_Text;

	void Awake () {
        FindAndInit();
	}

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        
        m_Text = m_Transform.Find("Key").GetComponent<Text>();
    }

    public void Init(string name, int key)
    {
        gameObject.name = name;
        m_Text.text = key.ToString();
    }

    public void ShowBG(bool isShow)
    {
        if (isShow)
        {
            m_Transform.GetComponent<Image>().color = Color.red;
        }else
        {
            m_Transform.GetComponent<Image>().color = Color.white;
        }
    }
}
