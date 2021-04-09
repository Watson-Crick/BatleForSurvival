using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialUIController : MonoBehaviour {

    private RectTransform m_RectTransform;
    private RectTransform icon_RectTransform;
    private Image icon_Image;

	void Awake () {
        FindAndInit();
	}
	
    private void FindAndInit()
    {
        m_RectTransform = gameObject.GetComponent<RectTransform>();
        icon_RectTransform = m_RectTransform.Find("MaterialIcon").GetComponent<RectTransform>();
        icon_Image = icon_RectTransform.GetComponent<Image>();
    }

	public void InitUI(float z, Sprite sprite)
    {
        m_RectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
        icon_Image.sprite = sprite;
    }

    public void LightMaterialBG(bool light)
    {
        if (light)
        {
            m_RectTransform.GetComponent<Image>().color = Color.red;
        }else
        {
            m_RectTransform.GetComponent<Image>().color = Color.white;
        }
    }
}
