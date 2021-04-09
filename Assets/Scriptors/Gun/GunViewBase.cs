using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class GunViewBase : MonoBehaviour {

    #region 枪械都一致的属性

    private Transform m_Transform;
    private Animator m_Animator;
    private Camera m_EnvCamera;

    private RectTransform gunStar_Transform;

    public Transform M_Transform { get { return m_Transform; } }
    public Animator M_Animator { get { return m_Animator; } }
    public Camera M_EnvCamera { get { return m_EnvCamera; } }

    public RectTransform GunStar_Transform { get { return gunStar_Transform; } }

    #endregion

    #region 枪械中可能不一致的属性

    //用于优化开镜动画
    private Vector3 startPos;
    private Vector3 startRot;
    private Vector3 endPos;
    private Vector3 endRot;

    private Transform muzzle_Transform;                     //枪口

    public Vector3 StartPos { get { return startPos; } set { startPos = value; } }
    public Vector3 StartRot { get { return startRot; } set { startRot = value; } }
    public Vector3 EndPos { get { return endPos; } set { endPos = value; } }
    public Vector3 EndRot { get { return endRot; } set { endRot = value; } }

    //部分相关Transform组件
    public Transform Muzzle_Transform { get { return muzzle_Transform; } set { muzzle_Transform = value; } }


    #endregion

    protected virtual void FindAndInit()
    {
        m_Transform = gameObject.transform;
        m_Animator = gameObject.GetComponent<Animator>();
        m_EnvCamera = GameObject.Find("FPSController/PersonCamera/EnvCamera").GetComponent<Camera>();

        gunStar_Transform = GameObject.Find("Canvas/MainPanel/GunStar").GetComponent<RectTransform>();

        InitHoldPose();
        InitMuzzleTransform();
    }

    /// <summary>
    /// 关镜
    /// </summary>
    public void EndHoldPose(float time = 0.2f, int fov = 60)
    {

        M_Transform.DOLocalMove(StartPos, time);
        M_Transform.DOLocalRotate(StartRot, time);
        M_EnvCamera.DOFieldOfView(fov, time);
    }

    /// <summary>
    /// 开镜
    /// </summary>
    public void EnterHoldPose(float time = 0.2f, int fov = 40)
    {
        M_Transform.DOLocalMove(EndPos, time);
        M_Transform.DOLocalRotate(EndRot, time);
        M_EnvCamera.DOFieldOfView(fov, time);
    }

    /// <summary>
    /// 初始化开/关镜相关数据
    /// </summary>
    protected abstract void InitHoldPose();

    /// <summary>
    /// 初始化射击相关Transform组件
    /// </summary>
    protected abstract void InitMuzzleTransform();
}
