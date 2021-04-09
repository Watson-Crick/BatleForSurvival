using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleController : GunWeaponBase {

    private AssaultRifleView m_AssaultRifleView;
    private AssaultRifleModel m_AssaultRifleModel;

    void Start () {
        FindAndInit();
	}

    protected override void FindAndInit()
    {
        base.FindAndInit();

        CanShoot = true;

        m_AssaultRifleView = (AssaultRifleView)M_GunViewBase;
        m_AssaultRifleModel = gameObject.GetComponent<AssaultRifleModel>();
    }

    protected override void LoadAudioAsset()
    {
        Audio = Resources.Load<AudioClip>("Audio/AssaultRifle_Fire");
    }

    protected override void LoadEffectAsset()
    {
        Effect = Resources.Load<GameObject>("Effects/Gun/AssaultRifle_GunPoint_Effect");
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    protected override void PlayEffect()
    {
        PlayGunEffect();
        PlayShellEffect();
    }

    private void PlayGunEffect()
    {
        GameObject gunEffect = null;
        if (EffectPool.Data())
        {
            //从对象池中提取对象
            gunEffect = EffectPool.GetObject();
            gunEffect.transform.position = m_AssaultRifleView.Muzzle_Transform.position;
            gunEffect.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            //实例化射击特效
            gunEffect = Instantiate(Effect, m_AssaultRifleView.Muzzle_Transform.position, Quaternion.identity, m_AssaultRifleView.GunEffectSet_Transform);
            gunEffect.GetComponent<ParticleSystem>().Play();
            gunEffect.name = "Gun_Effect";
        }

        StartCoroutine(DelayAddObjectPool(EffectPool, gunEffect, 1.0f));
    }

    private void PlayShellEffect()
    {
        GameObject shell = null;
        if (ShellPool.Data())
        {
            //从对象池中提取对象
            shell = ShellPool.GetObject();
            shell.GetComponent<Rigidbody>().isKinematic = true;
            shell.transform.position = m_AssaultRifleView.Magazine_Transform.position;
            shell.transform.rotation = Quaternion.Euler(0, 0, -30);
            shell.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            //实例化弹壳弹出效果
            shell = Instantiate(m_AssaultRifleView.Prefab_Shell, m_AssaultRifleView.Magazine_Transform.position, Quaternion.Euler(0, 0, -30), m_AssaultRifleView.GunShellSet_Transform);
            shell.name = "Shell";
        }
        shell.GetComponent<Rigidbody>().AddForce(shell.transform.up * 50);

        StartCoroutine(DelayAddObjectPool(ShellPool, shell, 3.0f));
    }

    /// <summary>
    /// 射击
    /// </summary>
    protected override void Shoot()
    {
        if (Hit.collider != null)
        {
            if (Hit.collider.GetComponent<BulletMark>() != null)
            {
                Hit.collider.GetComponent<BulletMark>().CreateBulletMark(Hit);
            }
            if (Hit.collider.gameObject.layer == LayerMask.NameToLayer("AI"))
            {
                if (Hit.collider.transform.parent.name == "Head")
                {
                    Hit.collider.GetComponentInParent<AIController>().HitHard(2 * Demage);
                }
                else
                {
                    Hit.collider.GetComponentInParent<AIController>().HitNormal(Demage);
                }
                Hit.collider.GetComponentInParent<AIController>().PlayEffect(Hit);
            }
        }

        Durable -= 1;
        m_AssaultRifleView.M_Animator.SetTrigger("Fire");
    }

}
