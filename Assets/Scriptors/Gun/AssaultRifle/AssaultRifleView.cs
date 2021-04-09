using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleView : GunViewBase {

    #region 枪械相关字段和属性

    private GameObject prefab_Bullet;
    private GameObject prefab_Shell;

    private Transform magazine_Transform;                 //弹夹  
    private Transform gunEffectSet_Transform;            //枪械开火特效集合
    private Transform gunShellSet_Transform;             //弹壳弹出特效集合

    public GameObject Prefab_Bullet { get { return prefab_Bullet; } }
    public GameObject Prefab_Shell { get { return prefab_Shell; } }

    public Transform Magazine_Transform { get { return magazine_Transform; } }
    public Transform GunEffectSet_Transform { get { return gunEffectSet_Transform; } }
    public Transform GunShellSet_Transform { get { return gunShellSet_Transform; } }

    #endregion

    void Awake () {
        FindAndInit();
	}

    protected override void FindAndInit()
    {
        base.FindAndInit();

        magazine_Transform = M_Transform.Find("Assault_Rifle/Magazine");
        gunEffectSet_Transform = GameObject.Find("TempObject/AssaultRifle_Effect_Set").transform;
        gunShellSet_Transform = GameObject.Find("TempObject/AssaultRifle_Shell_Set").transform;

        prefab_Bullet = Resources.Load<GameObject>("Gun/Bullet");
        prefab_Shell = Resources.Load<GameObject>("Gun/Shell");
    }

    protected override void InitHoldPose()
    {
        StartPos = M_Transform.localPosition;
        StartRot = M_Transform.localRotation.eulerAngles;
        EndPos = new Vector3(-0.065f, -1.85f, 0.25f);
        EndRot = new Vector3(2.8f, 1.3f, 0.08f);
    }

    protected override void InitMuzzleTransform()
    {
        Muzzle_Transform = M_Transform.Find("Assault_Rifle/Muzzle");
    }
}
