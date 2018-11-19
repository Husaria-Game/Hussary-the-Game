using UnityEngine;
using UnityEngine.UI;

public class ChooseFactionForSecondPlayer : MonoBehaviour {

    public InputField inputName = null;
    public Button startGame;
    public bool isClicked;

    void Start()
    {
        SettsHolder.instance.northName = "Gracz 2";
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
        if (string.IsNullOrEmpty(inputName.text))
        {
            SettsHolder.instance.northName = "Gracz 2";
        }
        else
        {
            SettsHolder.instance.northName = inputName.text;
        }
    }

    public void returnToMainMenu()
    {
        isClicked = false;
        inputName.text = "";
    }

}
