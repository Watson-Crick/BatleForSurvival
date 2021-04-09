using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHatchetController : MonoBehaviour {

    private StoneHatchetView m_StoneHatchetView;

    public StoneHatchetView M_StoneHatchetView { set { m_StoneHatchetView = value; } get { return m_StoneHatchetView; } }

    //数值字段
    [SerializeField] private int id;
    [SerializeField] private int demage;
    [SerializeField] private int durable;
    [SerializeField] private GunType gunWeaponType;
    private float durableLimit;         //耐久上限


    //数值属性
    public int Id { set { id = value; } get { return id; } }
    public int Demage { set { demage = value; } get { return demage; } }
    public int Durable
    {
        set
        {
            durable = value;
            if (durable <= 0)
            {
                Destroy(gameObject);
            }
        }
        get { return durable; }
    }
    public GunType GunWeaponType { set { gunWeaponType = value; } get { return gunWeaponType; } }

    private GameObject toolBarUI;           //工具栏物品UI
    private Transform rayPoint_Transform;
    private Transform m_Transform;
    RaycastHit hit;

    public GameObject ToolBarUI { set { toolBarUI = value; } get { return toolBarUI; } }

    void Start () {
        FindAndInit();
	}

	void Update () {
        Hit();
	}

    private void FindAndInit()
    {
        m_StoneHatchetView = gameObject.GetComponent<StoneHatchetView>();

        m_Transform = gameObject.transform;
        rayPoint_Transform = m_Transform.Find("RayPoint");

        durableLimit = durable;
    }

    private void Holster()
    {
        m_StoneHatchetView.M_Animator.SetTrigger("Holster");
    }

    private void UpdateUI()
    {
        toolBarUI.GetComponent<InventoryItemController>().UpdateUI(Durable / durableLimit);
    }

    private void Hit()
    {
        HitReady();
        if (Input.GetMouseButtonDown(0))
        {
            m_StoneHatchetView.M_Animator.SetTrigger("Hit");
            Durable--;
            UpdateUI();
        }
    }

    private void HitStone()
    {
        if (hit.collider != null && hit.collider.tag == "Stone")
        {
            hit.collider.GetComponent<BulletMark>().HatchetHit(hit, demage);
        }
    }

    private void HitReady()
    {
        Ray ray = new Ray(rayPoint_Transform.position, rayPoint_Transform.forward);
        Physics.Raycast(ray, out hit, 2, ~(1 << 10));
    }
}
