using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseFactionScriptWithComputer : MonoBehaviour
{

    public bool isClicked;
    public Button startGame;

    private Faction secondFaction;

    void Start()
    {
        startGame.enabled = false;
        isClicked = false;
    }

    void Update()
    {
        switch (isClicked)
        {
            case false:
                startGame.GetComponentInChildren<Text>().text = "Wybierz frakcję";
                startGame.GetComponentInChildren<Text>().color = Color.red;
                startGame.enabled = false;
                break;
            case true:
                startGame.GetComponentInChildren<Text>().text = "Dalej!";
                startGame.GetComponentInChildren<Text>().color = Color.black;
                startGame.enabled = true;
                break;
        }
    }

    public void setSecondFaction(string name)
    {
        switch (name)
        {
            case "Rzeczpospolita Obojga Narodów":
                secondFaction = Faction.Poland;
                break;
            case "Imperium Osmańskie":
                secondFaction = Faction.Ottoman;
                break;
        }
    }

    public Faction getSecondFaction()
    {
        return secondFaction;
    }

    public void returnToMainMenu()
    {
        isClicked = false;
    }
}

