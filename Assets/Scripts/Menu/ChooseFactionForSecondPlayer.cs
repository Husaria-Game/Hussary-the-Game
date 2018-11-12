using UnityEngine;
using UnityEngine.UI;

public class ChooseFactionForSecondPlayer : MonoBehaviour {

    public InputField inputName;
    public Button startGame;
    public bool isClicked;

    private string secondPlayersName;
    private Faction secondFaction;

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

    public void setSecondPlayersName()
    {
        secondPlayersName = inputName.text;
    }

    public string getSecondPlayersName()
    {
        return secondPlayersName;
    }

    public void returnToMainMenu()
    {
        isClicked = false;
        inputName.text = "";
    }

}
