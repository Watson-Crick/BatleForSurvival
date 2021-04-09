using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    private Transform m_Transform;
    private Transform door_Transform;

	void Start () {
        FindAndInit();
	}

    private void Update()
    {
        if (m_Transform.parent.Find("Door") != null)
        {
            door_Transform = m_Transform.parent.Find("Door");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (door_Transform != null)
        {
            if (other.gameObject.name == "FPSController")
            {
                OpenDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (door_Transform != null)
        {
            if (other.gameObject.name == "FPSController")
            {
                CloseDoor();
            }
        }
    }

    private void FindAndInit()
    {
        m_Transform = gameObject.transform;

        door_Transform = null;
    }

    private void OpenDoor()
    {
        door_Transform.Rotate(Vector3.up, -90);
    }

    private void CloseDoor()
    {
        door_Transform.Rotate(Vector3.up, 90);
    }

}
