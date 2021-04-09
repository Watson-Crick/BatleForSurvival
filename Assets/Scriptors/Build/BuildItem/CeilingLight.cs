using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLight : MaterialModelBase {

    private Transform roof_Transform;

    public Transform Roof_Transform { get { return roof_Transform; } }

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
            if (other.gameObject.tag == "RoofToLight")
            {
                IsAttach = true;
                State = BuildItemState.CANPUT;
                NewMaterial.color = new Color32(0, 255, 0, 100);
                M_MeshRenderer.material = NewMaterial;
                M_Transform.transform.position = other.transform.position;
                roof_Transform = other.transform.parent;
                AttachObject = other.gameObject;
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (other.gameObject.tag == "RoofToLight")
            {
                IsAttach = false;
                State = BuildItemState.CANNOTPUT;
                NewMaterial.color = new Color32(255, 0, 0, 100);
                M_MeshRenderer.material = NewMaterial;
                AttachObject = null;
            }
        }
    }
}
