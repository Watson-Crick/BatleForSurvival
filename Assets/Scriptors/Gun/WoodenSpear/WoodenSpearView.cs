using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpearView : GunViewBase {

    private GameObject prefab_Spear;

    public GameObject Prefab_Spear { get { return prefab_Spear; } }

    void Awake()
    {
        FindAndInit();
    }

    protected override void FindAndInit()
    {
        base.FindAndInit();

        prefab_Spear = Resources.Load<GameObject>("Gun/Wooden_Spear");
    }

    protected override void InitHoldPose()
    {
        StartPos = M_Transform.localPosition;
        StartRot = M_Transform.localRotation.eulerAngles;
        EndPos = new Vector3(0, -1.58f, 0.32f);
        EndRot = new Vector3(0, 4, 0.3f);
    }

    protected override void InitMuzzleTransform()
    {
        Muzzle_Transform = M_Transform.Find("Armature/Arm_R/Forearm_R/Wrist_R/Weapon/Muzzle");
    }
}
