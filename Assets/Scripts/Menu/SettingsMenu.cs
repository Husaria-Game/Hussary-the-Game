using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public Toggle arAvailable;
    public Toggle asrAvailable;
    public Toggle aIPlayerCardsSeen;
    public Toggle musicinGamePlaying;

    void Start()
    {
        if (SettsHolder.instance.isARAvailable == false) arAvailable.isOn = false;
        if (SettsHolder.instance.isASRAvailable == false) asrAvailable.isOn = false;
        if (SettsHolder.instance.aIPlayerCardsSeen == false) aIPlayerCardsSeen.isOn = false;
        if (SettsHolder.instance.isMusicInGamePlaying == false) musicinGamePlaying.isOn = false;

        arAvailable.onValueChanged.AddListener(delegate { SettsHolder.instance.changeARAvailable(); });
        asrAvailable.onValueChanged.AddListener(delegate { SettsHolder.instance.changeASRAvailable(); });
        aIPlayerCardsSeen.onValueChanged.AddListener(delegate { SettsHolder.instance.changeAIPlayerCardsSeen(); });
        musicinGamePlaying.onValueChanged.AddListener(delegate { SettsHolder.instance.changeMusicInGamePlaying(); });
    }

}
