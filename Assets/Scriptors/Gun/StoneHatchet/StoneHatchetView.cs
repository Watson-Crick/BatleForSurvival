using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHatchetView : MonoBehaviour {

    private Transform m_Transform;
    private Animator m_Animator;
    private Camera m_EnvCamera;

    private RectTransform gunStar_Transform;

    public Transform M_Transform { get { return m_Transform; } }
    public Animator M_Animator { get { return m_Animator; } }
    public Camera M_EnvCamera { get { return m_EnvCamera; } }

    public RectTransform GunStar_Transform { get { return gunStar_Transform; } }

    void Awake () {
        FindAndInit();
	}

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;
        m_Animator = gameObject.GetComponent<Animator>();
        m_EnvCamera = GameObject.Find("FPSController/PersonCamera/EnvCamera").GetComponent<Camera>();

        gunStar_Transform = GameObject.Find("Canvas/MainPanel/GunStar").GetComponent<RectTransform>();
    }
}
