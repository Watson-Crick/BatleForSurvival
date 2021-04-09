using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MaterialModelBase {

    protected override void Awake()
    {
        base.Awake();
        NewMaterial.color = new Color32(255, 0, 0, 100);
        M_MeshRenderer.material = NewMaterial;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (other.gameObject.tag == "PlatformToGround")
            {
                IsAttach = true;
                State = BuildItemState.CANPUT;
                NewMaterial.color = new Color32(0, 255, 0, 100);
                M_MeshRenderer.material = NewMaterial;
                M_Transform.position = other.transform.position;
                AttachObject = other.gameObject;
                JudgeRotAndPos(other);
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (other.gameObject.tag == "PlatformToGround")
            {
                IsAttach = false;
                State = BuildItemState.CANNOTPUT;
                NewMaterial.color = new Color32(255, 0, 0, 100);
                M_MeshRenderer.material = NewMaterial;
                AttachObject = null;
            }
        }
    }

    private void JudgeRotAndPos(Collider coll)
    {
        Vector3 modelOffset = Vector3.zero;
        Vector3 modelRot = Vector3.zero;
        switch(coll.gameObject.name)
        {
            case "GroundA":
                modelOffset = new Vector3(0, 0, -2.5f);
                modelRot = new Vector3(0, -90, 0);
                break;
            case "GroundB":
                modelOffset = new Vector3(0, 0, 2.5f);
                modelRot = new Vector3(0, 90, 0);
                break;
            case "GroundC":
                modelOffset = new Vector3(2.5f, 0, 0);
                modelRot = new Vector3(0, 180, 0);
                break;
            case "GroundD":
                modelOffset = new Vector3(-2.5f, 0, 0);
                break;
        }
        M_Transform.position = coll.gameObject.transform.parent.position + modelOffset;
        M_Transform.rotation = Quaternion.Euler(modelRot);
    }
}
