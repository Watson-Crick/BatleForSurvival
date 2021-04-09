using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunWeaponBase : GunControllerBase {

    protected override void FindAndInit()
    {
        base.FindAndInit();
        LoadEffectAsset();
    }

    protected override void ShootControl()
    {
        base.ShootControl();
        PlayEffect();
    }

    protected abstract void LoadEffectAsset();

    protected abstract void PlayEffect();
}
