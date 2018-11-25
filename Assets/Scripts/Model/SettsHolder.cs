﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettsHolder : MonoBehaviour {

    public static SettsHolder instance;

    private bool isPlayedAgain = false;

    private GameMode typeOfEnemy;

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
        instance.typeOfEnemy = GameMode.Human;
    }

    public void PlayAgainstAI()
    {
        instance.typeOfEnemy = GameMode.Computer;
    }
}
