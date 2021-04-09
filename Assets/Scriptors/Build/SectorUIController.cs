using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectorUIController : MonoBehaviour {

    private RectTransform m_RectTransform;
    private RectTransform icon_RectTransform;

	void Awake () {
        FindAndInit();
	}
	
    private void FindAndInit()
    {
        m_RectTransform = gameObject.GetComponent<RectTransform>();
        icon_RectTransform = m_RectTransform.Find("Icon").GetComponent<RectTransform>();
    }

    public void InitUI(int z, Sprite sprite)
    {
        m_RectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
        icon_RectTransform.rotation = Quaternion.Euler(Vector3.zero);
        if (sprite == null)
        {
            icon_RectTransform.GetComponent<Image>().enabled = false;
        }else
        {
            icon_RectTransform.GetComponent<Image>().sprite = sprite;
            m_RectTransform.GetComponent<Image>().enabled = false;
        }
    }

    public void ShowSectorBG(bool show)
    {
        m_RectTransform.GetComponent<Image>().enabled = show;
    }
}
