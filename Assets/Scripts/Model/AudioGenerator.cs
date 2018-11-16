using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGenerator : MonoBehaviour {

    public AudioClip magicSound;
    public AudioClip explosion;

    private AudioSource source;

    void Start ()
    {
        source = gameObject.AddComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            source.PlayOneShot(explosion);
        }
	}

    public void playClip()
    {
        source.PlayOneShot(magicSound);
    }
}
