using UnityEngine;
using UnityEngine.UI;

public class ChooseFactionForFirstPlayer : MonoBehaviour {

    public InputField inputName;
    public Button startGame;
    public bool isClicked;

    private void OnEnable()
    {
        gameObject.transform.SetParent(SettsHolder.instance.transform);
    }
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
                SettsHolder.instance.southFaction = Faction.Poland;
                break;
            case "Imperium Osmańskie":
                SettsHolder.instance.southFaction = Faction.Ottoman;
                break;
        }
    }

    public void setFirstPlayersName()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            SettsHolder.instance.southName = "Gracz 1";
        }
        else
        {
            SettsHolder.instance.southName = inputName.text;
        }

    }

    public void returnToMainMenu()
    {
        isClicked = false;
        inputName.text = "";
    }

}
