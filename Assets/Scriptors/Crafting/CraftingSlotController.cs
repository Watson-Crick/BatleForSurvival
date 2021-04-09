using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotController : MonoBehaviour {

    private Transform m_Transform;
    private Image m_Image;

    private bool isOpen;
    public bool IsOpen { get { return isOpen; } }

    private int id;
    public int Id
    {
        set { id = value; }
        get { return id; }
    }
	
	void Awake () {
        FindAndInit();
	}
	
	private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        m_Image = m_Transform.Find("Item").GetComponent<Image>();
        m_Image.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
        m_Image.gameObject.SetActive(false);
        isOpen = false;
    }

    public void InitSprite(Sprite sprite, string id)
    {
        m_Image.gameObject.SetActive(true);
        m_Image.sprite = sprite;
        isOpen = true;
        this.id = int.Parse(id);
    }

    public void ResetSprite()
    {
        m_Image.gameObject.SetActive(false);
        id = -1;
    }
}
