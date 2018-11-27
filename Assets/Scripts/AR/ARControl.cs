using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARControl : MonoBehaviour {

    private float timer = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //After press 9 -Back and add money
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ARManager.Instance.ARSceneResult(true);
            SceneManager.UnloadScene("ARScene");
        }

        //After press 8 -Back and no bonus money
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ARManager.Instance.ARSceneResult(true);
            SceneManager.UnloadScene("ARScene");
        }

        //After time go back
        timer += Time.deltaTime;
        if (timer > 30)
        {
            ARManager.Instance.ARSceneResult(false);
            SceneManager.UnloadScene("ARScene");
        }

    }
}
