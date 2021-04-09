using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour {

	void Update () {
        Vector3 shellVec = new Vector3(0, gameObject.transform.rotation.eulerAngles.y, 0);
        gameObject.transform.Rotate(shellVec * 5);
    }
}
