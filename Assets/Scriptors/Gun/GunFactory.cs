using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFactory : MonoBehaviour {

    public static GunFactory Instance; 

    private Transform m_Transform;

    private GameObject prefab_AssaultRifle;
    private GameObject prefab_Shotgun;
    private GameObject prefab_WoodenBow;
    private GameObject prefab_WoodenSpear;
    private GameObject prefab_StoneHatchet;

    void Awake () {
        Instance = this;
	}

    void Start()
    {
        FindAndInit();
    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;

        prefab_AssaultRifle = Resources.Load<GameObject>("Gun/Prefabs/Assault Rifle");
        prefab_Shotgun = Resources.Load<GameObject>("Gun/Prefabs/Shotgun");
        prefab_WoodenBow = Resources.Load<GameObject>("Gun/Prefabs/Wooden Bow");
        prefab_WoodenSpear = Resources.Load<GameObject>("Gun/Prefabs/Wooden Spear");
        prefab_StoneHatchet = Resources.Load<GameObject>("Gun/Prefabs/Stone Hatchet");
    }
	
    public GameObject CreateGun(string gunName, GameObject ui)
    {
        GameObject temp = null;
        switch (gunName)
        {
            case "Assault Rifle":
                temp = Instantiate(prefab_AssaultRifle, m_Transform);
                InitGunData(temp, 20, 150, GunType.AssaultRifle, ui);
                break;
            case "Shotgun":
                temp = Instantiate(prefab_Shotgun, m_Transform);
                InitGunData(temp, 80, 40, GunType.Shotgun, ui);
                break;
            case "Wooden Bow":
                temp = Instantiate(prefab_WoodenBow, m_Transform);
                InitGunData(temp, 30, 30, GunType.WoodenBow, ui);
                break;
            case "Wooden Spear":
                temp = Instantiate(prefab_WoodenSpear, m_Transform);
                InitGunData(temp, 100, 1, GunType.WoodenSpear, ui);
                break;
            case "Stone Hatchet":
                temp = Instantiate(prefab_StoneHatchet, m_Transform);
                StoneHatchetController gunController = temp.GetComponent<StoneHatchetController>();
                gunController.Demage = 50;
                gunController.Durable = 30;
                gunController.GunWeaponType = GunType.StoneHatchet;
                gunController.ToolBarUI = ui;
                break;
            default:
                break;
        }
        return temp;
    }

    private void InitGunData(GameObject gun, int demage, int durable, GunType type, GameObject ui)
    {
        GunControllerBase gunController = gun.GetComponent<GunControllerBase>();
        gunController.Demage = demage;
        gunController.Durable = durable;
        gunController.GunWeaponType = type;
        gunController.ToolBarUI = ui;
    }
}
