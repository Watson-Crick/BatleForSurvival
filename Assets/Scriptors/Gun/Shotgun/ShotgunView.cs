using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunView : GunViewBase {

    #region 枪械相关字段和属性

    private GameObject prefab_Bullet;
    private GameObject prefab_Shell;

    private AudioClip pump_Audio;                      //弹壳弹出音效

    private Transform magazine_Transform;                 //弹夹位置

    private Transform gunEffectSet_Transform;            //枪械开火特效集合
    private Transform gunShellSet_Transform;             //弹壳弹出特效集合

    public GameObject Prefab_Bullet { get { return prefab_Bullet; } }
    public GameObject Prefab_Shell { get { return prefab_Shell; } }

    public AudioClip Pump_Audio { get { return pump_Audio; } }

    public Transform Magazine_Transform { get { return magazine_Transform; } }

    public Transform GunEffectSet_Transform { get { return gunEffectSet_Transform; } }
    public Transform GunShellSet_Transform { get { return gunShellSet_Transform; } }

    #endregion

    void Awake()
    {
        FindAndInit();
    }

    protected override void FindAndInit()
    {
        base.FindAndInit();

        pump_Audio = Resources.Load<AudioClip>("Audio/Shotgun_Pump");

        magazine_Transform = M_Transform.Find("Armature/Weapon/Magazine");

        gunEffectSet_Transform = GameObject.Find("TempObject/Shotgun_Effect_Set").transform;
        gunShellSet_Transform = GameObject.Find("TempObject/Shotgun_Shell_Set").transform;

        prefab_Bullet = Resources.Load<GameObject>("Gun/Bullet");
        prefab_Shell = Resources.Load<GameObject>("Gun/Shotgun_Shell");
    }

    protected override void InitHoldPose()
    {
        StartPos = M_Transform.localPosition;
        StartRot = M_Transform.localRotation.eulerAngles;
        EndPos = new Vector3(-0.14f, -1.78f, 0.03f);
        EndRot = new Vector3(0, 10, -0.6f);
    }

    protected override void InitMuzzleTransform()
    {
        Muzzle_Transform = M_Transform.Find("Armature/Weapon/Muzzle");
    }
}
