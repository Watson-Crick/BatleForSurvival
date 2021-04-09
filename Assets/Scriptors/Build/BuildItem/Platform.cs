using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MaterialModelBase{

    private void Update()
    {
        PressRKeyRotate();
    }

    private void OnCollisionStay(Collision collision)
    {
        JudgeCollisionWithEnterAndStay(collision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        JudgeCollisionWithEnterAndStay(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        JudgeCollisionWithExit(collision);
    }

    /// <summary>
    /// 进入或停留判断自身状态
    /// </summary>
    private void JudgeCollisionWithEnterAndStay(Collision collision)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (collision.gameObject.tag == "Terrain")
            {
                State = BuildItemState.CANPUT;
                NewMaterial.color = new Color32(0, 255, 0, 100);
                M_MeshRenderer.material = NewMaterial;
            }
            else
            {
                State = BuildItemState.CANNOTPUT;
                NewMaterial.color = new Color32(255, 0, 0, 100);
                M_MeshRenderer.material = NewMaterial;
            }
        }
    }

    /// <summary>
    /// 退出碰撞器判断自身状态
    /// </summary>
    private void JudgeCollisionWithExit(Collision collision)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (collision.gameObject.tag == "Terrain")
            {
                State = BuildItemState.CANNOTPUT;
                NewMaterial.color = new Color32(255, 0, 0, 100);
                M_MeshRenderer.material = NewMaterial;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (other.tag == "PlatformToGround")
            {
                IsAttach = true;
                JudgePos(other);
                AttachObject = other.gameObject;
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (other.tag == "PlatformToGround")
            {
                IsAttach = false;
            }
        }
    }

    private void JudgePos(Collider coll)
    {
        Vector3 modelOffset = Vector3.zero;
        switch (coll.gameObject.name)
        {
            case "GroundA":
                modelOffset = new Vector3(0, 0, -3.3f);
                break;
            case "GroundB":
                modelOffset = new Vector3(0, 0, 3.3f);
                break;
            case "GroundC":
                modelOffset = new Vector3(3.3f, 0, 0);
                break;
            case "GroundD":
                modelOffset = new Vector3(-3.3f, 0, 0);
                break;
        }
        M_Transform.position = coll.gameObject.transform.parent.position + modelOffset;
    }

    private void PressRKeyRotate()
    {
        if (State != BuildItemState.HAVEPUT && IsAttach != true)
        {
            if (Input.GetKey(KeyCode.R))
            {
                M_Transform.Rotate(Vector3.up);
            }
        }
    }
}
