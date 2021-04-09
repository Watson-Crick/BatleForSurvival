using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : GunWeaponBase {

    private ShotgunView m_ShotgunView;
    private ShotgunModel m_ShotgunModel;

    void Start()
    {
        FindAndInit();
    }

    protected override void FindAndInit()
    {
        base.FindAndInit();

        m_ShotgunView = (ShotgunView)M_GunViewBase;
        m_ShotgunModel = gameObject.GetComponent<ShotgunModel>();
    }

    protected override void LoadAudioAsset()
    {
        Audio = Resources.Load<AudioClip>("Audio/Shotgun_Fire");
    }

    protected override void LoadEffectAsset()
    {
        Effect = Resources.Load<GameObject>("Effects/Gun/Shotgun_GunPoint_Effect");
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    protected override void PlayEffect()
    {
        PlayGunEffect();
        PlayShellEffect();
    }

    /// <summary>
    /// 播放射击特效
    /// </summary>
    private void PlayGunEffect()
    {
        GameObject gunEffect = null;
        if (EffectPool.Data())
        {
            //从对象池中提取对象
            gunEffect = EffectPool.GetObject();
            gunEffect.transform.position = m_ShotgunView.Muzzle_Transform.position;
            gunEffect.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            //实例化射击特效
            gunEffect = Instantiate(Effect, m_ShotgunView.Muzzle_Transform.position, Quaternion.identity, m_ShotgunView.GunEffectSet_Transform);
            gunEffect.GetComponent<ParticleSystem>().Play();
            gunEffect.name = "Gun_Effect";
        }

        StartCoroutine(DelayAddObjectPool(EffectPool, gunEffect, 1.0f));
    }

    /// <summary>
    /// 播放弹壳弹出特效
    /// </summary>
    private void PlayShellEffect()
    {
        GameObject shell = null;
        if (ShellPool.Data())
        {
            //从对象池中提取对象
            shell = ShellPool.GetObject();
            shell.GetComponent<Rigidbody>().isKinematic = true;
            shell.transform.position = m_ShotgunView.Magazine_Transform.position;
            shell.transform.rotation = Quaternion.Euler(m_ShotgunView.Magazine_Transform.rotation.eulerAngles);
            shell.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            //实例化弹壳弹出效果
            shell = Instantiate(m_ShotgunView.Prefab_Shell, m_ShotgunView.Magazine_Transform.position, Quaternion.Euler(m_ShotgunView.Magazine_Transform.rotation.eulerAngles), m_ShotgunView.GunShellSet_Transform);
            shell.name = "Shell";
        }
        shell.GetComponent<Rigidbody>().AddForce(m_ShotgunView.Magazine_Transform.up * 50);

        StartCoroutine(DelayAddObjectPool(ShellPool, shell, 3.0f));
    }

    /// <summary>
    /// 射击
    /// </summary>
    protected override void Shoot()
    {
        StartCoroutine(CreateBullet());

        Durable -= 1;
        m_ShotgunView.M_Animator.SetTrigger("Fire");
    }

    IEnumerator CreateBullet()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
            GameObject bullet = Instantiate<GameObject>(m_ShotgunView.Prefab_Bullet, m_ShotgunView.Muzzle_Transform.position, Quaternion.identity);
            bullet.GetComponent<ShotgunBulletController>().Shoot(m_ShotgunView.Muzzle_Transform.forward + offset, 3000, Demage, Hit);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void PlayPumpAudio()
    {
        AudioSource.PlayClipAtPoint(m_ShotgunView.Pump_Audio, m_ShotgunView.Muzzle_Transform.position);
    }
}
