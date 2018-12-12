using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARArrDL : MonoBehaviour {

    
    float speed = 0.1f; // multipliers of the speed of the shot
    public Transform target; // the destination to which it moves
    bool setSpeed = false; // flag indicating the speed draw

    // Update is called once per frame
    void Update () {

        if (ARControl.ARTargetFind == true)
        {

            //drawing the speed
            if (setSpeed == false)
            {
                int game = (int)Random.Range(10f, 18f);
                speed = speed * game;
                setSpeed = true;
            }

            //Moving arrow
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            transform.Rotate(new Vector3(45, 0, 0) * Time.deltaTime);
        }
    }

    //Collision function detect
    private void OnTriggerEnter(Collider other)
    {
        //Trigger hitbox for shield
        if (other.tag == "Shield")
        {
            print("HIT SHIELD");
            //Play sound after hit
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.ARArrowHitAudio);
            //Play animation after hit
            Vector3 pos = this.transform.position;
            ARControl.Instance.PlayAnimation(pos, "hitshield");
            //Destroy yourself
            Destroy(gameObject);
        }

        //Trigger hitbox for heart
        if (other.tag == "Heart")
        {
            print("HIT HEART");
            //play sound after hit
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.ARHeartHitAudio);
            //play animation after hit
            Vector3 pos = this.transform.position;
            ARControl.Instance.PlayAnimation(pos, "hitheart");
            //Add increase dmg take
            ARControl.arHits += 1;
            //Destroy yourself
            Destroy(gameObject);
        }
    }

}
