using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARArrDL : MonoBehaviour {

    float speed = 0.1f;
    public Transform target;
    bool setSpeed = false;

	// Update is called once per frame
	void Update () {

        if (ARControl.ARTargetFind == true)
        {
            if (setSpeed == false)
            {
                int game = (int)Random.Range(8f, 12f);
                speed = speed * game;
                setSpeed = true;
            }

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            transform.Rotate(new Vector3(45, 0, 0) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield")
        {
            print("HIT SHIELD");
            ARControl.Instance.PlayAudioAR(ARControl.Instance.ARArrowHitAudio);
            Vector3 pos = this.transform.position;
            ARControl.Instance.PlayAnimation(pos, "hitshield");
            Destroy(gameObject);
        }

        if (other.tag == "Heart")
        {
            print("HIT HEART");
            ARControl.Instance.PlayAudioAR(ARControl.Instance.ARHeartHitAudio);
            Vector3 pos = this.transform.position;
            ARControl.Instance.PlayAnimation(pos, "hitheart");
            ARControl.arHits += 1;
            Destroy(gameObject);
        }
    }

}
