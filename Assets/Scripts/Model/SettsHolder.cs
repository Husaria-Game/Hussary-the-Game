using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettsHolder : MonoBehaviour {

    public static SettsHolder instance;

    //Skrypty czytające dane z menu (imiona i frakcje) - MultiPlayer
    private ChooseFactionForFirstPlayer chooseFactionForFirstPlayer;
    private ChooseFactionForSecondPlayer chooseFactionForSecondPlayer;

    //Skrypty czytające dane z menu (imiona i frakcje) - SinglePlayer
    private ChooseFactionForFirstPlayer chooseFactionForFirstPlayerSingleMode;
    private ChooseFactionForAIPlayer chooseFactionForAIPlayer;

    public GameObject FirstMenuSingle;
    public GameObject SecondtMenuSingle;
    public GameObject FirstMenuMulti;
    public GameObject SecondtMenuMulti;

    private bool isPlayedAgain = false;
    private bool isEnemyHuman = true;

    public Faction southFaction;
    public Faction northFaction;

    public string northName;
    public string southName;

    // Use this for initialization
    void Awake()
    {
        if(instance == null)
        {
            instance = this;

            chooseFactionForFirstPlayer = FirstMenuMulti.GetComponent<ChooseFactionForFirstPlayer>();
            chooseFactionForSecondPlayer = SecondtMenuMulti.GetComponent<ChooseFactionForSecondPlayer>();

            chooseFactionForFirstPlayerSingleMode = FirstMenuSingle.GetComponent<ChooseFactionForFirstPlayer>();
            chooseFactionForAIPlayer = SecondtMenuSingle.GetComponent<ChooseFactionForAIPlayer>();

            DontDestroyOnLoad(this);
            return;


        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveNamesAndFactions()
    {
        if (isEnemyHuman)
        {
            //Factions
            southFaction = chooseFactionForFirstPlayer.getFirstFaction();
            northFaction = chooseFactionForSecondPlayer.getSecondFaction();

            //Names
            southName = chooseFactionForFirstPlayer.getFirstPlayersName();
            northName = chooseFactionForSecondPlayer.getSecondPlayersName();
            if (string.IsNullOrEmpty(southName)) southName = "Gracz 1";
            if (string.IsNullOrEmpty(northName)) northName = "Gracz 2";
        }
        else
        {
            //Factions
            southFaction = chooseFactionForFirstPlayerSingleMode.getFirstFaction();
            northFaction = chooseFactionForAIPlayer.getSecondFaction();

            //Names
            southName = chooseFactionForFirstPlayerSingleMode.getFirstPlayersName();
            northName = "Gracz AI";
            if (string.IsNullOrEmpty(southName)) southName = "Gracz 1";
        }
    }

    public void AttributeGameManagerNamesAndFactions()
    {
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
}
