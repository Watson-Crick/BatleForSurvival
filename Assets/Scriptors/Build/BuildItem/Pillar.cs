using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MaterialModelBase {

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
            if (other.gameObject.tag == "PlatformToPillar")
            {
                IsAttach = true;
                State = BuildItemState.CANPUT;
                NewMaterial.color = new Color32(0, 255, 0, 100);
                M_MeshRenderer.material = NewMaterial;
                M_Transform.transform.position = other.transform.position;
                AttachObject = other.gameObject;
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (State != BuildItemState.HAVEPUT)
        {
            if (other.gameObject.tag == "PlatformToPillar")
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
