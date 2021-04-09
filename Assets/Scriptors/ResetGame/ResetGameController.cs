using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetGameController : MonoBehaviour {

    private bool isChanging = false;

	void Start () {

	}
	
	void Update()
    {
        ChangeScene();
    }

    private void ChangeScene()
    {
        if (!isChanging)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                isChanging = true;
                SceneManager.LoadScene("Game");
            }
        }
    }
}
