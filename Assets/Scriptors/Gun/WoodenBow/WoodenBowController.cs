using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBowController : ThrowWeaponBase {

    private WoodenBowView m_WoodenBowView;
    private WoodenBowModel m_WoodenBowModel;

	void Start () {
        FindAndInit();
	}

    protected override void FindAndInit()
    {
        base.FindAndInit();

        m_WoodenBowView = (WoodenBowView)M_GunViewBase;
        m_WoodenBowModel = gameObject.GetComponent<WoodenBowModel>();
    }

    protected override void LoadAudioAsset()
    {
        Audio = Resources.Load<AudioClip>("Audio/Arrow Release");
    }

    protected override void Shoot()
    {
        GameObject temp = Instantiate(m_WoodenBowView.Prefab_Arrow, m_WoodenBowView.Muzzle_Transform.position, m_WoodenBowView.Muzzle_Transform.rotation);
        temp.GetComponent<ArrowController>().Shoot(m_WoodenBowView.Muzzle_Transform.forward, 1000, Demage, Hit);

        Durable -= 1;
        m_WoodenBowView.M_Animator.SetTrigger("Fire");
    }
}
