using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class MaterialModelBase : MonoBehaviour {

    private Transform m_Transform;
    private MeshRenderer m_MeshRenderer;
    private GameObject attachObject;

    private Material newMaterial;
    private Material oldMaterial;
    private BuildItemState state;
    private bool isAttach;

    public Transform M_Transform { set { m_Transform = value; } get { return m_Transform; } }
    public MeshRenderer M_MeshRenderer { set { m_MeshRenderer = value; } get { return m_MeshRenderer; } }
    public GameObject AttachObject { set { attachObject = value; } get { return attachObject; } }

    public Material NewMaterial { set { newMaterial = value; } get { return newMaterial; } }
    public Material OldMaterial { set { oldMaterial = value; } get { return oldMaterial; } }
    public BuildItemState State { set { state = value; }  get { return state; } }
    public bool IsAttach { set { isAttach = value; } get { return isAttach; } }

    protected virtual void Awake()
    {
        FindAndInit();
    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();
        attachObject = null;

        oldMaterial = Instantiate<Material>(m_MeshRenderer.material);
        newMaterial = Resources.Load<Material>("Build/Building Preview");
        state = BuildItemState.CANNOTPUT;
        isAttach = false;
    }

    /// <summary>
    /// 完成模型创建
    /// </summary>
    public void Normal()
    {
        state = BuildItemState.HAVEPUT;
        m_MeshRenderer.material = oldMaterial;
        gameObject.layer = 14;
        if (attachObject != null)
            attachObject.SetActive(false);
    }

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void OnTriggerExit(Collider other);
}
