using UnityEngine;
using UnityEngine.UI;

public class ChooseFactionScript1 : MonoBehaviour {

    private string firstPlayersName;
    public InputField inputName;

    public bool isClicked;
    public Button startGame;

    private Faction firstFaction;

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
