using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARBomb : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    	
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Dagger")
        {
            print("HIT BOMB !");
            ARControl.arPoints = 0;
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Coin");

            foreach (GameObject enemy in gameObjects)
                GameObject.Destroy(enemy);

            gameObjects = GameObject.FindGameObjectsWithTag("Bomb");

            foreach (GameObject enemy in gameObjects)
                GameObject.Destroy(enemy);

            Destroy(gameObject);
            Destroy(transform.root.gameObject);
            
            
        }
    }
}   
