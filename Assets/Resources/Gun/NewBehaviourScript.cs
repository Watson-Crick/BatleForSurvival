using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
