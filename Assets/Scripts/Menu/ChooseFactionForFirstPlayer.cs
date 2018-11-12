using UnityEngine;
using UnityEngine.UI;

public class ChooseFactionForFirstPlayer : MonoBehaviour {

    public InputField inputName;
    public Button startGame;
    public bool isClicked;

    private string firstPlayersName;
    private Faction firstFaction;

    void Start()
    {       
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

    public void setFirstFaction(string name)
    {
        switch(name)
        {
            case "Rzeczpospolita Obojga Narodów":
                firstFaction = Faction.Poland;
                break;
            case "Imperium Osmańskie":
                firstFaction = Faction.Ottoman;
                break;
        }
    }

    public Faction getFirstFaction()
    {
        return firstFaction;
    }


    public void setFirstPlayersName()
    {
        firstPlayersName = inputName.text;
    }

    public string getFirstPlayersName()
    {
        return firstPlayersName;
    }

    public void returnToMainMenu()
    {
        isClicked = false;
        inputName.text = "";
    }

}
