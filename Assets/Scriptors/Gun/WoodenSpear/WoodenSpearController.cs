using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpearController : ThrowWeaponBase {

    private WoodenSpearView m_WoodenSpearView;
    private WoodenSpearModel m_WoodenSpearModel;

    void Start()
    {
        FindAndInit();
    }

    protected override void FindAndInit()
    {
        base.FindAndInit();

        m_WoodenSpearView = (WoodenSpearView)M_GunViewBase;
        m_WoodenSpearModel = gameObject.GetComponent<WoodenSpearModel>();
    }

    protected override void LoadAudioAsset()
    {
        Audio = Resources.Load<AudioClip>("Audio/Arrow Release");
    }

    protected override void Shoot()
    {
        GameObject temp = Instantiate(m_WoodenSpearView.Prefab_Spear, m_WoodenSpearView.Muzzle_Transform.position, m_WoodenSpearView.Muzzle_Transform.rotation);
        temp.GetComponent<ArrowController>().Shoot(m_WoodenSpearView.Muzzle_Transform.forward, 1000, Demage, Hit);

        Durable -= 1;
        m_WoodenSpearView.M_Animator.SetTrigger("Fire");
    }

}
