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
        //Rotate object
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Dagger")
        {
            print("HIT BOMB !");
            ARControl.arPoints = 0;
            Vector3 pos = this.transform.position;
            ARControl.Instance.PlayAnimation(pos, "explosion");



           GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Coin");

            foreach (GameObject enemy in gameObjects)
                GameObject.Destroy(enemy);

            gameObjects = GameObject.FindGameObjectsWithTag("Bomb");

            foreach (GameObject enemy in gameObjects)
                GameObject.Destroy(enemy);

            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.ARBombHitAudio);
            Destroy(gameObject);
            Destroy(transform.root.gameObject);
            
            
        }
    }
}   
