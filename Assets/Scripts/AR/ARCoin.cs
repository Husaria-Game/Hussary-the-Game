using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCoin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (this.gameObject.tag == "Coin")
        {
            //Rotate object
            transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dagger")
        {
            print("HIT");
            //play sound after hit
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.ARCoinHitAudio);
            //play animation after hit
            Vector3 pos = this.transform.position;
            ARControl.Instance.PlayAnimation(pos, "hit");
            //increase money bonus
            ARControl.arPoints += 1;
            //Destoy hitbox and root object
            Destroy(gameObject);
            Destroy(transform.root.gameObject);
        }
    }
}
