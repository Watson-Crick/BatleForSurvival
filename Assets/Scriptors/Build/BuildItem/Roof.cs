using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roof : MaterialModelBase {

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
            if (other.gameObject.tag == "WallToRoof")
            {
                IsAttach = true;
                State = BuildItemState.CANPUT;
                NewMaterial.color = new Color32(0, 255, 0, 100);
                M_MeshRenderer.material = NewMaterial;
                M_Transform.position = other.transform.position;
                AttachObject = other.gameObject;
            }

            if (other.gameObject.tag == "RoofToRoof")
            {
                IsAttach = true;
                State = BuildItemState.CANPUT;
                NewMaterial.color = new Color32(0, 255, 0, 100);
                M_MeshRenderer.material = NewMaterial;
                JudgePos(other);
                AttachObject = other.gameObject;
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (other.gameObject.tag == "WallToRoof" || other.gameObject.tag == "RoofToRoof")
            {
                IsAttach = false;
                State = BuildItemState.CANNOTPUT;
                NewMaterial.color = new Color32(255, 0, 0, 100);
                M_MeshRenderer.material = NewMaterial;
                AttachObject = null;
            }
        }
    }

    private void JudgePos(Collider coll)
    {
        Vector3 modelOffset = Vector3.zero;
        switch (coll.gameObject.name)
        {
            case "EavesA":
                modelOffset = new Vector3(3.3f, 0, 0);
                break;
            case "EavesB":
                modelOffset = new Vector3(-3.3f, 0, 0);
                break;
            case "EavesC":
                modelOffset = new Vector3(0, 0, 3.3f);
                break;
            case "EavesD":
                modelOffset = new Vector3(0, 0, -3.3f);
                break;
        }
        M_Transform.position = coll.gameObject.transform.parent.position + modelOffset;
    }
}
