using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBowView : GunViewBase {

    private GameObject prefab_Arrow;

    public GameObject Prefab_Arrow { get { return prefab_Arrow; } }

    void Awake () {
        FindAndInit();
	}

    protected override void FindAndInit()
    {
        base.FindAndInit();

        prefab_Arrow = Resources.Load<GameObject>("Gun/Arrow");
    }

    protected override void InitHoldPose()
    {
        StartPos = M_Transform.localPosition;
        StartRot = M_Transform.localRotation.eulerAngles;
        EndPos = new Vector3(0.75f, -1.2f, 0.22f);
        EndRot = new Vector3(2.5f, -8, 35);
    }

    protected override void InitMuzzleTransform()
    {
        Muzzle_Transform = M_Transform.Find("Armature/Arm_L/Forearm_L/Wrist_L/Weapon/Muzzle");
    }
}
