using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettsHolder : MonoBehaviour {

    public static SettsHolder instance;

    private bool isPlayedAgain = false;

    //General settings
    public bool isARAvailable = true;
    public bool isASRAvailable = true;
    public bool aIPlayerCardsSeen = true;
    public bool isMusicInGamePlaying = true;

    //Players settings
    public GameMode typeOfEnemy;
    public Faction southFaction;
    public Faction northFaction;
    public string northName;
    public string southName;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return;
        }
        Destroy(this.gameObject);
    }

    public void AttributeGameManagerNamesAndFactions()
    {
        //General Settings
        GameManager.Instance.isARAvailable = isARAvailable;
        GameManager.Instance.isASRAvailable = isASRAvailable;
        GameManager.Instance.aIPlayerCardsSeen = aIPlayerCardsSeen;
        GameManager.Instance.isMusicInGamePlaying = isMusicInGamePlaying;

        //Game Mode
        GameManager.Instance.typeOfEnemy = typeOfEnemy;

        //Factions
        GameManager.Instance.southFaction = southFaction;
        GameManager.Instance.northFaction = northFaction;

        //Names
        GameManager.Instance.southName = southName;
        GameManager.Instance.northName = northName;
    }

    public bool GetIsPlayedAgain()
    {
        return isPlayedAgain;
    }

    public void SetIsPlayedAgain()
    {
        isPlayedAgain = true;
    }

    public void UnsetIsPlayedAgain()
    {
        isPlayedAgain = false;
    }

    public void PlayAgainstHuman()
    {
        typeOfEnemy = GameMode.Human;
    }

    public void PlayAgainstAI()
    {
        typeOfEnemy = GameMode.Computer;
    }

    public void changeARAvailable()
    {
        isARAvailable = !isARAvailable;
    }

    public void changeASRAvailable()
    {
        isASRAvailable = !isASRAvailable;
    }

    public void changeAIPlayerCardsSeen()
    {
        aIPlayerCardsSeen = !aIPlayerCardsSeen;
    }

    public void changeMusicInGamePlaying()
    {
        isMusicInGamePlaying = !isMusicInGamePlaying;
    }
}
