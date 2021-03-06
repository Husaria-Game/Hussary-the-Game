﻿using System.Collections;
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

    //Colision function
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Dagger")
        {
            print("HIT BOMB !");

            //set bonus money to 0
            ARControl.arPoints = 0;

            //play animation after hit
            Vector3 pos = this.transform.position;
            ARControl.Instance.PlayAnimation(pos, "explosion");


           //destroy other gameobjects
           GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Coin");

            foreach (GameObject enemy in gameObjects)
                GameObject.Destroy(enemy);

            gameObjects = GameObject.FindGameObjectsWithTag("Bomb");

            foreach (GameObject enemy in gameObjects)
                GameObject.Destroy(enemy);

            //play sound after hit
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.ARBombHitAudio);

            //Destory hitbox and root object
            Destroy(gameObject);
            Destroy(transform.root.gameObject);
            
            
        }
    }
}   
