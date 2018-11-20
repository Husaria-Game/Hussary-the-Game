using UnityEngine;
using UnityEngine.UI;

public class ChooseFactionForAIPlayer : MonoBehaviour
{
    public Button startGame;
    public bool isClicked;

    void Start()
    {
        SettsHolder.instance.northName = "Gracz AI";
        startGame.enabled = false;
        isClicked = false;
    }

    void Update()
    {
        if (!isClicked)
        {
            startGame.GetComponentInChildren<Text>().text = "Wybierz frakcję";
            startGame.GetComponentInChildren<Text>().color = Color.red;
            startGame.enabled = false;
        }
        else
        {
            startGame.GetComponentInChildren<Text>().text = "Dalej!";
            startGame.GetComponentInChildren<Text>().color = Color.black;
            startGame.enabled = true;
        }
    }

    public void setSecondFaction(string name)
    {
        switch (name)
        {
            case "Rzeczpospolita Obojga Narodów":
                SettsHolder.instance.northFaction = Faction.Poland;
                break;
            case "Imperium Osmańskie":
                SettsHolder.instance.northFaction = Faction.Ottoman;
                break;
        }
    }

    public void setSecondPlayersName()
    {
        SettsHolder.instance.northName = "Gracz AI";
    }

    public void returnToMainMenu()
    {
        isClicked = false;
    }
}

