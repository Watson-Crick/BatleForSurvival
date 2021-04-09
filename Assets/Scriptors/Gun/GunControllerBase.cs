using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunControllerBase : MonoBehaviour {

    //数值字段
    [SerializeField] private int id;
    [SerializeField] private int demage;
    [SerializeField] private int durable;
    [SerializeField] private GunType gunWeaponType;

    private bool isHoldPose;                    //开/关镜状态判断
    private bool canShoot;
    private float durableLimit;                   //耐久上限

    //数值属性
    public int Id { set { id = value; } get { return id; } }
    public int Demage { set { demage = value; } get { return demage; } }
    public int Durable
    {
        set
        {
            durable = value;
            if (durable <= 0)
            {
                Destroy(gameObject);
                m_GunViewBase.GunStar_Transform.gameObject.SetActive(false);
            }
        }
        get { return durable; }
    }
    public GunType GunWeaponType { set { gunWeaponType = value; } get { return gunWeaponType; } }
    public bool CanShoot { set { canShoot = value; } get { return CanShoot; } }

    //组件字段
    private GunViewBase m_GunViewBase;

    private AudioClip audio;
    private GameObject effect;
    private RaycastHit hit;                   //射击后命中的碰撞判定数据

    private ObjectPool shellPool;
    private ObjectPool effectPool;

    private GameObject toolBarUI;           //工具栏物品UI

    //组件属性
    public GunViewBase M_GunViewBase { set { m_GunViewBase = value; } get { return m_GunViewBase; } }

    public AudioClip Audio { set { audio = value; } get { return audio; } }
    public GameObject Effect { set { effect = value; } get { return effect; } }
    public RaycastHit Hit { set { hit = value; } get { return hit; } }

    public ObjectPool ShellPool { set { shellPool = value; } get { return shellPool; } }
    public ObjectPool EffectPool { set { effectPool = value; } get { return effectPool; } }

    public GameObject ToolBarUI { set { toolBarUI = value; } get { return toolBarUI; } }

    void Update()
    {
        ShootReady();
        MouseControl();
    }

    protected virtual void FindAndInit()
    {
        m_GunViewBase = gameObject.GetComponent<GunViewBase>();
        m_GunViewBase.GunStar_Transform.gameObject.SetActive(true);

        isHoldPose = false;
        durableLimit = durable;

        shellPool = new ObjectPool();
        effectPool = new ObjectPool();

        LoadAudioAsset();
    }

    protected abstract void LoadAudioAsset();

    /// <summary>
    /// 播放音效
    /// </summary>
    private void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(audio, m_GunViewBase.Muzzle_Transform.position);
    }

    /// <summary>
    /// 延迟后向对象池添加对象
    /// </summary>
    protected IEnumerator DelayAddObjectPool(ObjectPool pool, GameObject temp, float time)
    {
        yield return new WaitForSeconds(time);
        pool.AddObject(temp);
    }

    /// <summary>
    /// 射击准备
    /// </summary>
    private void ShootReady()
    {
        Ray ray = new Ray(M_GunViewBase.Muzzle_Transform.position, M_GunViewBase.Muzzle_Transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(m_GunViewBase.Muzzle_Transform.position, hit.point);
            Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(M_GunViewBase.M_EnvCamera, hit.point);
            M_GunViewBase.GunStar_Transform.position = uiPos;
        }
    }

    protected abstract void Shoot();

    /// <summary>
    /// 同步工具栏物品UI
    /// </summary>
    private void UpdateUI()
    {
        toolBarUI.GetComponent<InventoryItemController>().UpdateUI(Durable / durableLimit);
    }

    /// <summary>
    /// 鼠标控制
    /// </summary>
    protected virtual void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            ShootControl();
            UpdateUI();
        }

        if (Input.GetMouseButtonDown(1))
        {
            HoldPoseControl();
        }
    }

    protected virtual void ShootControl()
    {
        Shoot();
        PlayAudio();
    }

    private void HoldPoseControl()
    {
        if (isHoldPose)
        {
            //关镜
            isHoldPose = false;
            HoldPoseTransformChange(isHoldPose);
        }
        else
        {
            //开镜
            isHoldPose = true;
            HoldPoseTransformChange(isHoldPose);
        }
    }

    /// <summary>
    /// 开/关镜
    /// </summary>
    private void HoldPoseTransformChange(bool isEnter)
    {
        m_GunViewBase.M_Animator.SetBool("HoldPose", isEnter);
        if (!isEnter)
        {
            //关镜
            m_GunViewBase.GunStar_Transform.gameObject.SetActive(true);
            m_GunViewBase.EndHoldPose();
        }
        else
        {
            //开镜
            m_GunViewBase.GunStar_Transform.gameObject.SetActive(false);
            m_GunViewBase.EnterHoldPose();
        }
    }

    /// <summary>
    /// 判断是否可以射击
    /// </summary>
    private void IfCanShoot(int state)
    {
        if (state == 0)
        {
            CanShoot = false;
        }
        else if (state == 1)
        {
            CanShoot = true;
        }
    }

    /// <summary>
    /// 动画事件绑定隐藏自身方法
    /// </summary>
    private void HideSelf()
    {
        gameObject.SetActive(false);
    }
}
