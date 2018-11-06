using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager Instance;

    public string northName = "AI";
    public string southName = "Grzegorz";
    public PlayerModel playerSouth;
    public PlayerModel playerNorth;
    public MessageManager messageManager;
    public HandView northHandView;
    public HandView southHandView;
    public GameObject unitCard;
    public GameObject tacticsCard;
    public GameObject visuals;
    public GameObject deckNorth;
    public GameObject deckSouth;
    public GameObject resourcesNorth;
    public GameObject resourcesSouth;
    public GameObject dropzoneNorth;
    public GameObject dropzoneSouth;
    public GameObject mainMenu;
    public Position whoseTurn;
    public bool gameRunning;

    //Dodane przeze mnie skrypty zczytujące dane z menu (imiona i frakcje) - dwóch graczy
    public ChooseFactionScript1 chooseFactionScript1;
    public ChooseFactionScript2 chooseFactionScript2;

    public ChooseFactionScript1 chooseFactionScriptWithComputer1;
    public ChooseFactionScriptWithComputer chooseFactionScriptWithComputer2;
    public Faction firstFaction;
    public Faction secondFaction;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        visuals.SetActive(false);
        mainMenu.SetActive(true);
        

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            mainMenu.SetActive(false);
            visuals.SetActive(true);
            StartCoroutine(startGame());
        }*/

    }

    // Use this for initialization
    IEnumerator startGame()
    {
        while (messageManager.enabled == false)
        {
            yield return new WaitForSeconds(0.05f);
        }
        InitializeGame();
        messageManager.playerSouthName = southName;


        //// ----------draw 4 cards from deck to Player South
        for (int i=0;i<2; i++)
        {            
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);             
            }

            Card cardDrawn = playerSouth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
            southHandView.MoveDrawnCardFromDeckToHand(cardDrawn, playerSouth, deckSouth);
        }

        //// ----------draw 4 cards from deck to Player North
        for (int i = 0; i < 2; i++)
        {
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);
            }

            Card cardDrawn = playerNorth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
            northHandView.MoveDrawnCardFromDeckToHand(cardDrawn, playerNorth, deckNorth);
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        northHandView.blockAllOperations();
        southHandView.blockAllOperations();
        whoseTurn = Position.North;
        this.nextTurn();
    }

    public void nextTurn()
    {
        if (whoseTurn == Position.North)
        {
            whoseTurn = Position.South;
        }
        else if (whoseTurn == Position.South)
        {
            whoseTurn = Position.North;
        }
        if (whoseTurn == Position.South)
        {
            northHandView.blockAllOperations();
            southHandView.blockAllOperations();
            messageManager.ShowMessage(southName + "! Twoja tura", 2f);
            playerSouth.updateResourcesNewTurn();
            resourcesSouth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
            southHandView.setPlayableCards(playerSouth.resourcesCurrent);
        }
        if (whoseTurn == Position.North)
        {
            northHandView.blockAllOperations();
            southHandView.blockAllOperations();
            messageManager.ShowMessage(northName + "! Twoja tura", 2f);
            playerNorth.updateResourcesNewTurn();
            resourcesNorth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
            northHandView.setPlayableCards(playerSouth.resourcesCurrent);
        }
    }

    void InitializeGame()
    {
        Debug.Log("GameManger INITIALIZATION");
        gameRunning = true;
        IDFactory.ResetIDs();
        //Dodane przypiasanie frakcji
        firstFaction = chooseFactionScript1.getFirstFaction();
        secondFaction = chooseFactionScript2.getSecondFaction();
        southName = chooseFactionScript1.getFirstPlayersName();
        northName = chooseFactionScript2.getSecondPlayersName();

        playerNorth = new PlayerModel(0, "Cooper", secondFaction, Position.North);
        playerSouth = new PlayerModel(1, "Johnson", firstFaction, Position.South);
        resourcesNorth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
        resourcesSouth.GetComponent<ResourcePool>().updateResourcesView(playerSouth.resourcesCurrent, playerSouth.resourcesMaxThisTurn);
    }

    public void cardDraggedToFrontCommand(Position playerPosition, int cardId)
    {
        if(playerPosition == Position.North)
        {
            playerNorth.armymodel.armyCardsModel.moveCardFromHandToFront(cardId);
        }
        else if (playerPosition == Position.South)
        {
            playerSouth.armymodel.armyCardsModel.moveCardFromHandToFront(cardId);
        }
    }

    public void StartGameWithCouroutine()
    {
        StartCoroutine(startGame());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
